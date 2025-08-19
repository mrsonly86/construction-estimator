#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
Setup script để tạo file .exe cho Windows
Chạy: python setup.py build
"""

import sys
from cx_Freeze import setup, Executable
import os

# Thêm thư viện cần thiết
build_exe_options = {
    "packages": [
        "tkinter", "sqlite3", "pandas", "openpyxl", "reportlab", 
        "PIL", "matplotlib", "numpy", "datetime", "json", "os", "pathlib"
    ],
    "excludes": ["unittest", "test", "tkinter.test"],
    "include_files": [
        # Thêm các file cần thiết khác nếu có
    ],
    "zip_include_packages": ["*"],
    "zip_exclude_packages": []
}

# Base cho Windows GUI
base = None
if sys.platform == "win32":
    base = "Win32GUI"

# Icon file (nếu có)
icon_path = "icon.ico" if os.path.exists("icon.ico") else None

setup(
    name="Phần mềm Dự toán Xây dựng",
    version="1.0.0",
    description="Phần mềm dự toán xây dựng chuyên nghiệp - Miễn phí",
    author="AI Assistant",
    options={"build_exe": build_exe_options},
    executables=[
        Executable(
            "main.py",
            base=base,
            target_name="DuToanXayDung.exe",
            icon=icon_path,
            shortcut_name="Dự toán Xây dựng",
            shortcut_dir="DesktopFolder"
        )
    ]
)