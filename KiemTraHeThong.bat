@echo off
echo ========================================
echo   KIEM TRA HE THONG - DU TOAN XAY DUNG
echo ========================================
echo.

echo 🔍 KIEM TRA CAU HINH MAY TINH:
echo.

echo 💻 He dieu hanh:
ver
echo.

echo 🧠 Bo nho RAM:
wmic computersystem get TotalPhysicalMemory /format:value | findstr "TotalPhysicalMemory" >temp.txt
set /p RAM_BYTES=<temp.txt
set RAM_BYTES=%RAM_BYTES:TotalPhysicalMemory=%
set /a RAM_GB=%RAM_BYTES:~0,-9%
echo RAM: %RAM_GB% GB
del temp.txt
echo.

if %RAM_GB% LSS 4 (
    echo ⚠️ CANH BAO: RAM duoi 4GB co the chay cham
) else (
    echo ✅ RAM du dieu kien
)
echo.

echo 🐍 KIEM TRA PYTHON:
python --version >nul 2>&1
if errorlevel 1 (
    echo ❌ Python chua duoc cai dat
    echo 📥 Hay cai dat Python tu: https://python.org
) else (
    echo ✅ Python da cai dat:
    python --version
)
echo.

echo 📦 KIEM TRA THU VIEN:
echo.

set "libraries=pandas openpyxl reportlab matplotlib numpy scikit-learn"

for %%i in (%libraries%) do (
    python -c "import %%i" >nul 2>&1
    if errorlevel 1 (
        echo ❌ %%i: Chua cai dat
    ) else (
        echo ✅ %%i: Da cai dat
    )
)

echo.
echo 📁 KIEM TRA FILE PHAN MEM:
echo.

if exist "main.py" (
    echo ✅ main.py: Co san
) else (
    echo ❌ main.py: Khong tim thay
)

if exist "requirements.txt" (
    echo ✅ requirements.txt: Co san
) else (
    echo ❌ requirements.txt: Khong tim thay
)

if exist "construction_estimate.db" (
    echo ✅ Database: Da khoi tao
) else (
    echo ⚠️ Database: Chua khoi tao (se tu tao khi chay lan dau)
)

echo.
echo 🎯 KET LUAN:
echo.

python --version >nul 2>&1
if errorlevel 1 (
    echo ❌ HE THONG CHUA SAN SANG
    echo 📋 CAN LAM:
    echo 1. Cai dat Python tu https://python.org
    echo 2. Chay file "CaiDatTuDong.bat"
) else (
    python -c "import pandas" >nul 2>&1
    if errorlevel 1 (
        echo ⚠️ HE THONG THIEU THU VIEN
        echo 📋 CAN LAM:
        echo 1. Chay file "CaiDatTuDong.bat"
        echo 2. Hoac go lenh: pip install pandas openpyxl reportlab matplotlib numpy
    ) else (
        if exist "main.py" (
            echo ✅ HE THONG SAN SANG!
            echo 🚀 Co the chay phan mem bang cach:
            echo 1. Double-click "ChayPhanMem.bat"
            echo 2. Hoac go lenh: python main.py
        ) else (
            echo ❌ THIEU FILE PHAN MEM
            echo 📋 CAN LAM:
            echo 1. Tai day du file tu GitHub
            echo 2. Hoac sao chep file main.py vao thu muc nay
        )
    )
)

echo.
echo 📞 HO TRO:
echo - Email: support@dutoanxaydung.com
echo - Doc file: HUONG_DAN_CHI_TIET_CHO_NGUOI_MOI.md
echo.

pause