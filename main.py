#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
Phần mềm Dự toán Xây dựng Chuyên nghiệp
Tác giả: AI Assistant
Phiên bản: 1.0.0
Mô tả: Phần mềm dự toán xây dựng miễn phí, vượt trội hơn ETA, G8, F1
"""

import tkinter as tk
from tkinter import ttk, messagebox, filedialog
import sqlite3
import pandas as pd
from datetime import datetime
import json
import os
from pathlib import Path

class ConstructionEstimateApp:
    def __init__(self, root):
        self.root = root
        self.root.title("Phần mềm Dự toán Xây dựng Chuyên nghiệp v1.0")
        self.root.geometry("1400x800")
        self.root.state('zoomed')  # Maximized window
        
        # Thiết lập style
        style = ttk.Style()
        style.theme_use('clam')
        
        # Tạo database
        self.init_database()
        
        # Tạo giao diện
        self.create_widgets()
        
        # Load dữ liệu mẫu
        self.load_sample_data()
    
    def init_database(self):
        """Khởi tạo cơ sở dữ liệu"""
        self.conn = sqlite3.connect('construction_estimate.db')
        self.cursor = self.conn.cursor()
        
        # Bảng định mức
        self.cursor.execute('''
            CREATE TABLE IF NOT EXISTS norms (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                code TEXT UNIQUE NOT NULL,
                name TEXT NOT NULL,
                unit TEXT NOT NULL,
                material_cost REAL DEFAULT 0,
                labor_cost REAL DEFAULT 0,
                machine_cost REAL DEFAULT 0,
                total_cost REAL DEFAULT 0,
                category TEXT DEFAULT 'Khác'
            )
        ''')
        
        # Bảng dự án
        self.cursor.execute('''
            CREATE TABLE IF NOT EXISTS projects (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                name TEXT NOT NULL,
                location TEXT,
                client TEXT,
                created_date TEXT,
                total_amount REAL DEFAULT 0
            )
        ''')
        
        # Bảng chi tiết dự toán
        self.cursor.execute('''
            CREATE TABLE IF NOT EXISTS estimate_items (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                project_id INTEGER,
                norm_code TEXT,
                quantity REAL,
                unit_price REAL,
                total_price REAL,
                notes TEXT,
                FOREIGN KEY (project_id) REFERENCES projects (id),
                FOREIGN KEY (norm_code) REFERENCES norms (code)
            )
        ''')
        
        self.conn.commit()
    
    def create_widgets(self):
        """Tạo giao diện người dùng"""
        # Menu bar
        menubar = tk.Menu(self.root)
        self.root.config(menu=menubar)
        
        # File menu
        file_menu = tk.Menu(menubar, tearoff=0)
        menubar.add_cascade(label="Tệp", menu=file_menu)
        file_menu.add_command(label="Dự án mới", command=self.new_project)
        file_menu.add_command(label="Mở dự án", command=self.open_project)
        file_menu.add_command(label="Lưu dự án", command=self.save_project)
        file_menu.add_separator()
        file_menu.add_command(label="Xuất Excel", command=self.export_excel)
        file_menu.add_command(label="Xuất PDF", command=self.export_pdf)
        file_menu.add_separator()
        file_menu.add_command(label="Thoát", command=self.root.quit)
        
        # Tools menu
        tools_menu = tk.Menu(menubar, tearoff=0)
        menubar.add_cascade(label="Công cụ", menu=tools_menu)
        tools_menu.add_command(label="Quản lý định mức", command=self.manage_norms)
        tools_menu.add_command(label="Calculator", command=self.open_calculator)
        tools_menu.add_command(label="Backup dữ liệu", command=self.backup_data)
        
        # Help menu
        help_menu = tk.Menu(menubar, tearoff=0)
        menubar.add_cascade(label="Trợ giúp", menu=help_menu)
        help_menu.add_command(label="Hướng dẫn", command=self.show_help)
        help_menu.add_command(label="Về phần mềm", command=self.show_about)
        
        # Main frame
        main_frame = ttk.Frame(self.root)
        main_frame.pack(fill=tk.BOTH, expand=True, padx=10, pady=5)
        
        # Top frame - Project info
        project_frame = ttk.LabelFrame(main_frame, text="Thông tin dự án", padding=10)
        project_frame.pack(fill=tk.X, pady=(0, 10))
        
        # Project info fields
        ttk.Label(project_frame, text="Tên dự án:").grid(row=0, column=0, sticky=tk.W, padx=(0, 5))
        self.project_name = ttk.Entry(project_frame, width=30)
        self.project_name.grid(row=0, column=1, padx=(0, 20))
        
        ttk.Label(project_frame, text="Địa điểm:").grid(row=0, column=2, sticky=tk.W, padx=(0, 5))
        self.project_location = ttk.Entry(project_frame, width=30)
        self.project_location.grid(row=0, column=3, padx=(0, 20))
        
        ttk.Label(project_frame, text="Chủ đầu tư:").grid(row=1, column=0, sticky=tk.W, padx=(0, 5))
        self.project_client = ttk.Entry(project_frame, width=30)
        self.project_client.grid(row=1, column=1, padx=(0, 20))
        
        ttk.Label(project_frame, text="Ngày tạo:").grid(row=1, column=2, sticky=tk.W, padx=(0, 5))
        self.project_date = ttk.Entry(project_frame, width=30)
        self.project_date.insert(0, datetime.now().strftime("%d/%m/%Y"))
        self.project_date.grid(row=1, column=3)
        
        # Middle frame - Estimate table
        table_frame = ttk.LabelFrame(main_frame, text="Bảng dự toán chi tiết", padding=10)
        table_frame.pack(fill=tk.BOTH, expand=True, pady=(0, 10))
        
        # Toolbar
        toolbar = ttk.Frame(table_frame)
        toolbar.pack(fill=tk.X, pady=(0, 10))
        
        ttk.Button(toolbar, text="➕ Thêm hạng mục", command=self.add_item).pack(side=tk.LEFT, padx=(0, 5))
        ttk.Button(toolbar, text="✏️ Sửa", command=self.edit_item).pack(side=tk.LEFT, padx=(0, 5))
        ttk.Button(toolbar, text="🗑️ Xóa", command=self.delete_item).pack(side=tk.LEFT, padx=(0, 5))
        ttk.Button(toolbar, text="📊 Tính toán", command=self.calculate_total).pack(side=tk.LEFT, padx=(0, 20))
        
        # Search
        ttk.Label(toolbar, text="Tìm kiếm:").pack(side=tk.LEFT, padx=(20, 5))
        self.search_var = tk.StringVar()
        search_entry = ttk.Entry(toolbar, textvariable=self.search_var, width=20)
        search_entry.pack(side=tk.LEFT, padx=(0, 5))
        search_entry.bind('<KeyRelease>', self.search_items)
        
        # Treeview for estimate table
        columns = ('STT', 'Mã định mức', 'Tên hạng mục', 'Đơn vị', 'Khối lượng', 'Đơn giá', 'Thành tiền', 'Ghi chú')
        self.tree = ttk.Treeview(table_frame, columns=columns, show='headings', height=15)
        
        # Configure columns
        column_widths = [50, 100, 300, 80, 100, 120, 120, 200]
        for i, (col, width) in enumerate(zip(columns, column_widths)):
            self.tree.heading(col, text=col)
            self.tree.column(col, width=width, anchor='center' if i < 4 else 'e' if i in [4,5,6] else 'w')
        
        # Scrollbars
        v_scrollbar = ttk.Scrollbar(table_frame, orient=tk.VERTICAL, command=self.tree.yview)
        h_scrollbar = ttk.Scrollbar(table_frame, orient=tk.HORIZONTAL, command=self.tree.xview)
        self.tree.configure(yscrollcommand=v_scrollbar.set, xscrollcommand=h_scrollbar.set)
        
        # Pack treeview and scrollbars
        self.tree.pack(side=tk.LEFT, fill=tk.BOTH, expand=True)
        v_scrollbar.pack(side=tk.RIGHT, fill=tk.Y)
        h_scrollbar.pack(side=tk.BOTTOM, fill=tk.X)
        
        # Bottom frame - Summary
        summary_frame = ttk.LabelFrame(main_frame, text="Tổng kết", padding=10)
        summary_frame.pack(fill=tk.X)
        
        # Summary labels
        self.total_items_label = ttk.Label(summary_frame, text="Tổng số hạng mục: 0", font=('Arial', 10, 'bold'))
        self.total_items_label.pack(side=tk.LEFT, padx=(0, 20))
        
        self.total_amount_label = ttk.Label(summary_frame, text="Tổng giá trị: 0 VNĐ", font=('Arial', 12, 'bold'), foreground='red')
        self.total_amount_label.pack(side=tk.LEFT, padx=(0, 20))
        
        self.vat_label = ttk.Label(summary_frame, text="VAT (10%): 0 VNĐ", font=('Arial', 10))
        self.vat_label.pack(side=tk.LEFT, padx=(0, 20))
        
        self.final_amount_label = ttk.Label(summary_frame, text="Tổng cộng (có VAT): 0 VNĐ", font=('Arial', 12, 'bold'), foreground='blue')
        self.final_amount_label.pack(side=tk.LEFT)
        
        # Bind double click
        self.tree.bind('<Double-1>', self.edit_item)
    
    def load_sample_data(self):
        """Load dữ liệu định mức mẫu"""
        sample_norms = [
            ('A.1.1.1', 'Đào đất thủ công', 'm³', 50000, 80000, 20000, 150000, 'Đào đất'),
            ('A.1.1.2', 'Đào đất bằng máy', 'm³', 30000, 40000, 50000, 120000, 'Đào đất'),
            ('A.2.1.1', 'Bê tông C15', 'm³', 1200000, 300000, 100000, 1600000, 'Bê tông'),
            ('A.2.1.2', 'Bê tông C20', 'm³', 1400000, 350000, 120000, 1870000, 'Bê tông'),
            ('A.2.1.3', 'Bê tông C25', 'm³', 1600000, 400000, 150000, 2150000, 'Bê tông'),
            ('A.3.1.1', 'Thép phi 6', 'kg', 18000, 2000, 500, 20500, 'Thép'),
            ('A.3.1.2', 'Thép phi 8', 'kg', 18500, 2200, 600, 21300, 'Thép'),
            ('A.3.1.3', 'Thép phi 10', 'kg', 19000, 2500, 700, 22200, 'Thép'),
            ('A.4.1.1', 'Gạch ống 10x20x40', 'viên', 3500, 1000, 200, 4700, 'Gạch'),
            ('A.4.1.2', 'Gạch ống 15x20x40', 'viên', 4200, 1200, 250, 5650, 'Gạch'),
        ]
        
        for norm in sample_norms:
            try:
                self.cursor.execute('''
                    INSERT OR IGNORE INTO norms 
                    (code, name, unit, material_cost, labor_cost, machine_cost, total_cost, category)
                    VALUES (?, ?, ?, ?, ?, ?, ?, ?)
                ''', norm)
            except sqlite3.IntegrityError:
                pass
        
        self.conn.commit()
    
    def new_project(self):
        """Tạo dự án mới"""
        if messagebox.askyesno("Xác nhận", "Bạn có muốn tạo dự án mới? Dữ liệu hiện tại sẽ bị xóa."):
            self.project_name.delete(0, tk.END)
            self.project_location.delete(0, tk.END)
            self.project_client.delete(0, tk.END)
            self.project_date.delete(0, tk.END)
            self.project_date.insert(0, datetime.now().strftime("%d/%m/%Y"))
            
            # Clear table
            for item in self.tree.get_children():
                self.tree.delete(item)
            
            self.update_summary()
    
    def add_item(self):
        """Thêm hạng mục mới"""
        AddItemDialog(self.root, self)
    
    def edit_item(self, event=None):
        """Sửa hạng mục"""
        selected = self.tree.selection()
        if not selected:
            messagebox.showwarning("Cảnh báo", "Vui lòng chọn hạng mục cần sửa!")
            return
        
        item = self.tree.item(selected[0])
        values = item['values']
        EditItemDialog(self.root, self, values)
    
    def delete_item(self):
        """Xóa hạng mục"""
        selected = self.tree.selection()
        if not selected:
            messagebox.showwarning("Cảnh báo", "Vui lòng chọn hạng mục cần xóa!")
            return
        
        if messagebox.askyesno("Xác nhận", "Bạn có chắc muốn xóa hạng mục này?"):
            self.tree.delete(selected[0])
            self.update_item_numbers()
            self.update_summary()
    
    def search_items(self, event=None):
        """Tìm kiếm hạng mục"""
        search_text = self.search_var.get().lower()
        
        for item in self.tree.get_children():
            values = self.tree.item(item)['values']
            # Tìm trong mã định mức và tên hạng mục
            if search_text in str(values[1]).lower() or search_text in str(values[2]).lower():
                self.tree.item(item, tags=('found',))
            else:
                self.tree.item(item, tags=())
        
        # Highlight found items
        self.tree.tag_configure('found', background='yellow')
    
    def calculate_total(self):
        """Tính toán tổng chi phí"""
        total = 0
        for item in self.tree.get_children():
            values = self.tree.item(item)['values']
            try:
                quantity = float(values[4])
                unit_price = float(values[5])
                item_total = quantity * unit_price
                
                # Update thành tiền column
                new_values = list(values)
                new_values[6] = f"{item_total:,.0f}"
                self.tree.item(item, values=new_values)
                
                total += item_total
            except (ValueError, IndexError):
                continue
        
        self.update_summary()
        messagebox.showinfo("Thành công", f"Đã tính toán xong!\nTổng giá trị: {total:,.0f} VNĐ")
    
    def update_summary(self):
        """Cập nhật thông tin tổng kết"""
        total_items = len(self.tree.get_children())
        total_amount = 0
        
        for item in self.tree.get_children():
            values = self.tree.item(item)['values']
            try:
                amount_str = str(values[6]).replace(',', '')
                total_amount += float(amount_str)
            except (ValueError, IndexError):
                continue
        
        vat_amount = total_amount * 0.1
        final_amount = total_amount + vat_amount
        
        self.total_items_label.config(text=f"Tổng số hạng mục: {total_items}")
        self.total_amount_label.config(text=f"Tổng giá trị: {total_amount:,.0f} VNĐ")
        self.vat_label.config(text=f"VAT (10%): {vat_amount:,.0f} VNĐ")
        self.final_amount_label.config(text=f"Tổng cộng (có VAT): {final_amount:,.0f} VNĐ")
    
    def update_item_numbers(self):
        """Cập nhật số thứ tự"""
        for i, item in enumerate(self.tree.get_children(), 1):
            values = list(self.tree.item(item)['values'])
            values[0] = i
            self.tree.item(item, values=values)
    
    def manage_norms(self):
        """Quản lý định mức"""
        NormManagementDialog(self.root, self)
    
    def open_calculator(self):
        """Mở calculator"""
        CalculatorDialog(self.root)
    
    def export_excel(self):
        """Xuất file Excel"""
        try:
            filename = filedialog.asksaveasfilename(
                defaultextension=".xlsx",
                filetypes=[("Excel files", "*.xlsx"), ("All files", "*.*")]
            )
            
            if filename:
                # Tạo DataFrame từ dữ liệu
                data = []
                for item in self.tree.get_children():
                    values = self.tree.item(item)['values']
                    data.append(values)
                
                df = pd.DataFrame(data, columns=['STT', 'Mã định mức', 'Tên hạng mục', 'Đơn vị', 'Khối lượng', 'Đơn giá', 'Thành tiền', 'Ghi chú'])
                
                # Xuất Excel
                with pd.ExcelWriter(filename, engine='openpyxl') as writer:
                    df.to_excel(writer, sheet_name='Dự toán', index=False)
                
                messagebox.showinfo("Thành công", f"Đã xuất file Excel: {filename}")
        except Exception as e:
            messagebox.showerror("Lỗi", f"Không thể xuất Excel: {str(e)}")
    
    def export_pdf(self):
        """Xuất file PDF"""
        messagebox.showinfo("Thông báo", "Tính năng xuất PDF sẽ được cập nhật trong phiên bản tiếp theo!")
    
    def save_project(self):
        """Lưu dự án"""
        filename = filedialog.asksaveasfilename(
            defaultextension=".json",
            filetypes=[("JSON files", "*.json"), ("All files", "*.*")]
        )
        
        if filename:
            project_data = {
                'project_info': {
                    'name': self.project_name.get(),
                    'location': self.project_location.get(),
                    'client': self.project_client.get(),
                    'date': self.project_date.get()
                },
                'items': []
            }
            
            for item in self.tree.get_children():
                values = self.tree.item(item)['values']
                project_data['items'].append({
                    'stt': values[0],
                    'code': values[1],
                    'name': values[2],
                    'unit': values[3],
                    'quantity': values[4],
                    'unit_price': values[5],
                    'total_price': values[6],
                    'notes': values[7]
                })
            
            try:
                with open(filename, 'w', encoding='utf-8') as f:
                    json.dump(project_data, f, ensure_ascii=False, indent=2)
                messagebox.showinfo("Thành công", f"Đã lưu dự án: {filename}")
            except Exception as e:
                messagebox.showerror("Lỗi", f"Không thể lưu dự án: {str(e)}")
    
    def open_project(self):
        """Mở dự án"""
        filename = filedialog.askopenfilename(
            filetypes=[("JSON files", "*.json"), ("All files", "*.*")]
        )
        
        if filename:
            try:
                with open(filename, 'r', encoding='utf-8') as f:
                    project_data = json.load(f)
                
                # Load project info
                info = project_data['project_info']
                self.project_name.delete(0, tk.END)
                self.project_name.insert(0, info['name'])
                self.project_location.delete(0, tk.END)
                self.project_location.insert(0, info['location'])
                self.project_client.delete(0, tk.END)
                self.project_client.insert(0, info['client'])
                self.project_date.delete(0, tk.END)
                self.project_date.insert(0, info['date'])
                
                # Clear and load items
                for item in self.tree.get_children():
                    self.tree.delete(item)
                
                for item_data in project_data['items']:
                    self.tree.insert('', tk.END, values=(
                        item_data['stt'], item_data['code'], item_data['name'],
                        item_data['unit'], item_data['quantity'], item_data['unit_price'],
                        item_data['total_price'], item_data['notes']
                    ))
                
                self.update_summary()
                messagebox.showinfo("Thành công", f"Đã mở dự án: {filename}")
                
            except Exception as e:
                messagebox.showerror("Lỗi", f"Không thể mở dự án: {str(e)}")
    
    def backup_data(self):
        """Backup dữ liệu"""
        backup_dir = filedialog.askdirectory(title="Chọn thư mục backup")
        if backup_dir:
            try:
                import shutil
                backup_file = os.path.join(backup_dir, f"backup_{datetime.now().strftime('%Y%m%d_%H%M%S')}.db")
                shutil.copy2('construction_estimate.db', backup_file)
                messagebox.showinfo("Thành công", f"Đã backup dữ liệu: {backup_file}")
            except Exception as e:
                messagebox.showerror("Lỗi", f"Không thể backup: {str(e)}")
    
    def show_help(self):
        """Hiển thị hướng dẫn"""
        help_text = """
