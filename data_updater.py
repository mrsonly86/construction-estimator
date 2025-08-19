#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
Module cập nhật dữ liệu định mức từ các bộ (BXD, BGT, BNN...)
Tích hợp API và crawling dữ liệu chính thức
"""

import requests
import sqlite3
import json
import pandas as pd
from datetime import datetime, timedelta
import threading
import time
import os
from pathlib import Path
import hashlib

class DataUpdater:
    def __init__(self, db_path='construction_estimate.db'):
        self.db_path = db_path
        self.conn = sqlite3.connect(db_path, check_same_thread=False)
        self.cursor = self.conn.cursor()
        self.setup_update_tables()
        
        # URLs của các bộ (giả lập - cần thay thế bằng API thực)
        self.data_sources = {
            'BXD': {
                'name': 'Bộ Xây dựng',
                'url': 'https://api.bxd.gov.vn/dinhmuc',
                'update_frequency': 30,  # days
                'categories': ['Xây dựng dân dụng', 'Hạ tầng kỹ thuật', 'Công trình công nghiệp']
            },
            'BGT': {
                'name': 'Bộ Giao thông Vận tải',
                'url': 'https://api.bgt.gov.vn/dinhmuc',
                'update_frequency': 45,
                'categories': ['Đường bộ', 'Cầu đường', 'Sân bay', 'Cảng biển']
            },
            'BNN': {
                'name': 'Bộ Nông nghiệp và Phát triển nông thôn',
                'url': 'https://api.bnn.gov.vn/dinhmuc',
                'update_frequency': 60,
                'categories': ['Thủy lợi', 'Nông nghiệp', 'Lâm nghiệp', 'Thủy sản']
            },
            'BKH': {
                'name': 'Bộ Kế hoạch và Đầu tư',
                'url': 'https://api.bkh.gov.vn/dinhmuc',
                'update_frequency': 90,
                'categories': ['Định mức chung', 'Giá vật liệu', 'Chỉ số giá']
            }
        }
    
    def setup_update_tables(self):
        """Tạo bảng theo dõi cập nhật"""
        self.cursor.execute('''
            CREATE TABLE IF NOT EXISTS data_updates (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                source TEXT NOT NULL,
                last_update TEXT,
                next_update TEXT,
                status TEXT DEFAULT 'pending',
                records_count INTEGER DEFAULT 0,
                checksum TEXT
            )
        ''')
        
        self.cursor.execute('''
            CREATE TABLE IF NOT EXISTS norm_sources (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                norm_code TEXT,
                source TEXT,
                official_code TEXT,
                last_updated TEXT,
                price_history TEXT,
                FOREIGN KEY (norm_code) REFERENCES norms (code)
            )
        ''')
        
        # Bảng lưu lịch sử giá
        self.cursor.execute('''
            CREATE TABLE IF NOT EXISTS price_history (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                norm_code TEXT,
                price REAL,
                date TEXT,
                source TEXT,
                region TEXT DEFAULT 'Toàn quốc'
            )
        ''')
        
        self.conn.commit()
    
    def fetch_official_data(self, source_key):
        """Lấy dữ liệu từ nguồn chính thức"""
        try:
            source = self.data_sources[source_key]
            
            # Giả lập API call (thực tế cần thay thế bằng API thực)
            # response = requests.get(source['url'], timeout=30)
            
            # Dữ liệu mẫu từ các bộ
            sample_data = self.generate_sample_official_data(source_key)
            
            return True, sample_data
            
        except Exception as e:
            return False, f"Lỗi kết nối {source_key}: {str(e)}"
    
    def generate_sample_official_data(self, source_key):
        """Tạo dữ liệu mẫu từ các bộ (thay thế bằng API thực)"""
        
        bxd_data = [
            {'code': 'BXD.A.1.1.1', 'name': 'Đào đất thủ công loại I', 'unit': 'm³', 'price': 155000, 'category': 'Đào đất'},
            {'code': 'BXD.A.1.1.2', 'name': 'Đào đất thủ công loại II', 'unit': 'm³', 'price': 165000, 'category': 'Đào đất'},
            {'code': 'BXD.A.2.1.1', 'name': 'Bê tông C15 đổ tại chỗ', 'unit': 'm³', 'price': 1650000, 'category': 'Bê tông'},
            {'code': 'BXD.A.2.1.2', 'name': 'Bê tông C20 đổ tại chỗ', 'unit': 'm³', 'price': 1850000, 'category': 'Bê tông'},
            {'code': 'BXD.A.2.1.3', 'name': 'Bê tông C25 đổ tại chỗ', 'unit': 'm³', 'price': 2100000, 'category': 'Bê tông'},
            {'code': 'BXD.A.3.1.1', 'name': 'Thép CB240-T phi 6', 'unit': 'kg', 'price': 21500, 'category': 'Thép'},
            {'code': 'BXD.A.3.1.2', 'name': 'Thép CB300-V phi 10', 'unit': 'kg', 'price': 23000, 'category': 'Thép'},
            {'code': 'BXD.A.4.1.1', 'name': 'Gạch ống đất sét nung 220x105x60', 'unit': 'viên', 'price': 4800, 'category': 'Gạch'},
        ]
        
        bgt_data = [
            {'code': 'BGT.D.1.1.1', 'name': 'Đào đất nền đường cấp III', 'unit': 'm³', 'price': 145000, 'category': 'Đường bộ'},
            {'code': 'BGT.D.1.2.1', 'name': 'Đắp đất nền đường', 'unit': 'm³', 'price': 135000, 'category': 'Đường bộ'},
            {'code': 'BGT.D.2.1.1', 'name': 'Bê tông nhựa AC loại I', 'unit': 'm³', 'price': 2850000, 'category': 'Mặt đường'},
            {'code': 'BGT.C.1.1.1', 'name': 'Cọc khoan nhồi D80cm', 'unit': 'm', 'price': 850000, 'category': 'Cầu'},
            {'code': 'BGT.C.2.1.1', 'name': 'Dầm BTCT dự ứng lực', 'unit': 'm³', 'price': 3200000, 'category': 'Cầu'},
        ]
        
        bnn_data = [
            {'code': 'BNN.T.1.1.1', 'name': 'Đào kênh mương đất cấp II', 'unit': 'm³', 'price': 125000, 'category': 'Thủy lợi'},
            {'code': 'BNN.T.1.2.1', 'name': 'Xây tường kè bằng đá hộc', 'unit': 'm³', 'price': 1850000, 'category': 'Thủy lợi'},
            {'code': 'BNN.T.2.1.1', 'name': 'Bê tông M200 cho công trình thủy lợi', 'unit': 'm³', 'price': 1750000, 'category': 'Bê tông'},
            {'code': 'BNN.N.1.1.1', 'name': 'Làm đất trồng lúa', 'unit': 'ha', 'price': 12500000, 'category': 'Nông nghiệp'},
        ]
        
        bkh_data = [
            {'code': 'BKH.G.1.1.1', 'name': 'Xi măng PCB40', 'unit': 'tấn', 'price': 2850000, 'category': 'Vật liệu'},
            {'code': 'BKH.G.1.2.1', 'name': 'Thép xây dựng CB300-V', 'unit': 'tấn', 'price': 18500000, 'category': 'Vật liệu'},
            {'code': 'BKH.G.1.3.1', 'name': 'Cát xây dựng', 'unit': 'm³', 'price': 285000, 'category': 'Vật liệu'},
            {'code': 'BKH.G.1.4.1', 'name': 'Đá dăm 1x2', 'unit': 'm³', 'price': 385000, 'category': 'Vật liệu'},
        ]
        
        data_map = {
            'BXD': bxd_data,
            'BGT': bgt_data, 
            'BNN': bnn_data,
            'BKH': bkh_data
        }
        
        return data_map.get(source_key, [])
    
    def update_norms_from_source(self, source_key, data):
        """Cập nhật định mức từ nguồn dữ liệu"""
        try:
            updated_count = 0
            
            for item in data:
                # Kiểm tra định mức đã tồn tại chưa
                self.cursor.execute("SELECT id FROM norms WHERE code = ?", (item['code'],))
                existing = self.cursor.fetchone()
                
                if existing:
                    # Cập nhật giá mới
                    self.cursor.execute("""
                        UPDATE norms SET 
                        total_cost = ?, 
                        name = ?,
                        unit = ?,
                        category = ?
                        WHERE code = ?
                    """, (item['price'], item['name'], item['unit'], item['category'], item['code']))
                else:
                    # Thêm định mức mới
                    self.cursor.execute("""
                        INSERT INTO norms (code, name, unit, total_cost, category, material_cost, labor_cost, machine_cost)
                        VALUES (?, ?, ?, ?, ?, ?, ?, ?)
                    """, (
                        item['code'], item['name'], item['unit'], item['price'], 
                        item['category'], item['price'] * 0.6, item['price'] * 0.3, item['price'] * 0.1
                    ))
                
                # Lưu lịch sử giá
                self.cursor.execute("""
                    INSERT INTO price_history (norm_code, price, date, source)
                    VALUES (?, ?, ?, ?)
                """, (item['code'], item['price'], datetime.now().isoformat(), source_key))
                
                updated_count += 1
            
            # Cập nhật trạng thái
            now = datetime.now().isoformat()
            next_update = (datetime.now() + timedelta(days=self.data_sources[source_key]['update_frequency'])).isoformat()
            
            self.cursor.execute("""
                INSERT OR REPLACE INTO data_updates (source, last_update, next_update, status, records_count)
                VALUES (?, ?, ?, 'completed', ?)
            """, (source_key, now, next_update, updated_count))
            
            self.conn.commit()
            return True, f"Đã cập nhật {updated_count} định mức từ {source_key}"
            
        except Exception as e:
            return False, f"Lỗi cập nhật từ {source_key}: {str(e)}"
    
    def auto_update_all_sources(self):
        """Cập nhật tự động từ tất cả nguồn"""
        results = {}
        
        for source_key in self.data_sources.keys():
            # Kiểm tra xem có cần cập nhật không
            if self.should_update(source_key):
                success, data = self.fetch_official_data(source_key)
                if success:
                    update_success, message = self.update_norms_from_source(source_key, data)
                    results[source_key] = {'success': update_success, 'message': message}
                else:
                    results[source_key] = {'success': False, 'message': data}
            else:
                results[source_key] = {'success': True, 'message': 'Không cần cập nhật'}
        
        return results
    
    def should_update(self, source_key):
        """Kiểm tra xem nguồn có cần cập nhật không"""
        self.cursor.execute("""
            SELECT next_update FROM data_updates 
            WHERE source = ? AND status = 'completed'
            ORDER BY last_update DESC LIMIT 1
        """, (source_key,))
        
        result = self.cursor.fetchone()
        if not result:
            return True  # Chưa từng cập nhật
        
        next_update = datetime.fromisoformat(result[0])
        return datetime.now() >= next_update
    
    def get_update_status(self):
        """Lấy trạng thái cập nhật của tất cả nguồn"""
        self.cursor.execute("""
            SELECT source, last_update, next_update, status, records_count
            FROM data_updates
            ORDER BY last_update DESC
        """)
        
        results = []
        for row in self.cursor.fetchall():
            source_name = self.data_sources.get(row[0], {}).get('name', row[0])
            results.append({
                'source': row[0],
                'source_name': source_name,
                'last_update': row[1],
                'next_update': row[2], 
                'status': row[3],
                'records_count': row[4]
            })
        
        return results
    
    def get_price_trend(self, norm_code, days=365):
        """Lấy xu hướng giá của định mức"""
        start_date = (datetime.now() - timedelta(days=days)).isoformat()
        
        self.cursor.execute("""
            SELECT price, date, source FROM price_history
            WHERE norm_code = ? AND date >= ?
            ORDER BY date ASC
        """, (norm_code, start_date))
        
        history = []
        for row in self.cursor.fetchall():
            history.append({
                'price': row[0],
                'date': row[1],
                'source': row[2]
            })
        
        return history
    
    def start_background_updater(self):
        """Bắt đầu cập nhật tự động trong background"""
        def update_worker():
            while True:
                try:
                    results = self.auto_update_all_sources()
                    print(f"[{datetime.now()}] Cập nhật tự động hoàn thành: {results}")
                    
                    # Nghỉ 24 giờ trước khi cập nhật tiếp
                    time.sleep(24 * 3600)
                    
                except Exception as e:
                    print(f"[{datetime.now()}] Lỗi cập nhật tự động: {str(e)}")
                    time.sleep(3600)  # Nghỉ 1 giờ nếu có lỗi
        
        update_thread = threading.Thread(target=update_worker, daemon=True)
        update_thread.start()
        return update_thread


class RegionalPriceManager:
    """Quản lý giá theo vùng miền"""
    
    def __init__(self, db_path='construction_estimate.db'):
        self.db_path = db_path
        self.conn = sqlite3.connect(db_path, check_same_thread=False)
        self.cursor = self.conn.cursor()
        self.setup_regional_tables()
        
        # Hệ số điều chỉnh theo vùng
        self.regional_factors = {
            'Hà Nội': 1.15,
            'TP.HCM': 1.20,
            'Đà Nẵng': 1.10,
            'Hải Phòng': 1.08,
            'Cần Thơ': 1.05,
            'Miền Bắc': 1.00,
            'Miền Trung': 0.95,
            'Miền Nam': 1.02,
            'Vùng sâu vùng xa': 0.90
        }
    
    def setup_regional_tables(self):
        """Tạo bảng quản lý giá theo vùng"""
        self.cursor.execute('''
            CREATE TABLE IF NOT EXISTS regional_prices (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                norm_code TEXT,
                region TEXT,
                base_price REAL,
                adjusted_price REAL,
                factor REAL,
                last_updated TEXT,
                FOREIGN KEY (norm_code) REFERENCES norms (code)
            )
        ''')
        self.conn.commit()
    
    def calculate_regional_price(self, norm_code, base_price, region):
        """Tính giá theo vùng"""
        factor = self.regional_factors.get(region, 1.0)
        adjusted_price = base_price * factor
        
        # Lưu vào database
        self.cursor.execute("""
            INSERT OR REPLACE INTO regional_prices 
            (norm_code, region, base_price, adjusted_price, factor, last_updated)
            VALUES (?, ?, ?, ?, ?, ?)
        """, (norm_code, region, base_price, adjusted_price, factor, datetime.now().isoformat()))
        
        self.conn.commit()
        return adjusted_price
    
    def get_regional_prices(self, norm_code):
        """Lấy giá của định mức theo tất cả vùng"""
        self.cursor.execute("""
            SELECT region, base_price, adjusted_price, factor 
            FROM regional_prices WHERE norm_code = ?
        """, (norm_code,))
        
        return [{'region': row[0], 'base_price': row[1], 'adjusted_price': row[2], 'factor': row[3]} 
                for row in self.cursor.fetchall()]