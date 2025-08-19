#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
Script tạo file .exe bằng PyInstaller
Chạy: python setup_pyinstaller.py
"""

import os
import subprocess
import sys

def create_exe():
    """Tạo file .exe bằng PyInstaller"""
    
    # Kiểm tra PyInstaller đã cài chưa
    try:
        import PyInstaller
    except ImportError:
        print("Đang cài đặt PyInstaller...")
        subprocess.check_call([sys.executable, "-m", "pip", "install", "PyInstaller"])
    
    # Tạo spec file với cấu hình tùy chỉnh
    spec_content = '''
# -*- mode: python ; coding: utf-8 -*-

block_cipher = None

a = Analysis(
    ['main.py'],
    pathex=[],
    binaries=[],
    datas=[],
    hiddenimports=[
        'tkinter', 'tkinter.ttk', 'tkinter.messagebox', 'tkinter.filedialog',
        'sqlite3', 'pandas', 'openpyxl', 'reportlab.lib', 'reportlab.platypus',
        'reportlab.lib.pagesizes', 'reportlab.lib.colors', 'reportlab.lib.styles',
        'PIL', 'matplotlib', 'numpy'
    ],
    hookspath=[],
    hooksconfig={},
    runtime_hooks=[],
    excludes=['unittest', 'test'],
    win_no_prefer_redirects=False,
    win_private_assemblies=False,
    cipher=block_cipher,
    noarchive=False,
)

pyz = PYZ(a.pure, a.zipped_data, cipher=block_cipher)

exe = EXE(
    pyz,
    a.scripts,
    a.binaries,
    a.zipfiles,
    a.datas,
    [],
    name='DuToanXayDung',
    debug=False,
    bootloader_ignore_signals=False,
    strip=False,
    upx=True,
    upx_exclude=[],
    runtime_tmpdir=None,
    console=False,
    disable_windowed_traceback=False,
    argv_emulation=False,
    target_arch=None,
    codesign_identity=None,
    entitlements_file=None,
    version='version_info.txt',
    icon='icon.ico'
)
'''
    
    # Ghi spec file
    with open('DuToanXayDung.spec', 'w', encoding='utf-8') as f:
        f.write(spec_content)
    
    # Tạo version info
    version_info = '''
VSVersionInfo(
  ffi=FixedFileInfo(
    filevers=(1, 0, 0, 0),
    prodvers=(1, 0, 0, 0),
    mask=0x3f,
    flags=0x0,
    OS=0x4,
    fileType=0x1,
    subtype=0x0,
    date=(0, 0)
  ),
  kids=[
    StringFileInfo(
      [
      StringTable(
        '040904B0',
        [StringStruct('CompanyName', 'AI Assistant'),
        StringStruct('FileDescription', 'Phần mềm Dự toán Xây dựng Chuyên nghiệp'),
        StringStruct('FileVersion', '1.0.0.0'),
        StringStruct('InternalName', 'DuToanXayDung'),
        StringStruct('LegalCopyright', 'Copyright © 2024 - Miễn phí'),
        StringStruct('OriginalFilename', 'DuToanXayDung.exe'),
        StringStruct('ProductName', 'Dự toán Xây dựng'),
        StringStruct('ProductVersion', '1.0.0.0')])
      ]), 
    VarFileInfo([VarStruct('Translation', [1033, 1200])])
  ]
)
'''
    
    with open('version_info.txt', 'w', encoding='utf-8') as f:
        f.write(version_info)
    
    print("Đang tạo file .exe...")
    print("Quá trình này có thể mất vài phút...")
    
    try:
        # Chạy PyInstaller
        cmd = [
            'pyinstaller', 
            '--onefile',
            '--windowed',
            '--name=DuToanXayDung',
            '--distpath=dist',
            '--workpath=build',
            '--specpath=.',
            'DuToanXayDung.spec'
        ]
        
        result = subprocess.run(cmd, capture_output=True, text=True)
        
        if result.returncode == 0:
            print("✅ Tạo file .exe thành công!")
            print("📁 File .exe được lưu trong thư mục 'dist'")
            print("🚀 Bạn có thể chạy file 'DuToanXayDung.exe' để sử dụng phần mềm")
        else:
            print("❌ Có lỗi khi tạo file .exe:")
            print(result.stderr)
            
    except FileNotFoundError:
        print("❌ Không tìm thấy PyInstaller. Đang cài đặt...")
        subprocess.check_call([sys.executable, "-m", "pip", "install", "PyInstaller"])
        print("✅ Đã cài đặt PyInstaller. Vui lòng chạy lại script này.")

if __name__ == "__main__":
    create_exe()