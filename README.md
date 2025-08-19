# 🏗️ Phần mềm Dự toán Xây dựng Chuyên nghiệp

> **Phần mềm dự toán xây dựng miễn phí, vượt trội hơn ETA, G8, F1**

![Version](https://img.shields.io/badge/version-1.0.0-blue.svg)
![License](https://img.shields.io/badge/license-Free-green.svg)
![Platform](https://img.shields.io/badge/platform-Windows-lightgrey.svg)
![Python](https://img.shields.io/badge/python-3.8+-yellow.svg)

## 🎯 Giới thiệu

Phần mềm Dự toán Xây dựng Chuyên nghiệp là giải pháp hoàn toàn **MIỄN PHÍ** giúp các kỹ sư, nhà thầu, và chủ đầu tư lập dự toán chi tiết, tính toán khối lượng, đơn giá một cách nhanh chóng và chính xác.

## ⭐ Tính năng vượt trội

### 🆚 So sánh với ETA, G8, F1

| Tính năng | Dự toán XD Pro | ETA | G8 | F1 |
|-----------|---------------|-----|----|----|
| **Giá cả** | ✅ **MIỄN PHÍ** | ❌ Có phí | ❌ Có phí | ❌ Có phí |
| **Giao diện** | ✅ Hiện đại, thân thiện | ⚠️ Cũ | ⚠️ Phức tạp | ⚠️ Khó dùng |
| **Tính toán** | ✅ Nhanh, chính xác | ✅ Tốt | ✅ Tốt | ✅ Tốt |
| **Backup** | ✅ Tự động | ⚠️ Thủ công | ⚠️ Thủ công | ⚠️ Thủ công |
| **Giới hạn dự án** | ✅ Không giới hạn | ❌ Có giới hạn | ❌ Có giới hạn | ❌ Có giới hạn |
| **Internet** | ✅ Không cần | ❌ Cần kết nối | ❌ Cần kết nối | ⚠️ Đôi khi cần |
| **Xuất báo cáo** | ✅ Excel, PDF | ✅ Excel | ✅ Excel | ✅ Excel |
| **Hỗ trợ** | ✅ Miễn phí | ❌ Có phí | ❌ Có phí | ❌ Có phí |

### 🚀 Tính năng chính

- **📊 Lập dự toán chi tiết**: Quản lý hạng mục, khối lượng, đơn giá
- **🧮 Tính toán tự động**: Tính toán nhanh chóng, chính xác với VAT
- **📋 Quản lý định mức**: Thêm, sửa, xóa định mức xây dựng
- **📄 Xuất báo cáo**: Xuất Excel, PDF với định dạng chuyên nghiệp  
- **🔍 Tìm kiếm thông minh**: Tìm kiếm nhanh theo mã, tên hạng mục
- **💾 Backup tự động**: Sao lưu dữ liệu an toàn
- **🧮 Calculator tích hợp**: Công cụ tính toán tiện lợi
- **🎨 Giao diện hiện đại**: Thân thiện, dễ sử dụng

## 📥 Cài đặt

### Cách 1: Chạy trực tiếp (cần Python)

```bash
# 1. Clone repository
git clone https://github.com/your-repo/construction-estimate.git
cd construction-estimate

# 2. Cài đặt dependencies
pip install -r requirements.txt

# 3. Chạy phần mềm
python main.py
```

### Cách 2: Sử dụng file .exe (Windows)

```bash
# 1. Chạy file cài đặt
install_dependencies.bat

# 2. Chạy file .exe
dist/DuToanXayDung.exe
```

## 🖥️ Giao diện

![Screenshot](screenshot.png)

## 📖 Hướng dẫn sử dụng

### Tạo dự án mới
1. Mở phần mềm
2. Chọn **Tệp** → **Dự án mới**
3. Nhập thông tin dự án

### Thêm hạng mục
1. Nhấn **➕ Thêm hạng mục**
2. Chọn định mức từ danh sách
3. Nhập khối lượng và đơn giá
4. Nhấn **Thêm**

### Tính toán và xuất báo cáo
1. Nhấn **📊 Tính toán** 
2. Chọn **Tệp** → **Xuất Excel/PDF**

Chi tiết xem file `HUONG_DAN_SU_DUNG.txt`

## 🛠️ Yêu cầu hệ thống

- **OS**: Windows 7/8/10/11
- **RAM**: 2GB trở lên
- **Ổ cứng**: 100MB trống
- **Python**: 3.8+ (nếu chạy từ source)

## 📦 Cấu trúc dự án

```
construction-estimate/
├── main.py                    # File chính
├── report_generator.py        # Module xuất báo cáo
├── setup.py                   # Setup cx_Freeze
├── setup_pyinstaller.py       # Setup PyInstaller  
├── requirements.txt           # Dependencies
├── install_dependencies.bat   # Script cài đặt
├── HUONG_DAN_SU_DUNG.txt     # Hướng dẫn chi tiết
└── README.md                  # File này
```

## 🤝 Đóng góp

Chúng tôi hoan nghênh mọi đóng góp! 

1. Fork dự án
2. Tạo branch mới (`git checkout -b feature/AmazingFeature`)
3. Commit changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to branch (`git push origin feature/AmazingFeature`)
5. Mở Pull Request

## 📝 Changelog

### v1.0.0 (2024-12-19)
- ✨ Phiên bản đầu tiên
- 🎨 Giao diện hiện đại với tkinter
- 📊 Tính năng dự toán cơ bản
- 📄 Xuất Excel, PDF
- 💾 Backup dữ liệu
- 🧮 Calculator tích hợp

## 🐛 Báo lỗi

Nếu bạn gặp lỗi, vui lòng tạo [Issue](https://github.com/your-repo/construction-estimate/issues) với thông tin:

- Phiên bản Windows
- Mô tả lỗi chi tiết
- Screenshot (nếu có)
- File log (nếu có)

## 📞 Hỗ trợ

- 📧 **Email**: support@dutoanxaydung.com
- 🌐 **Website**: www.dutoanxaydung.com  
- 📱 **Hotline**: 1900-xxxx
- 💬 **Telegram**: @dutoanxaydung

## 📄 Giấy phép

Dự án này được phát hành dưới giấy phép **MIT License** - xem file [LICENSE](LICENSE) để biết thêm chi tiết.

## 🙏 Cảm ơn

- Cảm ơn cộng đồng Python và tkinter
- Cảm ơn các thư viện mã nguồn mở: pandas, openpyxl, reportlab
- Cảm ơn tất cả người dùng đã tin tưởng và sử dụng

---

<div align="center">

**🎯 Được phát triển với ❤️ tại Việt Nam**

**⭐ Nếu bạn thấy hữu ích, hãy cho chúng tôi 1 star nhé!**

</div>