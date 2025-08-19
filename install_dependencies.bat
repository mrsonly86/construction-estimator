@echo off
echo ========================================
echo   Cài đặt phần mềm Dự toán Xây dựng
echo ========================================
echo.

echo Đang kiểm tra Python...
python --version >nul 2>&1
if errorlevel 1 (
    echo ❌ Không tìm thấy Python!
    echo 📥 Vui lòng tải và cài đặt Python từ: https://python.org
    pause
    exit /b 1
)

echo ✅ Đã tìm thấy Python

echo.
echo Đang cài đặt các thư viện cần thiết...
echo.

pip install --upgrade pip
pip install tkinter
pip install pandas>=1.5.0
pip install openpyxl>=3.1.0
pip install reportlab>=4.0.0
pip install Pillow>=10.0.0
pip install matplotlib>=3.7.0
pip install numpy>=1.24.0
pip install PyInstaller>=5.13.0

echo.
echo ✅ Đã cài đặt xong tất cả thư viện!
echo.

echo Đang tạo file .exe...
python setup_pyinstaller.py

echo.
echo ========================================
echo   Hoàn thành cài đặt!
echo ========================================
echo.
echo 🚀 Chạy file 'DuToanXayDung.exe' trong thư mục 'dist' để sử dụng
echo 📚 Đọc file 'HUONG_DAN_SU_DUNG.txt' để biết cách sử dụng
echo.
pause