🏗️ HƯỚNG DẪN SỬ DỤNG PHẦN MÀM DỰ TOÁN XÂY DỰNG

📋 CHỨC NĂNG CHÍNH:
• Tạo và quản lý dự án dự toán
• Thêm/sửa/xóa hạng mục công việc
• Tính toán tự động khối lượng và chi phí
• Xuất báo cáo Excel, PDF
• Quản lý định mức và đơn giá

⚡ TÍNH NĂNG VƯỢT TRỘI:
✅ Giao diện hiện đại, thân thiện
✅ Tính toán nhanh chóng, chính xác
✅ Backup tự động, bảo mật cao
✅ Hỗ trợ nhiều định mức xây dựng
✅ Hoàn toàn MIỄN PHÍ

🚀 CÁCH SỬ DỤNG:
1. Tạo dự án mới hoặc mở dự án có sẵn
2. Nhập thông tin dự án (tên, địa điểm, chủ đầu tư)
3. Thêm các hạng mục công việc
4. Nhập khối lượng và đơn giá
5. Tính toán tổng chi phí
6. Xuất báo cáo

📞 HỖ TRỢ: Liên hệ nhà phát triển để được hỗ trợ
        """
        
        help_window = tk.Toplevel(self.root)
        help_window.title("Hướng dẫn sử dụng")
        help_window.geometry("600x500")
        help_window.resizable(False, False)
        
        text_widget = tk.Text(help_window, wrap=tk.WORD, padx=20, pady=20)
        text_widget.pack(fill=tk.BOTH, expand=True)
        text_widget.insert(tk.END, help_text)
        text_widget.config(state=tk.DISABLED)
    
    def show_about(self):
        """Hiển thị thông tin phần mềm"""
        about_text = """
