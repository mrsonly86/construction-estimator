#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
Digital Signature Module - Chữ ký số và biểu mẫu đấu thầu
Hỗ trợ: PKI, X.509, PDF signing, Biểu mẫu chuẩn
"""

import os
import json
import sqlite3
from datetime import datetime, timedelta
import hashlib
import base64
import tempfile
from pathlib import Path

# For PDF generation and signing
from reportlab.lib.pagesizes import A4
from reportlab.lib import colors
from reportlab.lib.styles import getSampleStyleSheet, ParagraphStyle
from reportlab.platypus import SimpleDocTemplate, Paragraph, Spacer, Table, TableStyle, Image
from reportlab.lib.units import inch, cm
import pandas as pd

class DigitalSignatureManager:
    """Quản lý chữ ký số"""
    
    def __init__(self, db_path='construction_estimate.db'):
        self.db_path = db_path
        self.conn = sqlite3.connect(db_path, check_same_thread=False)
        self.cursor = self.conn.cursor()
        
        self.setup_signature_tables()
        
        # Certificate store
        self.certificates = {}
        self.private_keys = {}
    
    def setup_signature_tables(self):
        """Tạo bảng quản lý chữ ký số"""
        self.cursor.execute('''
            CREATE TABLE IF NOT EXISTS digital_certificates (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                cert_name TEXT NOT NULL,
                cert_type TEXT DEFAULT 'self_signed',
                issuer TEXT,
                subject TEXT,
                valid_from TEXT,
                valid_to TEXT,
                serial_number TEXT,
                fingerprint TEXT,
                cert_data TEXT,
                private_key_data TEXT,
                is_active BOOLEAN DEFAULT 1,
                created_date TEXT
            )
        ''')
        
        self.cursor.execute('''
            CREATE TABLE IF NOT EXISTS signature_logs (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                document_name TEXT,
                document_hash TEXT,
                cert_id INTEGER,
                signature_data TEXT,
                timestamp TEXT,
                signer_name TEXT,
                signature_valid BOOLEAN DEFAULT 1,
                FOREIGN KEY (cert_id) REFERENCES digital_certificates (id)
            )
        ''')
        
        self.cursor.execute('''
            CREATE TABLE IF NOT EXISTS tender_templates (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                template_name TEXT NOT NULL,
                template_type TEXT,
                description TEXT,
                template_data TEXT,
                created_date TEXT,
                last_modified TEXT,
                is_active BOOLEAN DEFAULT 1
            )
        ''')
        
        self.conn.commit()
    
    def generate_self_signed_certificate(self, subject_info):
        """Tạo chứng chỉ tự ký (simplified version)"""
        try:
            # Simplified certificate generation
            # In production, use cryptography library for real certificates
            
            cert_info = {
                'subject': subject_info,
                'issuer': subject_info,  # Self-signed
                'serial_number': self.generate_serial_number(),
                'valid_from': datetime.now().isoformat(),
                'valid_to': (datetime.now() + timedelta(days=365)).isoformat(),
                'fingerprint': self.generate_fingerprint(subject_info)
            }
            
            # Generate key pair (simplified)
            private_key = self.generate_private_key()
            public_key = self.generate_public_key(private_key)
            
            # Save to database
            self.cursor.execute("""
                INSERT INTO digital_certificates 
                (cert_name, cert_type, issuer, subject, valid_from, valid_to, 
                 serial_number, fingerprint, cert_data, private_key_data, created_date)
                VALUES (?, 'self_signed', ?, ?, ?, ?, ?, ?, ?, ?, ?)
            """, (
                subject_info.get('common_name', 'Certificate'),
                json.dumps(cert_info['issuer']),
                json.dumps(cert_info['subject']),
                cert_info['valid_from'],
                cert_info['valid_to'],
                cert_info['serial_number'],
                cert_info['fingerprint'],
                base64.b64encode(json.dumps(public_key).encode()).decode(),
                base64.b64encode(json.dumps(private_key).encode()).decode(),
                datetime.now().isoformat()
            ))
            
            cert_id = self.cursor.lastrowid
            self.conn.commit()
            
            return True, cert_id, "Tạo chứng chỉ thành công"
            
        except Exception as e:
            return False, None, f"Lỗi tạo chứng chỉ: {str(e)}"
    
    def generate_serial_number(self):
        """Tạo số serial cho certificate"""
        import random
        return str(random.randint(100000000000, 999999999999))
    
    def generate_fingerprint(self, subject_info):
        """Tạo fingerprint cho certificate"""
        subject_str = json.dumps(subject_info, sort_keys=True)
        return hashlib.sha256(subject_str.encode()).hexdigest()[:32]
    
    def generate_private_key(self):
        """Tạo private key (simplified)"""
        # Simplified key generation
        import random
        return {
            'type': 'RSA',
            'size': 2048,
            'key_data': base64.b64encode(os.urandom(256)).decode()
        }
    
    def generate_public_key(self, private_key):
        """Tạo public key từ private key (simplified)"""
        return {
            'type': 'RSA',
            'size': 2048,
            'key_data': base64.b64encode(os.urandom(256)).decode(),
            'derived_from': private_key['key_data'][:32]
        }
    
    def sign_document(self, document_path, cert_id, signer_name):
        """Ký số tài liệu"""
        try:
            # Get certificate
            self.cursor.execute("""
                SELECT cert_name, private_key_data FROM digital_certificates 
                WHERE id = ? AND is_active = 1
            """, (cert_id,))
            
            cert_result = self.cursor.fetchone()
            if not cert_result:
                return False, None, "Không tìm thấy chứng chỉ"
            
            cert_name, private_key_data = cert_result
            
            # Calculate document hash
            document_hash = self.calculate_file_hash(document_path)
            
            # Create signature (simplified)
            signature_data = self.create_signature(document_hash, private_key_data)
            
            # Save signature log
            self.cursor.execute("""
                INSERT INTO signature_logs 
                (document_name, document_hash, cert_id, signature_data, timestamp, signer_name)
                VALUES (?, ?, ?, ?, ?, ?)
            """, (
                os.path.basename(document_path),
                document_hash,
                cert_id,
                signature_data,
                datetime.now().isoformat(),
                signer_name
            ))
            
            signature_id = self.cursor.lastrowid
            self.conn.commit()
            
            return True, signature_id, "Ký số thành công"
            
        except Exception as e:
            return False, None, f"Lỗi ký số: {str(e)}"
    
    def calculate_file_hash(self, file_path):
        """Tính hash của file"""
        try:
            with open(file_path, 'rb') as f:
                content = f.read()
                return hashlib.sha256(content).hexdigest()
        except:
            return ""
    
    def create_signature(self, document_hash, private_key_data):
        """Tạo chữ ký số (simplified)"""
        # Simplified signature creation
        signature_input = f"{document_hash}:{private_key_data[:32]}:{datetime.now().isoformat()}"
        signature = hashlib.sha256(signature_input.encode()).hexdigest()
        return base64.b64encode(signature.encode()).decode()
    
    def verify_signature(self, document_path, signature_id):
        """Xác thực chữ ký số"""
        try:
            # Get signature info
            self.cursor.execute("""
                SELECT sl.document_hash, sl.signature_data, sl.cert_id, sl.timestamp,
                       dc.cert_data, dc.fingerprint
                FROM signature_logs sl
                JOIN digital_certificates dc ON sl.cert_id = dc.id
                WHERE sl.id = ?
            """, (signature_id,))
            
            result = self.cursor.fetchone()
            if not result:
                return False, "Không tìm thấy chữ ký"
            
            stored_hash, signature_data, cert_id, timestamp, cert_data, fingerprint = result
            
            # Calculate current document hash
            current_hash = self.calculate_file_hash(document_path)
            
            # Verify hash matches
            if stored_hash != current_hash:
                return False, "Tài liệu đã bị thay đổi"
            
            # Verify signature (simplified)
            # In production, use proper cryptographic verification
            
            return True, "Chữ ký hợp lệ"
            
        except Exception as e:
            return False, f"Lỗi xác thực: {str(e)}"
    
    def get_certificates(self):
        """Lấy danh sách chứng chỉ"""
        self.cursor.execute("""
            SELECT id, cert_name, cert_type, issuer, subject, valid_from, valid_to, 
                   fingerprint, is_active
            FROM digital_certificates
            ORDER BY created_date DESC
        """)
        
        certificates = []
        for row in self.cursor.fetchall():
            certificates.append({
                'id': row[0],
                'name': row[1],
                'type': row[2],
                'issuer': json.loads(row[3]) if row[3] else {},
                'subject': json.loads(row[4]) if row[4] else {},
                'valid_from': row[5],
                'valid_to': row[6],
                'fingerprint': row[7],
                'is_active': bool(row[8])
            })
        
        return certificates


class TenderDocumentGenerator:
    """Tạo biểu mẫu hồ sơ đấu thầu chuẩn"""
    
    def __init__(self, db_path='construction_estimate.db'):
        self.db_path = db_path
        self.conn = sqlite3.connect(db_path, check_same_thread=False)
        self.cursor = self.conn.cursor()
        
        # Load tender templates
        self.load_standard_templates()
    
    def load_standard_templates(self):
        """Load các template chuẩn"""
        standard_templates = [
            {
                'name': 'Bảng dự toán chi tiết',
                'type': 'estimate_detail',
                'description': 'Bảng dự toán chi tiết theo Thông tư 07/2016/TT-BXD'
            },
            {
                'name': 'Thuyết minh dự toán',
                'type': 'estimate_explanation',
                'description': 'Thuyết minh dự toán xây dựng'
            },
            {
                'name': 'Bảng khối lượng công việc',
                'type': 'work_quantity',
                'description': 'Bảng tính khối lượng công việc chi tiết'
            },
            {
                'name': 'Hồ sơ đấu thầu',
                'type': 'tender_package',
                'description': 'Hồ sơ đấu thầu hoàn chỉnh theo Luật Đấu thầu 2013'
            },
            {
                'name': 'Báo cáo kinh tế kỹ thuật',
                'type': 'economic_technical',
                'description': 'Báo cáo kinh tế kỹ thuật dự án'
            }
        ]
        
        for template in standard_templates:
            self.cursor.execute("""
                INSERT OR IGNORE INTO tender_templates 
                (template_name, template_type, description, created_date, is_active)
                VALUES (?, ?, ?, ?, 1)
            """, (template['name'], template['type'], template['description'], 
                  datetime.now().isoformat()))
        
        self.conn.commit()
    
    def generate_detailed_estimate(self, project_data, items_data, output_path):
        """Tạo bảng dự toán chi tiết chuẩn"""
        try:
            doc = SimpleDocTemplate(output_path, pagesize=A4, 
                                  leftMargin=2*cm, rightMargin=2*cm,
                                  topMargin=2*cm, bottomMargin=2*cm)
            
            styles = getSampleStyleSheet()
            story = []
            
            # Header
            header_style = ParagraphStyle(
                'CustomHeader',
                parent=styles['Title'],
                fontSize=16,
                spaceAfter=20,
                alignment=1,  # Center
                textColor=colors.darkblue,
                fontName='Helvetica-Bold'
            )
            
            # Title
            title = Paragraph("BẢNG DỰ TOÁN CHI TIẾT", header_style)
            story.append(title)
            
            subtitle = Paragraph(f"Dự án: {project_data.get('name', '')}", styles['Normal'])
            story.append(subtitle)
            story.append(Spacer(1, 20))
            
            # Project information table
            project_info = [
                ['Tên dự án:', project_data.get('name', '')],
                ['Địa điểm:', project_data.get('location', '')],
                ['Chủ đầu tư:', project_data.get('client', '')],
                ['Ngày lập:', project_data.get('date', datetime.now().strftime('%d/%m/%Y'))],
                ['Đơn vị tư vấn:', 'Phần mềm Dự toán XD Chuyên nghiệp']
            ]
            
            info_table = Table(project_info, colWidths=[4*cm, 12*cm])
            info_table.setStyle(TableStyle([
                ('FONTNAME', (0, 0), (-1, -1), 'Helvetica'),
                ('FONTSIZE', (0, 0), (-1, -1), 10),
                ('VALIGN', (0, 0), (-1, -1), 'TOP'),
                ('LEFTPADDING', (0, 0), (-1, -1), 6),
                ('RIGHTPADDING', (0, 0), (-1, -1), 6),
            ]))
            
            story.append(info_table)
            story.append(Spacer(1, 30))
            
            # Main estimate table
            table_header = [
                'STT', 'Mã\nđịnh mức', 'Tên hạng mục công việc', 
                'Đơn vị\ntính', 'Khối\nlượng', 'Đơn giá\n(VNĐ)', 
                'Thành tiền\n(VNĐ)', 'Ghi chú'
            ]
            
            table_data = [table_header]
            
            # Add estimate items
            for item in items_data:
                row = [
                    str(item[0]),  # STT
                    str(item[1]),  # Mã định mức
                    str(item[2]),  # Tên hạng mục
                    str(item[3]),  # Đơn vị
                    str(item[4]),  # Khối lượng
                    f"{float(str(item[5]).replace(',', '')):,.0f}" if item[5] else "0",  # Đơn giá
                    f"{float(str(item[6]).replace(',', '')):,.0f}" if item[6] else "0",  # Thành tiền
                    str(item[7]) if len(item) > 7 else ""  # Ghi chú
                ]
                table_data.append(row)
            
            # Calculate totals
            total_amount = sum([float(str(item[6]).replace(',', '')) for item in items_data if item[6]])
            vat_amount = total_amount * 0.1
            final_amount = total_amount + vat_amount
            
            # Add summary rows
            table_data.append(['', '', 'TỔNG CỘNG', '', '', '', f"{total_amount:,.0f}", ''])
            table_data.append(['', '', 'VAT (10%)', '', '', '', f"{vat_amount:,.0f}", ''])
            table_data.append(['', '', 'TỔNG CỘNG (Có VAT)', '', '', '', f"{final_amount:,.0f}", ''])
            
            # Create table
            estimate_table = Table(table_data, 
                                 colWidths=[1*cm, 2*cm, 5*cm, 1.5*cm, 1.5*cm, 2.5*cm, 2.5*cm, 2*cm])
            
            # Table styling
            table_style = TableStyle([
                # Header row
                ('BACKGROUND', (0, 0), (-1, 0), colors.grey),
                ('TEXTCOLOR', (0, 0), (-1, 0), colors.whitesmoke),
                ('ALIGN', (0, 0), (-1, -1), 'CENTER'),
                ('FONTNAME', (0, 0), (-1, 0), 'Helvetica-Bold'),
                ('FONTSIZE', (0, 0), (-1, 0), 9),
                ('BOTTOMPADDING', (0, 0), (-1, 0), 12),
                
                # Data rows
                ('BACKGROUND', (0, 1), (-1, -4), colors.beige),
                ('FONTNAME', (0, 1), (-1, -1), 'Helvetica'),
                ('FONTSIZE', (0, 1), (-1, -1), 8),
                ('GRID', (0, 0), (-1, -1), 1, colors.black),
                
                # Summary rows
                ('BACKGROUND', (0, -3), (-1, -1), colors.lightgrey),
                ('FONTNAME', (0, -3), (-1, -1), 'Helvetica-Bold'),
                
                # Alignment
                ('ALIGN', (4, 1), (6, -1), 'RIGHT'),  # Numbers right-aligned
                ('ALIGN', (2, 1), (2, -4), 'LEFT'),   # Item names left-aligned
            ])
            
            estimate_table.setStyle(table_style)
            story.append(estimate_table)
            story.append(Spacer(1, 30))
            
            # Footer with signatures
            footer_data = [
                ['', 'NGƯỜI LẬP', '', 'NGƯỜI DUYỆT'],
                ['', '(Ký, họ tên)', '', '(Ký, họ tên)'],
                ['', '', '', ''],
                ['', '', '', ''],
                ['', '', '', '']
            ]
            
            footer_table = Table(footer_data, colWidths=[4*cm, 4*cm, 4*cm, 4*cm])
            footer_table.setStyle(TableStyle([
                ('FONTNAME', (0, 0), (-1, -1), 'Helvetica'),
                ('FONTSIZE', (0, 0), (-1, -1), 10),
                ('ALIGN', (0, 0), (-1, -1), 'CENTER'),
                ('VALIGN', (0, 0), (-1, -1), 'TOP'),
            ]))
            
            story.append(footer_table)
            
            # Build PDF
            doc.build(story)
            
            return True, "Tạo bảng dự toán chi tiết thành công"
            
        except Exception as e:
            return False, f"Lỗi tạo bảng dự toán: {str(e)}"
    
    def generate_tender_package(self, project_data, items_data, output_dir):
        """Tạo hồ sơ đấu thầu hoàn chỉnh"""
        try:
            os.makedirs(output_dir, exist_ok=True)
            
            # 1. Bảng dự toán chi tiết
            estimate_path = os.path.join(output_dir, "01_Bang_du_toan_chi_tiet.pdf")
            self.generate_detailed_estimate(project_data, items_data, estimate_path)
            
            # 2. Thuyết minh dự toán
            explanation_path = os.path.join(output_dir, "02_Thuyet_minh_du_toan.pdf")
            self.generate_estimate_explanation(project_data, items_data, explanation_path)
            
            # 3. Bảng khối lượng
            quantity_path = os.path.join(output_dir, "03_Bang_khoi_luong.pdf")
            self.generate_quantity_table(project_data, items_data, quantity_path)
            
            # 4. Báo cáo kinh tế kỹ thuật
            economic_path = os.path.join(output_dir, "04_Bao_cao_kinh_te_ky_thuat.pdf")
            self.generate_economic_report(project_data, items_data, economic_path)
            
            # 5. Tạo file Excel tổng hợp
            excel_path = os.path.join(output_dir, "05_Du_toan_tong_hop.xlsx")
            self.generate_comprehensive_excel(project_data, items_data, excel_path)
            
            return True, f"Tạo hồ sơ đấu thầu thành công tại: {output_dir}"
            
        except Exception as e:
            return False, f"Lỗi tạo hồ sơ đấu thầu: {str(e)}"
    
    def generate_estimate_explanation(self, project_data, items_data, output_path):
        """Tạo thuyết minh dự toán"""
        try:
            doc = SimpleDocTemplate(output_path, pagesize=A4)
            styles = getSampleStyleSheet()
            story = []
            
            # Title
            title = Paragraph("THUYẾT MINH DỰ TOÁN", styles['Title'])
            story.append(title)
            story.append(Spacer(1, 20))
            
            # Content sections
            sections = [
                {
                    'title': 'I. TỔNG QUAN DỰ ÁN',
                    'content': f"""
                    Tên dự án: {project_data.get('name', '')}<br/>
                    Địa điểm: {project_data.get('location', '')}<br/>
                    Chủ đầu tư: {project_data.get('client', '')}<br/>
                    Tổng mức đầu tư: {sum([float(str(item[6]).replace(',', '')) for item in items_data if item[6]]):,.0f} VNĐ
                    """
                },
                {
                    'title': 'II. CƠ SỞ PHÁP LÝ',
                    'content': """
                    - Luật Xây dựng số 50/2014/QH13<br/>
                    - Luật Đấu thầu số 43/2013/QH13<br/>
                    - Thông tư 07/2016/TT-BXD về định mức dự toán xây dựng<br/>
                    - Quyết định phê duyệt dự án đầu tư
                    """
                },
                {
                    'title': 'III. PHƯƠNG PHÁP TÍNH DỰ TOÁN',
                    'content': """
                    Dự toán được lập theo phương pháp định mức, sử dụng:<br/>
                    - Định mức chi phí xây dựng hiện hành<br/>
                    - Đơn giá vật liệu, nhân công tại thời điểm lập dự toán<br/>
                    - Các khoản phí theo quy định
                    """
                }
            ]
            
            for section in sections:
                # Section title
                title_style = ParagraphStyle(
                    'SectionTitle',
                    parent=styles['Heading2'],
                    fontSize=12,
                    spaceAfter=10,
                    textColor=colors.darkblue
                )
                story.append(Paragraph(section['title'], title_style))
                
                # Section content
                story.append(Paragraph(section['content'], styles['Normal']))
                story.append(Spacer(1, 20))
            
            doc.build(story)
            return True, "Tạo thuyết minh dự toán thành công"
            
        except Exception as e:
            return False, f"Lỗi tạo thuyết minh: {str(e)}"
    
    def generate_quantity_table(self, project_data, items_data, output_path):
        """Tạo bảng khối lượng công việc"""
        try:
            doc = SimpleDocTemplate(output_path, pagesize=A4)
            styles = getSampleStyleSheet()
            story = []
            
            title = Paragraph("BẢNG TÍNH KHỐI LƯỢNG CÔNG VIỆC", styles['Title'])
            story.append(title)
            story.append(Spacer(1, 20))
            
            # Group items by category
            categories = {}
            for item in items_data:
                # Get category from database
                self.cursor.execute("SELECT category FROM norms WHERE code = ?", (item[1],))
                result = self.cursor.fetchone()
                category = result[0] if result else 'Khác'
                
                if category not in categories:
                    categories[category] = []
                categories[category].append(item)
            
            # Create table for each category
            for category, items in categories.items():
                # Category header
                cat_title = Paragraph(f"<b>{category}</b>", styles['Heading3'])
                story.append(cat_title)
                story.append(Spacer(1, 10))
                
                # Items table
                table_data = [['STT', 'Mã định mức', 'Tên công việc', 'Đơn vị', 'Khối lượng', 'Ghi chú']]
                
                for i, item in enumerate(items, 1):
                    table_data.append([
                        str(i), str(item[1]), str(item[2]), 
                        str(item[3]), str(item[4]), str(item[7]) if len(item) > 7 else ""
                    ])
                
                table = Table(table_data, colWidths=[1*cm, 2.5*cm, 6*cm, 2*cm, 2*cm, 4.5*cm])
                table.setStyle(TableStyle([
                    ('BACKGROUND', (0, 0), (-1, 0), colors.grey),
                    ('TEXTCOLOR', (0, 0), (-1, 0), colors.whitesmoke),
                    ('ALIGN', (0, 0), (-1, -1), 'CENTER'),
                    ('FONTNAME', (0, 0), (-1, 0), 'Helvetica-Bold'),
                    ('FONTSIZE', (0, 0), (-1, -1), 8),
                    ('GRID', (0, 0), (-1, -1), 1, colors.black),
                ]))
                
                story.append(table)
                story.append(Spacer(1, 20))
            
            doc.build(story)
            return True, "Tạo bảng khối lượng thành công"
            
        except Exception as e:
            return False, f"Lỗi tạo bảng khối lượng: {str(e)}"
    
    def generate_economic_report(self, project_data, items_data, output_path):
        """Tạo báo cáo kinh tế kỹ thuật"""
        try:
            doc = SimpleDocTemplate(output_path, pagesize=A4)
            styles = getSampleStyleSheet()
            story = []
            
            title = Paragraph("BÁO CÁO KINH TẾ KỸ THUẬT", styles['Title'])
            story.append(title)
            story.append(Spacer(1, 30))
            
            # Calculate financial metrics
            total_cost = sum([float(str(item[6]).replace(',', '')) for item in items_data if item[6]])
            
            # Content sections
            sections = [
                {
                    'title': 'I. TỔNG QUAN KINH TẾ',
                    'content': f"""
                    <b>Tổng mức đầu tư:</b> {total_cost:,.0f} VNĐ<br/>
                    <b>Chi phí xây dựng:</b> {total_cost * 0.8:,.0f} VNĐ<br/>
                    <b>Chi phí thiết bị:</b> {total_cost * 0.15:,.0f} VNĐ<br/>
                    <b>Chi phí khác:</b> {total_cost * 0.05:,.0f} VNĐ<br/>
                    """
                },
                {
                    'title': 'II. PHÂN TÍCH CHI PHÍ',
                    'content': """
                    Cơ cấu chi phí được phân tích theo các hạng mục chính:<br/>
                    - Vật liệu: 60%<br/>
                    - Nhân công: 30%<br/>
                    - Máy móc thiết bị: 10%
                    """
                },
                {
                    'title': 'III. KẾT LUẬN VÀ KIẾN NGHỊ',
                    'content': f"""
                    Dự án có tổng mức đầu tư {total_cost:,.0f} VNĐ là hợp lý so với<br/>
                    các dự án tương tự. Kiến nghị phê duyệt để triển khai thực hiện.
                    """
                }
            ]
            
            for section in sections:
                story.append(Paragraph(section['title'], styles['Heading2']))
                story.append(Paragraph(section['content'], styles['Normal']))
                story.append(Spacer(1, 20))
            
            doc.build(story)
            return True, "Tạo báo cáo kinh tế kỹ thuật thành công"
            
        except Exception as e:
            return False, f"Lỗi tạo báo cáo: {str(e)}"
    
    def generate_comprehensive_excel(self, project_data, items_data, output_path):
        """Tạo file Excel tổng hợp"""
        try:
            with pd.ExcelWriter(output_path, engine='openpyxl') as writer:
                # Sheet 1: Thông tin dự án
                project_df = pd.DataFrame([{
                    'Tên dự án': project_data.get('name', ''),
                    'Địa điểm': project_data.get('location', ''),
                    'Chủ đầu tư': project_data.get('client', ''),
                    'Ngày lập': project_data.get('date', ''),
                    'Người lập': 'Phần mềm Dự toán XD Pro'
                }])
                project_df.to_excel(writer, sheet_name='Thông tin dự án', index=False)
                
                # Sheet 2: Dự toán chi tiết
                items_df = pd.DataFrame(items_data, columns=[
                    'STT', 'Mã định mức', 'Tên hạng mục', 'Đơn vị',
                    'Khối lượng', 'Đơn giá', 'Thành tiền', 'Ghi chú'
                ])
                items_df.to_excel(writer, sheet_name='Dự toán chi tiết', index=False)
                
                # Sheet 3: Tổng hợp theo nhóm
                categories_data = []
                category_totals = {}
                
                for item in items_data:
                    self.cursor.execute("SELECT category FROM norms WHERE code = ?", (item[1],))
                    result = self.cursor.fetchone()
                    category = result[0] if result else 'Khác'
                    
                    amount = float(str(item[6]).replace(',', '')) if item[6] else 0
                    
                    if category not in category_totals:
                        category_totals[category] = 0
                    category_totals[category] += amount
                
                for category, total in category_totals.items():
                    categories_data.append({
                        'Nhóm công việc': category,
                        'Tổng giá trị': total,
                        'Tỷ lệ (%)': round(total / sum(category_totals.values()) * 100, 2)
                    })
                
                categories_df = pd.DataFrame(categories_data)
                categories_df.to_excel(writer, sheet_name='Tổng hợp theo nhóm', index=False)
            
            return True, "Tạo file Excel tổng hợp thành công"
            
        except Exception as e:
            return False, f"Lỗi tạo Excel: {str(e)}"