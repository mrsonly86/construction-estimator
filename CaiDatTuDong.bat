@echo off
echo ========================================
echo   CAI DAT TU DONG - DU TOAN XAY DUNG
echo ========================================
echo.

echo Dang kiem tra Python...
python --version >nul 2>&1
if errorlevel 1 (
    echo ❌ Chua cai dat Python!
    echo.
    echo 📋 HUONG DAN CAI DAT PYTHON:
    echo 1. Vao trang: https://python.org
    echo 2. Nhan "Downloads" 
    echo 3. Tai "Python 3.11.x"
    echo 4. Chay file tai ve
    echo 5. ✅ QUAN TRONG: Tich vao "Add Python to PATH"
    echo 6. Nhan "Install Now"
    echo 7. Chay lai file nay sau khi cai xong
    echo.
    pause
    exit /b 1
)

echo ✅ Python da san sang!
python --version
echo.

echo Dang cai dat cac thu vien can thiet...
echo Vui long cho 5-10 phut...
echo.

echo [1/8] Cai dat pandas...
pip install pandas --quiet

echo [2/8] Cai dat openpyxl...  
pip install openpyxl --quiet

echo [3/8] Cai dat reportlab...
pip install reportlab --quiet

echo [4/8] Cai dat Pillow...
pip install Pillow --quiet

echo [5/8] Cai dat matplotlib...
pip install matplotlib --quiet

echo [6/8] Cai dat numpy...
pip install numpy --quiet

echo [7/8] Cai dat scikit-learn...
pip install scikit-learn --quiet

echo [8/8] Cai dat requests...
pip install requests --quiet

echo.
echo ✅ Hoan thanh cai dat tat ca thu vien!
echo.

echo Dang tao file du lieu mau...
python -c "
import sqlite3
conn = sqlite3.connect('construction_estimate.db')
cursor = conn.cursor()
cursor.execute('CREATE TABLE IF NOT EXISTS norms (id INTEGER PRIMARY KEY, code TEXT, name TEXT, unit TEXT, total_cost REAL, category TEXT)')
cursor.execute(\"INSERT OR IGNORE INTO norms VALUES (1, 'A.1.1.1', 'Đào đất thủ công', 'm³', 150000, 'Đào đất')\")
cursor.execute(\"INSERT OR IGNORE INTO norms VALUES (2, 'A.2.1.1', 'Bê tông C20', 'm³', 1850000, 'Bê tông')\")
cursor.execute(\"INSERT OR IGNORE INTO norms VALUES (3, 'A.3.1.1', 'Thép phi 10', 'kg', 22000, 'Thép')\")
conn.commit()
conn.close()
print('✅ Đã tạo dữ liệu mẫu!')
"

echo.
echo 🎉 CAI DAT HOAN THANH!
echo.
echo 📋 CACH SU DUNG:
echo 1. Double-click file "ChayPhanMem.bat" de chay phan mem
echo 2. Hoac go lenh: python main.py
echo 3. Doc file "HUONG_DAN_CHI_TIET_CHO_NGUOI_MOI.md" de biet them chi tiet
echo.
echo 💡 LUU Y:
echo - Giu nguyen tat ca file trong thu muc nay
echo - Neu gap loi, xem phan "Khac phuc su co" trong huong dan
echo - Lien he ho tro: support@dutoanxaydung.com
echo.

pause