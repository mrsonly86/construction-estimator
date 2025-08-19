# 🎯 HƯỚNG DẪN CHI TIẾT CHO NGƯỜI MỚI HOÀN TOÀN

> **Dành cho người không biết lập trình - Hướng dẫn từ A đến Z**

---

## 📋 MỤC LỤC
1. [Chuẩn bị máy tính](#1-chuẩn-bị-máy-tính)
2. [Cài đặt Python](#2-cài-đặt-python)
3. [Tải và cài đặt phần mềm](#3-tải-và-cài-đặt-phần-mềm)
4. [Chạy phần mềm lần đầu](#4-chạy-phần-mềm-lần-đầu)
5. [Hướng dẫn sử dụng cơ bản](#5-hướng-dẫn-sử-dụng-cơ-bản)
6. [Khắc phục sự cố thường gặp](#6-khắc-phục-sự-cố-thường-gặp)

---

## 1. CHUẨN BỊ MÁY TÍNH

### ✅ Yêu cầu tối thiểu:
- **Hệ điều hành:** Windows 7/8/10/11 (64-bit)
- **RAM:** 4GB trở lên (khuyến nghị 8GB)
- **Ổ cứng:** 2GB dung lượng trống
- **Kết nối internet:** Để tải phần mềm và cập nhật

### 🔍 Kiểm tra máy tính của bạn:
1. **Nhấn phím Windows + R**
2. **Gõ "msinfo32"** và nhấn Enter
3. **Xem thông tin:**
   - System Type: phải có "x64" (64-bit)
   - Total Physical Memory: tối thiểu 4GB

---

## 2. CÀI ĐẶT PYTHON

### 🐍 Tại sao cần Python?
Python là "động cơ" chạy phần mềm. Giống như bạn cần có Microsoft Office để mở file Word.

### 📥 Cách cài đặt Python:

#### Bước 1: Tải Python
1. **Mở trình duyệt** (Chrome, Edge, Firefox...)
2. **Vào địa chỉ:** https://python.org
3. **Nhấn nút "Downloads"** (màu vàng)
4. **Chọn "Python 3.11.x"** (phiên bản mới nhất)
5. **Tải file** (khoảng 30MB)

#### Bước 2: Cài đặt Python
1. **Chạy file vừa tải** (python-3.11.x-amd64.exe)
2. **✅ QUAN TRỌNG: Tích vào "Add Python to PATH"** (ô vuông dưới cùng)
3. **Nhấn "Install Now"**
4. **Chờ 5-10 phút** cho quá trình cài đặt
5. **Nhấn "Close"** khi hoàn thành

#### Bước 3: Kiểm tra Python đã cài thành công
1. **Nhấn Windows + R**
2. **Gõ "cmd"** và nhấn Enter
3. **Gõ lệnh:** `python --version`
4. **Nếu thấy "Python 3.11.x"** → ✅ Thành công!
5. **Nếu báo lỗi** → ❌ Cài lại Python, nhớ tích "Add to PATH"

---

## 3. TẢI VÀ CÀI ĐẶT PHẦN MỀM

### 📂 Tạo thư mục làm việc:
1. **Mở File Explorer** (Windows + E)
2. **Vào ổ C:** hoặc D:
3. **Tạo thư mục mới:** "DuToanXayDung"
4. **Vào trong thư mục** vừa tạo

### 💾 Tải phần mềm:

#### Cách 1: Tải từ GitHub (Khuyến nghị)
1. **Vào:** https://github.com/your-username/construction-estimate
2. **Nhấn nút "Code"** (màu xanh)
3. **Chọn "Download ZIP"**
4. **Giải nén file ZIP** vào thư mục "DuToanXayDung"

#### Cách 2: Sao chép thủ công
1. **Tạo các file** trong thư mục "DuToanXayDung":
   - `main.py`
   - `requirements.txt`  
   - `data_updater.py`
   - `ai_engine.py`
   - `cloud_sync.py`
   - `digital_signature.py`
   - `report_generator.py`
2. **Sao chép nội dung** từ các file tôi đã tạo ở trên

### 🔧 Cài đặt thư viện cần thiết:

#### Bước 1: Mở Command Prompt
1. **Vào thư mục "DuToanXayDung"**
2. **Nhấn Shift + Chuột phải** trong thư mục trống
3. **Chọn "Open PowerShell window here"** hoặc "Open command window here"

#### Bước 2: Cài đặt thư viện
```bash
# Sao chép và dán từng dòng này vào Command Prompt:

pip install pandas
pip install openpyxl  
pip install reportlab
pip install Pillow
pip install matplotlib
pip install numpy
pip install scikit-learn
pip install requests
```

**⏱️ Chờ 10-15 phút** để tải và cài đặt tất cả thư viện.

---

## 4. CHẠY PHẦN MỀM LẦN ĐẦU

### 🚀 Khởi chạy phần mềm:

#### Cách 1: Chạy từ Command Prompt
1. **Mở Command Prompt** trong thư mục phần mềm
2. **Gõ lệnh:** `python main.py`
3. **Nhấn Enter**

#### Cách 2: Tạo file BAT để chạy dễ dàng
1. **Tạo file mới** tên "ChayPhanMem.bat"
2. **Mở bằng Notepad** và gõ:
```batch
@echo off
cd /d "%~dp0"
python main.py
pause
```
3. **Lưu file**
4. **Double-click vào file BAT** để chạy phần mềm

### ✅ Dấu hiệu thành công:
- Cửa sổ phần mềm hiện ra
- Có giao diện với bảng dự toán
- Không có thông báo lỗi màu đỏ

### ❌ Nếu có lỗi:
- Xem phần [Khắc phục sự cố](#6-khắc-phục-sự-cố-thường-gặp)

---

## 5. HƯỚNG DẪN SỬ DỤNG CỞ BẢN

### 🏗️ Tạo dự án đầu tiên:

#### Bước 1: Nhập thông tin dự án
1. **Tên dự án:** "Nhà ở gia đình - Thử nghiệm"
2. **Địa điểm:** "Hà Nội"  
3. **Chủ đầu tư:** "Anh/Chị [Tên bạn]"
4. **Ngày:** Để mặc định

#### Bước 2: Thêm hạng mục công việc
1. **Nhấn nút "➕ Thêm hạng mục"**
2. **Chọn định mức** từ danh sách (ví dụ: "Đào đất thủ công")
3. **Nhập khối lượng:** 100 (m³)
4. **Đơn giá** sẽ tự động hiện
5. **Nhấn "Thêm"**

#### Bước 3: Thêm nhiều hạng mục
Thêm tiếp các hạng mục:
- Bê tông C20: 50 m³
- Thép phi 10: 2000 kg  
- Gạch ống: 5000 viên

#### Bước 4: Tính toán tổng chi phí
1. **Nhấn "📊 Tính toán"**
2. **Xem kết quả** ở phần "Tổng kết" dưới cùng

#### Bước 5: Lưu dự án
1. **Menu "Tệp" → "Lưu dự án"**
2. **Đặt tên file:** "DuAnThuNghiem.json"
3. **Chọn thư mục lưu**

#### Bước 6: Xuất Excel
1. **Menu "Tệp" → "Xuất Excel"**
2. **Chọn nơi lưu file**
3. **Mở Excel** để xem kết quả

### 🤖 Sử dụng tính năng AI (Nâng cao):

#### Dự báo giá vật liệu:
1. **Chọn 1 hạng mục** trong bảng
2. **Nhấn "🤖 AI Tối ưu"** hoặc "📈 Dự báo giá"
3. **Xem kết quả** AI phân tích

#### Tối ưu chi phí:
1. **Tạo dự án** với nhiều hạng mục
2. **Nhấn "💰 Tối ưu chi phí"**
3. **AI sẽ gợi ý** vật liệu thay thế tiết kiệm

---

## 6. KHẮC PHỤC SỰ CỐ THƯỜNG GẶP

### ❌ Lỗi: "Python is not recognized"

**Nguyên nhân:** Chưa cài Python hoặc chưa thêm vào PATH

**Cách khắc phục:**
1. Gỡ Python cũ (Control Panel → Programs)
2. Cài lại Python, **nhớ tích "Add Python to PATH"**
3. Khởi động lại máy tính

### ❌ Lỗi: "No module named 'pandas'"

**Nguyên nhân:** Chưa cài thư viện

**Cách khắc phục:**
```bash
pip install pandas openpyxl reportlab matplotlib numpy scikit-learn
```

### ❌ Lỗi: "Permission denied"

**Nguyên nhân:** Không đủ quyền ghi file

**Cách khắc phục:**
1. **Chạy Command Prompt as Administrator:**
   - Tìm "cmd" trong Start Menu
   - Chuột phải → "Run as administrator"
2. Hoặc **chuyển thư mục** sang Desktop hoặc Documents

### ❌ Phần mềm chạy chậm

**Cách khắc phục:**
1. **Đóng các ứng dụng** khác đang chạy
2. **Tăng RAM** nếu có thể
3. **Chạy phiên bản cơ bản** (main.py) thay vì phiên bản AI

### ❌ Không xuất được Excel/PDF

**Cách khắc phục:**
1. **Đóng Excel** nếu đang mở file cùng tên
2. **Chọn thư mục khác** để lưu (Desktop, Documents)
3. **Chạy phần mềm as Administrator**

### ❌ Giao diện bị lỗi font tiếng Việt

**Cách khắc phục:**
1. **Cài font tiếng Việt:** Arial Unicode MS
2. **Thay đổi Regional Settings:**
   - Control Panel → Region → Administrative
   - Change system locale → Vietnamese

---

## 🆘 HỖ TRỢ KHẨN CẤP

### 📞 Khi cần trợ giúp:

1. **Chụp màn hình lỗi** (Print Screen)
2. **Ghi lại bước** đang làm khi bị lỗi
3. **Liên hệ hỗ trợ:**
   - 📧 Email: support@dutoanxaydung.com
   - 💬 Telegram: @dutoanxaydung
   - 📱 Hotline: 1900-xxxx

### 📝 Thông tin cần cung cấp:
- Hệ điều hành Windows phiên bản nào
- Phiên bản Python (python --version)
- Thông báo lỗi chính xác
- Bước đang làm khi bị lỗi

---

## 🎯 CHECKLIST HOÀN THÀNH

Đánh dấu ✅ khi hoàn thành:

**Chuẩn bị:**
- [ ] Kiểm tra máy tính đủ cấu hình
- [ ] Có kết nối internet ổn định

**Cài đặt:**
- [ ] Tải và cài Python 3.11
- [ ] Tích "Add Python to PATH"
- [ ] Kiểm tra `python --version` thành công
- [ ] Tạo thư mục "DuToanXayDung"
- [ ] Tải và giải nén phần mềm
- [ ] Cài đặt thư viện `pip install...`

**Sử dụng:**
- [ ] Chạy `python main.py` thành công
- [ ] Giao diện hiện ra không lỗi
- [ ] Tạo được dự án đầu tiên
- [ ] Thêm được hạng mục
- [ ] Tính toán được tổng chi phí
- [ ] Lưu và xuất Excel thành công

**Hoàn thành:** 
- [ ] Đã sử dụng thành thạo các chức năng cơ bản
- [ ] Biết cách khắc phục lỗi thường gặp
- [ ] Có thể tạo dự toán hoàn chỉnh

---

## 🎉 CHÚC MỪNG!

🏆 **Bạn đã thành công cài đặt và sử dụng phần mềm Dự toán Xây dựng!**

💡 **Lời khuyên:**
- Luyện tập với các dự án nhỏ trước
- Khám phá từng tính năng một cách từ từ
- Backup dữ liệu thường xuyên
- Cập nhật phần mềm định kỳ

🚀 **Bước tiếp theo:**
- Tìm hiểu tính năng AI Assistant
- Thử nghiệm Cloud Sync
- Tạo biểu mẫu đấu thầu chuyên nghiệp

**Chúc bạn thành công trong công việc dự toán xây dựng!** 🏗️✨