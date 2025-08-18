# Phần mềm Dự toán Xây dựng Chuyên nghiệp

![Construction Estimator](https://img.shields.io/badge/Version-1.0.0-blue.svg)
![.NET](https://img.shields.io/badge/.NET-8.0-purple.svg)
![License](https://img.shields.io/badge/License-Free-green.svg)

Phần mềm dự toán xây dựng miễn phí, chuyên nghiệp cho thị trường Việt Nam - cạnh tranh trực tiếp với ETA, G8, F1.

## 🎯 Mục tiêu

Xây dựng một phần mềm dự toán xây dựng hoàn chỉnh cho Windows với các tính năng vượt trội và hoàn toàn miễn phí, phục vụ cộng đồng xây dựng Việt Nam.

## ✨ Tính năng chính

### 🖥️ Giao diện người dùng
- Giao diện hiện đại với Material Design
- Hỗ trợ đa ngôn ngữ (Tiếng Việt, Tiếng Anh)
- Theme tối/sáng tùy chỉnh
- Toolbar và menu đầy đủ chức năng

### 📊 Quản lý dự án
- Tạo, mở, lưu dự án dự toán
- Import/Export từ Excel, Word, PDF
- Backup và restore dự án tự động
- Lịch sử thay đổi (version control)

### 🧮 Tính toán dự toán
- Tính toán khối lượng tự động theo công thức
- Quản lý đơn giá vật liệu, nhân công, máy thi công
- Tính toán chi phí chung, lợi nhuận, thuế VAT
- Hỗ trợ nhiều loại công trình (dân dụng, công nghiệp, giao thông)

### 💾 Cơ sở dữ liệu
- Database định mức xây dựng Việt Nam cập nhật
- Quản lý giá vật liệu theo thời gian và khu vực
- Tích hợp API lấy giá vật liệu online
- Import định mức từ các nguồn khác nhau

### 📋 Báo cáo và xuất file
- Xuất báo cáo dự toán chi tiết, tổng hợp
- Xuất ra Excel, Word, PDF với template tùy chỉnh
- In ấn chuyên nghiệp
- Biểu đồ và đồ thị phân tích chi phí

### 🚀 Tính năng nâng cao
- Tính toán theo BIM (Building Information Modeling)
- Tích hợp AI để dự đoán giá vật liệu
- Phân tích rủi ro và biến động giá
- Quản lý tiến độ thi công và thanh toán
- Đồng bộ với cloud storage

## 🛠️ Công nghệ sử dụng

- **Frontend**: WPF (Windows Presentation Foundation) với C#
- **Database**: SQLite cho local, SQL Server cho enterprise
- **Framework**: .NET 8
- **UI Framework**: MaterialDesignInXamlToolkit
- **Architecture**: MVVM Pattern với CommunityToolkit.Mvvm
- **ORM**: Entity Framework Core
- **Reporting**: Custom reporting engine
- **Packaging**: WiX Toolset cho Windows Installer

## 📁 Cấu trúc dự án

```
ConstructionEstimator/
├── src/
│   ├── ConstructionEstimator.Core/          # Business logic và models
│   ├── ConstructionEstimator.Data/          # Data access layer với EF Core
│   ├── ConstructionEstimator.WPF/           # WPF UI với Material Design
│   ├── ConstructionEstimator.Reports/       # Reporting engine
│   └── ConstructionEstimator.Tests/         # Unit tests với xUnit
├── data/
│   ├── database/                            # Database scripts và sample data
│   ├── standards/                           # Định mức xây dựng Việt Nam
│   └── templates/                           # Report templates
├── docs/                                    # Documentation
└── installer/                               # Windows installer với WiX
```

## ⚙️ Yêu cầu hệ thống

- **OS**: Windows 10/11 (64-bit)
- **Framework**: .NET 8 Runtime
- **RAM**: Tối thiểu 4GB, khuyến nghị 8GB
- **Storage**: 2GB cho cài đặt cơ bản
- **Display**: Hỗ trợ độ phân giải từ 1366x768 trở lên

## 🚀 Cài đặt và chạy

### Prerequisites
1. Cài đặt [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
2. Cài đặt Visual Studio 2022 hoặc VS Code

### Build và chạy
```bash
# Clone repository
git clone https://github.com/mrsonly86/construction-estimator.git
cd construction-estimator

# Restore packages
dotnet restore

# Build solution
dotnet build

# Run WPF application (chỉ trên Windows)
dotnet run --project src/ConstructionEstimator.WPF
```

### Database Setup
```bash
# Tạo database migrations
dotnet ef migrations add InitialCreate --project src/ConstructionEstimator.Data

# Update database
dotnet ef database update --project src/ConstructionEstimator.Data
```

## 🎯 Ưu điểm cạnh tranh

| Tính năng | Construction Estimator | ETA/G8/F1 |
|-----------|----------------------|-----------|
| **Giá** | Hoàn toàn miễn phí | Tính phí license |
| **Cập nhật** | Liên tục, tự động | Theo phiên bản |
| **Tùy chỉnh** | Mã nguồn mở | Hạn chế |
| **AI Support** | Tích hợp AI/ML | Không có |
| **Cloud Sync** | Có | Hạn chế |
| **Community** | Open source | Closed source |

## 📅 Lộ trình phát triển

### ✅ Giai đoạn 1: Foundation (Hoàn thành)
- [x] Tạo cấu trúc dự án hoàn chỉnh
- [x] Thiết lập models và database schema
- [x] Cấu hình WPF với Material Design
- [x] Dependency Injection setup
- [x] Basic MVVM architecture

### 🔄 Giai đoạn 2: Core Features (Đang phát triển)
- [ ] Giao diện chính và navigation
- [ ] Quản lý dự án (CRUD operations)
- [ ] Database migrations và seeding
- [ ] Basic estimation calculations

### 📋 Giai đoạn 3: Advanced Features
- [ ] Tính toán dự toán và quản lý vật liệu
- [ ] Standards và norms management
- [ ] Price management với regional support

### 📊 Giai đoạn 4: Reporting
- [ ] Báo cáo và xuất file (Excel, PDF, Word)
- [ ] Custom report templates
- [ ] Charts và visualization

### 🚀 Giai đoạn 5: Advanced Features
- [ ] BIM integration
- [ ] AI price prediction
- [ ] Cloud synchronization
- [ ] Mobile companion app

## 🤝 Đóng góp

Chúng tôi hoan nghênh mọi đóng góp từ cộng đồng! 

### Cách đóng góp:
1. Fork repository
2. Tạo feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to branch (`git push origin feature/AmazingFeature`)
5. Tạo Pull Request

### Coding Standards:
- Sử dụng C# coding conventions
- Viết unit tests cho features mới
- Tuân thủ MVVM pattern
- Comment bằng tiếng Việt cho business logic

## 📄 License

Dự án này được phát hành dưới **MIT License** - hoàn toàn miễn phí cho mọi mục đích sử dụng.

## 📞 Liên hệ

- **Email**: support@constructionestimator.vn
- **Website**: https://constructionestimator.vn
- **Issues**: [GitHub Issues](https://github.com/mrsonly86/construction-estimator/issues)
- **Discussions**: [GitHub Discussions](https://github.com/mrsonly86/construction-estimator/discussions)

## 🙏 Cảm ơn

Cảm ơn tất cả các contributors và cộng đồng xây dựng Việt Nam đã hỗ trợ dự án này!

---

**Made with ❤️ for Vietnamese Construction Industry**
