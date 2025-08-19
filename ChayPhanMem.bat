@echo off
echo ========================================
echo   PHAN MEM DU TOAN XAY DUNG v2.0
echo   Dang khoi dong...
echo ========================================
echo.

cd /d "%~dp0"

echo Kiem tra Python...
python --version >nul 2>&1
if errorlevel 1 (
    echo ❌ Khong tim thay Python!
    echo 📥 Vui long cai dat Python tu: https://python.org
    echo ✅ Nho tich "Add Python to PATH" khi cai dat
    pause
    exit /b 1
)

echo ✅ Da tim thay Python
echo.

echo Dang khoi chay phan mem...
echo Vui long cho...
echo.

python main.py

if errorlevel 1 (
    echo.
    echo ❌ Co loi khi chay phan mem!
    echo 💡 Hay thu cac buoc sau:
    echo    1. Cai dat thu vien: pip install pandas openpyxl reportlab
    echo    2. Kiem tra file main.py co ton tai khong
    echo    3. Lien he ho tro: support@dutoanxaydung.com
    echo.
)

pause