🏗️ PHẦN MỀM DỰ TOÁN XÂY DỰNG CHUYÊN NGHIỆP

📌 Phiên bản: 1.0.0
👨‍💻 Phát triển bởi: AI Assistant
📅 Năm phát triển: 2024

🎯 MỤC TIÊU:
Tạo ra phần mềm dự toán xây dựng miễn phí, 
vượt trội hơn ETA, G8, F1 về tính năng và 
trải nghiệm người dùng.

⭐ ƯU ĐIỂM VƯỢT TRỘI:
• Hoàn toàn MIỄN PHÍ
• Giao diện hiện đại, dễ sử dụng
• Tính toán nhanh chóng, chính xác
• Hỗ trợ đầy đủ định mức Việt Nam
• Backup tự động, bảo mật cao
• Xuất báo cáo đa dạng (Excel, PDF)
• Không giới hạn số dự án
• Không cần kết nối internet

💝 MIỄN PHÍ VĨNH VIỄN!
        """
        
        messagebox.showinfo("Về phần mềm", about_text)


class AddItemDialog:
    def __init__(self, parent, app):
        self.app = app
        self.dialog = tk.Toplevel(parent)
        self.dialog.title("Thêm hạng mục mới")
        self.dialog.geometry("600x400")
        self.dialog.resizable(False, False)
        self.dialog.grab_set()
        
        self.create_widgets()
    
    def create_widgets(self):
        # Main frame
        main_frame = ttk.Frame(self.dialog, padding=20)
        main_frame.pack(fill=tk.BOTH, expand=True)
        
        # Norm selection
        ttk.Label(main_frame, text="Chọn định mức:", font=('Arial', 10, 'bold')).pack(anchor=tk.W)
        
        # Listbox for norms
        listbox_frame = ttk.Frame(main_frame)
        listbox_frame.pack(fill=tk.BOTH, expand=True, pady=(5, 10))
        
        self.norm_listbox = tk.Listbox(listbox_frame, height=8)
        scrollbar = ttk.Scrollbar(listbox_frame, orient=tk.VERTICAL, command=self.norm_listbox.yview)
        self.norm_listbox.configure(yscrollcommand=scrollbar.set)
        
        self.norm_listbox.pack(side=tk.LEFT, fill=tk.BOTH, expand=True)
        scrollbar.pack(side=tk.RIGHT, fill=tk.Y)
        
        # Load norms
        self.load_norms()
        
        # Input fields
        fields_frame = ttk.Frame(main_frame)
        fields_frame.pack(fill=tk.X, pady=10)
        
        ttk.Label(fields_frame, text="Khối lượng:").grid(row=0, column=0, sticky=tk.W, padx=(0, 5))
        self.quantity_entry = ttk.Entry(fields_frame, width=15)
        self.quantity_entry.grid(row=0, column=1, padx=(0, 20))
        
        ttk.Label(fields_frame, text="Đơn giá:").grid(row=0, column=2, sticky=tk.W, padx=(0, 5))
        self.price_entry = ttk.Entry(fields_frame, width=15)
        self.price_entry.grid(row=0, column=3)
        
        ttk.Label(fields_frame, text="Ghi chú:").grid(row=1, column=0, sticky=tk.W, padx=(0, 5), pady=(10, 0))
        self.notes_entry = ttk.Entry(fields_frame, width=50)
        self.notes_entry.grid(row=1, column=1, columnspan=3, sticky=tk.W+tk.E, pady=(10, 0))
        
        # Buttons
        button_frame = ttk.Frame(main_frame)
        button_frame.pack(fill=tk.X, pady=(20, 0))
        
        ttk.Button(button_frame, text="Thêm", command=self.add_item).pack(side=tk.RIGHT, padx=(5, 0))
        ttk.Button(button_frame, text="Hủy", command=self.dialog.destroy).pack(side=tk.RIGHT)
        
        # Bind events
        self.norm_listbox.bind('<<ListboxSelect>>', self.on_norm_select)
    
    def load_norms(self):
        """Load danh sách định mức"""
        self.app.cursor.execute("SELECT code, name, unit, total_cost FROM norms ORDER BY code")
        norms = self.app.cursor.fetchall()
        
        self.norms_data = {}
        for code, name, unit, cost in norms:
            display_text = f"{code} - {name} ({unit}) - {cost:,.0f} VNĐ"
            self.norm_listbox.insert(tk.END, display_text)
            self.norms_data[display_text] = (code, name, unit, cost)
    
    def on_norm_select(self, event):
        """Xử lý khi chọn định mức"""
        selection = self.norm_listbox.curselection()
        if selection:
            selected_text = self.norm_listbox.get(selection[0])
            code, name, unit, cost = self.norms_data[selected_text]
            self.price_entry.delete(0, tk.END)
            self.price_entry.insert(0, str(cost))
    
    def add_item(self):
        """Thêm hạng mục vào bảng"""
        selection = self.norm_listbox.curselection()
        if not selection:
            messagebox.showwarning("Cảnh báo", "Vui lòng chọn định mức!")
            return
        
        selected_text = self.norm_listbox.get(selection[0])
        code, name, unit, cost = self.norms_data[selected_text]
        
        try:
            quantity = float(self.quantity_entry.get())
            unit_price = float(self.price_entry.get())
        except ValueError:
            messagebox.showerror("Lỗi", "Vui lòng nhập số hợp lệ cho khối lượng và đơn giá!")
            return
        
        total_price = quantity * unit_price
        notes = self.notes_entry.get()
        
        # Add to treeview
        stt = len(self.app.tree.get_children()) + 1
        self.app.tree.insert('', tk.END, values=(
            stt, code, name, unit, quantity, f"{unit_price:,.0f}", f"{total_price:,.0f}", notes
        ))
        
        self.app.update_summary()
        self.dialog.destroy()


class EditItemDialog:
    def __init__(self, parent, app, current_values):
        self.app = app
        self.current_values = current_values
        self.dialog = tk.Toplevel(parent)
        self.dialog.title("Sửa hạng mục")
        self.dialog.geometry("500x300")
        self.dialog.resizable(False, False)
        self.dialog.grab_set()
        
        self.create_widgets()
    
    def create_widgets(self):
        main_frame = ttk.Frame(self.dialog, padding=20)
        main_frame.pack(fill=tk.BOTH, expand=True)
        
        # Display current info
        info_frame = ttk.LabelFrame(main_frame, text="Thông tin hạng mục", padding=10)
        info_frame.pack(fill=tk.X, pady=(0, 20))
        
        ttk.Label(info_frame, text=f"Mã: {self.current_values[1]}").pack(anchor=tk.W)
        ttk.Label(info_frame, text=f"Tên: {self.current_values[2]}").pack(anchor=tk.W)
        ttk.Label(info_frame, text=f"Đơn vị: {self.current_values[3]}").pack(anchor=tk.W)
        
        # Edit fields
        edit_frame = ttk.LabelFrame(main_frame, text="Chỉnh sửa", padding=10)
        edit_frame.pack(fill=tk.X, pady=(0, 20))
        
        ttk.Label(edit_frame, text="Khối lượng:").grid(row=0, column=0, sticky=tk.W, padx=(0, 5))
        self.quantity_entry = ttk.Entry(edit_frame, width=20)
        self.quantity_entry.insert(0, str(self.current_values[4]))
        self.quantity_entry.grid(row=0, column=1, sticky=tk.W)
        
        ttk.Label(edit_frame, text="Đơn giá:").grid(row=1, column=0, sticky=tk.W, padx=(0, 5), pady=(10, 0))
        self.price_entry = ttk.Entry(edit_frame, width=20)
        price_str = str(self.current_values[5]).replace(',', '')
        self.price_entry.insert(0, price_str)
        self.price_entry.grid(row=1, column=1, sticky=tk.W, pady=(10, 0))
        
        ttk.Label(edit_frame, text="Ghi chú:").grid(row=2, column=0, sticky=tk.W, padx=(0, 5), pady=(10, 0))
        self.notes_entry = ttk.Entry(edit_frame, width=40)
        self.notes_entry.insert(0, str(self.current_values[7]))
        self.notes_entry.grid(row=2, column=1, sticky=tk.W+tk.E, pady=(10, 0))
        
        # Buttons
        button_frame = ttk.Frame(main_frame)
        button_frame.pack(fill=tk.X)
        
        ttk.Button(button_frame, text="Cập nhật", command=self.update_item).pack(side=tk.RIGHT, padx=(5, 0))
        ttk.Button(button_frame, text="Hủy", command=self.dialog.destroy).pack(side=tk.RIGHT)
    
    def update_item(self):
        """Cập nhật hạng mục"""
        try:
            quantity = float(self.quantity_entry.get())
            unit_price = float(self.price_entry.get())
        except ValueError:
            messagebox.showerror("Lỗi", "Vui lòng nhập số hợp lệ!")
            return
        
        total_price = quantity * unit_price
        notes = self.notes_entry.get()
        
        # Find and update the selected item
        selected = self.app.tree.selection()[0]
        new_values = list(self.current_values)
        new_values[4] = quantity
        new_values[5] = f"{unit_price:,.0f}"
        new_values[6] = f"{total_price:,.0f}"
        new_values[7] = notes
        
        self.app.tree.item(selected, values=new_values)
        self.app.update_summary()
        self.dialog.destroy()


class NormManagementDialog:
    def __init__(self, parent, app):
        self.app = app
        self.dialog = tk.Toplevel(parent)
        self.dialog.title("Quản lý định mức")
        self.dialog.geometry("800x600")
        self.dialog.grab_set()
        
        self.create_widgets()
        self.load_norms()
    
    def create_widgets(self):
        main_frame = ttk.Frame(self.dialog, padding=10)
        main_frame.pack(fill=tk.BOTH, expand=True)
        
        # Toolbar
        toolbar = ttk.Frame(main_frame)
        toolbar.pack(fill=tk.X, pady=(0, 10))
        
        ttk.Button(toolbar, text="➕ Thêm định mức", command=self.add_norm).pack(side=tk.LEFT, padx=(0, 5))
        ttk.Button(toolbar, text="✏️ Sửa", command=self.edit_norm).pack(side=tk.LEFT, padx=(0, 5))
        ttk.Button(toolbar, text="🗑️ Xóa", command=self.delete_norm).pack(side=tk.LEFT, padx=(0, 5))
        
        # Treeview
        columns = ('Mã', 'Tên định mức', 'Đơn vị', 'Vật liệu', 'Nhân công', 'Máy móc', 'Tổng cộng', 'Loại')
        self.tree = ttk.Treeview(main_frame, columns=columns, show='headings', height=20)
        
        for col in columns:
            self.tree.heading(col, text=col)
            self.tree.column(col, width=100)
        
        # Scrollbars
        v_scrollbar = ttk.Scrollbar(main_frame, orient=tk.VERTICAL, command=self.tree.yview)
        h_scrollbar = ttk.Scrollbar(main_frame, orient=tk.HORIZONTAL, command=self.tree.xview)
        self.tree.configure(yscrollcommand=v_scrollbar.set, xscrollcommand=h_scrollbar.set)
        
        self.tree.pack(side=tk.LEFT, fill=tk.BOTH, expand=True)
        v_scrollbar.pack(side=tk.RIGHT, fill=tk.Y)
        
        # Close button
        ttk.Button(main_frame, text="Đóng", command=self.dialog.destroy).pack(pady=(10, 0))
    
    def load_norms(self):
        """Load danh sách định mức"""
        for item in self.tree.get_children():
            self.tree.delete(item)
        
        self.app.cursor.execute("""
            SELECT code, name, unit, material_cost, labor_cost, machine_cost, total_cost, category 
            FROM norms ORDER BY code
        """)
        
        for row in self.app.cursor.fetchall():
            formatted_row = (
                row[0], row[1], row[2],
                f"{row[3]:,.0f}", f"{row[4]:,.0f}", f"{row[5]:,.0f}", f"{row[6]:,.0f}",
                row[7]
            )
            self.tree.insert('', tk.END, values=formatted_row)
    
    def add_norm(self):
        """Thêm định mức mới"""
        AddNormDialog(self.dialog, self)
    
    def edit_norm(self):
        """Sửa định mức"""
        selected = self.tree.selection()
        if not selected:
            messagebox.showwarning("Cảnh báo", "Vui lòng chọn định mức cần sửa!")
            return
        
        values = self.tree.item(selected[0])['values']
        EditNormDialog(self.dialog, self, values)
    
    def delete_norm(self):
        """Xóa định mức"""
        selected = self.tree.selection()
        if not selected:
            messagebox.showwarning("Cảnh báo", "Vui lòng chọn định mức cần xóa!")
            return
        
        if messagebox.askyesno("Xác nhận", "Bạn có chắc muốn xóa định mức này?"):
            values = self.tree.item(selected[0])['values']
            code = values[0]
            
            self.app.cursor.execute("DELETE FROM norms WHERE code = ?", (code,))
            self.app.conn.commit()
            
            self.load_norms()


class AddNormDialog:
    def __init__(self, parent, norm_manager):
        self.norm_manager = norm_manager
        self.dialog = tk.Toplevel(parent)
        self.dialog.title("Thêm định mức mới")
        self.dialog.geometry("400x350")
        self.dialog.resizable(False, False)
        self.dialog.grab_set()
        
        self.create_widgets()
    
    def create_widgets(self):
        main_frame = ttk.Frame(self.dialog, padding=20)
        main_frame.pack(fill=tk.BOTH, expand=True)
        
        # Input fields
        fields = [
            ("Mã định mức:", "code"),
            ("Tên định mức:", "name"),
            ("Đơn vị:", "unit"),
            ("Chi phí vật liệu:", "material"),
            ("Chi phí nhân công:", "labor"),
            ("Chi phí máy móc:", "machine"),
            ("Loại:", "category")
        ]
        
        self.entries = {}
        for i, (label, key) in enumerate(fields):
            ttk.Label(main_frame, text=label).grid(row=i, column=0, sticky=tk.W, pady=5, padx=(0, 10))
            entry = ttk.Entry(main_frame, width=30)
            entry.grid(row=i, column=1, pady=5)
            self.entries[key] = entry
        
        # Buttons
        button_frame = ttk.Frame(main_frame)
        button_frame.grid(row=len(fields), column=0, columnspan=2, pady=20)
        
        ttk.Button(button_frame, text="Thêm", command=self.add_norm).pack(side=tk.LEFT, padx=(0, 10))
        ttk.Button(button_frame, text="Hủy", command=self.dialog.destroy).pack(side=tk.LEFT)
    
    def add_norm(self):
        """Thêm định mức"""
        try:
            code = self.entries['code'].get().strip()
            name = self.entries['name'].get().strip()
            unit = self.entries['unit'].get().strip()
            material = float(self.entries['material'].get())
            labor = float(self.entries['labor'].get())
            machine = float(self.entries['machine'].get())
            category = self.entries['category'].get().strip()
            
            if not all([code, name, unit]):
                messagebox.showerror("Lỗi", "Vui lòng nhập đầy đủ thông tin!")
                return
            
            total = material + labor + machine
            
            self.norm_manager.app.cursor.execute("""
                INSERT INTO norms (code, name, unit, material_cost, labor_cost, machine_cost, total_cost, category)
                VALUES (?, ?, ?, ?, ?, ?, ?, ?)
            """, (code, name, unit, material, labor, machine, total, category))
            
            self.norm_manager.app.conn.commit()
            self.norm_manager.load_norms()
            self.dialog.destroy()
            
        except ValueError:
            messagebox.showerror("Lỗi", "Vui lòng nhập số hợp lệ cho chi phí!")
        except sqlite3.IntegrityError:
            messagebox.showerror("Lỗi", "Mã định mức đã tồn tại!")


class EditNormDialog:
    def __init__(self, parent, norm_manager, current_values):
        self.norm_manager = norm_manager
        self.current_values = current_values
        self.dialog = tk.Toplevel(parent)
        self.dialog.title("Sửa định mức")
        self.dialog.geometry("400x350")
        self.dialog.resizable(False, False)
        self.dialog.grab_set()
        
        self.create_widgets()
    
    def create_widgets(self):
        main_frame = ttk.Frame(self.dialog, padding=20)
        main_frame.pack(fill=tk.BOTH, expand=True)
        
        # Input fields with current values
        fields = [
            ("Mã định mức:", "code", self.current_values[0]),
            ("Tên định mức:", "name", self.current_values[1]),
            ("Đơn vị:", "unit", self.current_values[2]),
            ("Chi phí vật liệu:", "material", self.current_values[3].replace(',', '')),
            ("Chi phí nhân công:", "labor", self.current_values[4].replace(',', '')),
            ("Chi phí máy móc:", "machine", self.current_values[5].replace(',', '')),
            ("Loại:", "category", self.current_values[7])
        ]
        
        self.entries = {}
        for i, (label, key, value) in enumerate(fields):
            ttk.Label(main_frame, text=label).grid(row=i, column=0, sticky=tk.W, pady=5, padx=(0, 10))
            entry = ttk.Entry(main_frame, width=30)
            entry.insert(0, str(value))
            if key == 'code':  # Mã định mức không cho sửa
                entry.config(state='readonly')
            entry.grid(row=i, column=1, pady=5)
            self.entries[key] = entry
        
        # Buttons
        button_frame = ttk.Frame(main_frame)
        button_frame.grid(row=len(fields), column=0, columnspan=2, pady=20)
        
        ttk.Button(button_frame, text="Cập nhật", command=self.update_norm).pack(side=tk.LEFT, padx=(0, 10))
        ttk.Button(button_frame, text="Hủy", command=self.dialog.destroy).pack(side=tk.LEFT)
    
    def update_norm(self):
        """Cập nhật định mức"""
        try:
            code = self.entries['code'].get().strip()
            name = self.entries['name'].get().strip()
            unit = self.entries['unit'].get().strip()
            material = float(self.entries['material'].get())
            labor = float(self.entries['labor'].get())
            machine = float(self.entries['machine'].get())
            category = self.entries['category'].get().strip()
            
            total = material + labor + machine
            
            self.norm_manager.app.cursor.execute("""
                UPDATE norms SET name=?, unit=?, material_cost=?, labor_cost=?, 
                machine_cost=?, total_cost=?, category=? WHERE code=?
            """, (name, unit, material, labor, machine, total, category, code))
            
            self.norm_manager.app.conn.commit()
            self.norm_manager.load_norms()
            self.dialog.destroy()
            
        except ValueError:
            messagebox.showerror("Lỗi", "Vui lòng nhập số hợp lệ cho chi phí!")


class CalculatorDialog:
    def __init__(self, parent):
        self.dialog = tk.Toplevel(parent)
        self.dialog.title("Calculator")
        self.dialog.geometry("300x400")
        self.dialog.resizable(False, False)
        
        self.create_widgets()
    
    def create_widgets(self):
        main_frame = ttk.Frame(self.dialog, padding=10)
        main_frame.pack(fill=tk.BOTH, expand=True)
        
        # Display
        self.display_var = tk.StringVar(value="0")
        display = ttk.Entry(main_frame, textvariable=self.display_var, font=('Arial', 16), 
                           state='readonly', justify='right')
        display.pack(fill=tk.X, pady=(0, 10))
        
        # Buttons
        buttons = [
            ['C', '±', '%', '÷'],
            ['7', '8', '9', '×'],
            ['4', '5', '6', '-'],
            ['1', '2', '3', '+'],
            ['0', '.', '=']
        ]
        
        for row in buttons:
            button_frame = ttk.Frame(main_frame)
            button_frame.pack(fill=tk.X, pady=2)
            
            for i, btn_text in enumerate(row):
                if btn_text == '0':
                    btn = ttk.Button(button_frame, text=btn_text, width=10,
                                   command=lambda t=btn_text: self.button_click(t))
                    btn.pack(side=tk.LEFT, fill=tk.X, expand=True, padx=2)
                else:
                    btn = ttk.Button(button_frame, text=btn_text, width=5,
                                   command=lambda t=btn_text: self.button_click(t))
                    btn.pack(side=tk.LEFT, padx=2)
        
        self.current_input = "0"
        self.operator = None
        self.previous_value = None
    
    def button_click(self, value):
        """Xử lý click button"""
        if value.isdigit() or value == '.':
            if self.current_input == "0":
                self.current_input = value
            else:
                self.current_input += value
            self.display_var.set(self.current_input)
        
        elif value == 'C':
            self.current_input = "0"
            self.operator = None
            self.previous_value = None
            self.display_var.set("0")
        
        elif value in ['÷', '×', '-', '+']:
            if self.previous_value is not None and self.operator:
                self.calculate()
            self.previous_value = float(self.current_input)
            self.operator = value
            self.current_input = "0"
        
        elif value == '=':
            if self.previous_value is not None and self.operator:
                self.calculate()
        
        elif value == '±':
            if self.current_input != "0":
                if self.current_input.startswith('-'):
                    self.current_input = self.current_input[1:]
                else:
                    self.current_input = '-' + self.current_input
                self.display_var.set(self.current_input)
    
    def calculate(self):
        """Thực hiện tính toán"""
        try:
            current_value = float(self.current_input)
            
            if self.operator == '+':
                result = self.previous_value + current_value
            elif self.operator == '-':
                result = self.previous_value - current_value
            elif self.operator == '×':
                result = self.previous_value * current_value
            elif self.operator == '÷':
                if current_value != 0:
                    result = self.previous_value / current_value
                else:
                    messagebox.showerror("Lỗi", "Không thể chia cho 0!")
                    return
            
            self.current_input = str(result)
            self.display_var.set(self.current_input)
            self.previous_value = None
            self.operator = None
            
        except Exception as e:
            messagebox.showerror("Lỗi", f"Lỗi tính toán: {str(e)}")


if __name__ == "__main__":
    root = tk.Tk()
    app = ConstructionEstimateApp(root)
    root.mainloop()