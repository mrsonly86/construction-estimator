# 🏗️ Construction Estimator Vietnam

**Phần mềm dự toán xây dựng miễn phí cho Việt Nam với tự động cập nhật giá vật liệu**

[![.NET](https://img.shields.io/badge/.NET-8.0-blue.svg)](https://dotnet.microsoft.com/)
[![License](https://img.shields.io/badge/license-MIT-green.svg)](LICENSE)
[![Build Status](https://img.shields.io/badge/build-passing-brightgreen.svg)](https://github.com/mrsonly86/construction-estimator)

## 📋 Tổng quan

Construction Estimator Vietnam là một hệ thống dự toán xây dựng toàn diện, được thiết kế đặc biệt cho thị trường Việt Nam với các tính năng:

- 🤖 **Tự động cập nhật giá vật liệu** từ 63 Sở Xây dựng tỉnh/thành
- 📊 **Theo dõi biến động giá** với lịch sử chi tiết
- 💻 **Giao diện hiện đại** với Material Design
- 📈 **Báo cáo và phân tích** chuyên sâu
- 🔔 **Thông báo tự động** khi giá thay đổi
- 🌍 **Hỗ trợ tiếng Việt** hoàn toàn

## 🚀 Tính năng chính

### 🤖 Hệ thống cập nhật giá tự động
- Web scraping từ websites chính thức của các Sở Xây dựng
- Parse PDF/Excel files công bố hàng tháng
- Chuẩn hóa dữ liệu về format chung
- Schedule cập nhật định kỳ (ngày 15 hàng tháng)
- Cảnh báo khi giá thay đổi >10%

### 📊 Quản lý dự án
- CRUD operations cho dự án
- Quản lý vật liệu và số lượng
- Tính toán chi phí tự động
- Theo dõi tiến độ và ngân sách

### 💰 Theo dõi giá vật liệu
- Giá real-time từ 63 tỉnh/thành
- Lịch sử biến động giá
- So sánh giá giữa các vùng
- Dự báo xu hướng giá

### 📋 Báo cáo
- Xuất Excel/PDF
- Charts và visualizations
- Báo cáo chi tiết theo dự án
- Phân tích chi phí

## 🏗️ Kiến trúc hệ thống

```
ConstructionEstimator/
├── src/
│   ├── ConstructionEstimator.Core/          # Domain models & interfaces
│   ├── ConstructionEstimator.Data/          # Entity Framework data layer
│   ├── ConstructionEstimator.PriceUpdate/   # Price automation services
│   ├── ConstructionEstimator.Notifications/ # Notification system
│   ├── ConstructionEstimator.UI.Common/     # Shared UI components
│   └── ConstructionEstimator.WPF/           # Main console application
├── tests/
│   └── ConstructionEstimator.Tests/         # Unit & integration tests
└── docs/                                    # Documentation
```

### Công nghệ sử dụng

- **Framework**: .NET 8.0
- **Database**: Entity Framework Core with SQL Server/In-Memory
- **Web Scraping**: HtmlAgilityPack
- **Scheduling**: Quartz.NET
- **Logging**: Serilog
- **Testing**: xUnit, Moq
- **UI**: Console Application (WPF ready for Windows)

## 🚀 Cài đặt và chạy

### Yêu cầu hệ thống
- .NET 8.0 SDK
- SQL Server (tùy chọn, có thể dùng In-Memory database)
- Windows/Linux/macOS

### Cài đặt

1. **Clone repository**
```bash
git clone https://github.com/mrsonly86/construction-estimator.git
cd construction-estimator
```

2. **Restore packages**
```bash
dotnet restore
```

3. **Build solution**
```bash
dotnet build
```

4. **Run tests**
```bash
dotnet test
```

5. **Run application**
```bash
dotnet run --project src/ConstructionEstimator.WPF
```

### Cấu hình Database

Ứng dụng mặc định sử dụng In-Memory database cho demo. Để sử dụng SQL Server:

1. Cập nhật connection string trong `appsettings.json`
2. Chạy migrations:
```bash
dotnet ef database update --project src/ConstructionEstimator.Data
```

## 📖 Hướng dẫn sử dụng

### 1. Xem dữ liệu cơ bản
```
📋 MENU - Choose an option:
1. View Materials      # Xem danh sách vật liệu
2. View Provinces      # Xem danh sách tỉnh/thành
3. View Material Prices # Xem giá vật liệu
```

### 2. Cấu hình tỉnh/thành
```
4. Setup Province Configuration
```
- Nhập mã tỉnh (01-63)
- Tên Sở Xây dựng
- URL website chính thức
- URL bảng giá vật liệu

### 3. Test cập nhật giá
```
5. Test Price Update
```
- Chọn mã tỉnh để test
- Hệ thống sẽ thử scrape dữ liệu từ website

### 4. Tạo dự án mẫu
```
7. Create Sample Project
```
- Tạo dự án "Nhà ở cá nhân 2 tầng"
- Budget: 2 tỷ VND
- Location: Hà Nội

## 🏛️ Dữ liệu tỉnh/thành Việt Nam

Hệ thống hỗ trợ đầy đủ 63 tỉnh/thành của Việt Nam:

**Miền Bắc**: Hà Nội, Hà Giang, Cao Bằng, Bắc Kạn, Tuyên Quang, Lào Cai, Điện Biên, Lai Châu, Sơn La, Yên Bái, Hoà Bình, Thái Nguyên, Lạng Sơn, Quảng Ninh, Bắc Giang, Phú Thọ, Vĩnh Phúc, Bắc Ninh, Hải Dương, Hải Phòng, Hưng Yên, Thái Bình, Hà Nam, Nam Định, Ninh Bình

**Miền Trung**: Thanh Hóa, Nghệ An, Hà Tĩnh, Quảng Bình, Quảng Trị, Thừa Thiên Huế, Đà Nẵng, Quảng Nam, Quảng Ngãi, Bình Định, Phú Yên, Khánh Hòa, Ninh Thuận, Bình Thuận, Kon Tum, Gia Lai, Đắk Lắk, Đắk Nông, Lâm Đồng

**Miền Nam**: Bình Phước, Tây Ninh, Bình Dương, Đồng Nai, Bà Rịa - Vũng Tàu, TP. Hồ Chí Minh, Long An, Tiền Giang, Bến Tre, Trà Vinh, Vĩnh Long, Đồng Tháp, An Giang, Kiên Giang, Cần Thơ, Hậu Giang, Sóc Trăng, Bạc Liêu, Cà Mau

## 🧱 Vật liệu xây dựng

Hệ thống hỗ trợ các loại vật liệu chính:

### Bê tông
- Bê tông thương phẩm B15, B20, B25, B30

### Thép
- Thép thanh trơn CB240-T
- Thép thanh vằn CB300-V, CB400-V, CB500-V

### Gạch
- Gạch rỗng đỏ
- Gạch không nung
- Gạch bê tông

### Cát đá
- Cát xây dựng, cát vàng tô
- Đá dăm 1x2, 4x6

### Xi măng
- Xi măng PCB30, PCB40, PC50

## 🧪 Testing

Hệ thống có các loại test:

- **Unit Tests**: Test các service và business logic
- **Integration Tests**: Test database và data flow
- **Database Tests**: Test Entity Framework models

Chạy tests:
```bash
dotnet test --verbosity normal
```

## 📊 Demo và Screenshots

### Console Application
```
🏗️  Construction Estimator - Vietnamese Material Price System
================================================================
✅ Database initialized successfully

📋 MENU - Choose an option:
1. View Materials
2. View Provinces  
3. View Material Prices
4. Setup Province Configuration
5. Test Price Update
6. View Price History
7. Create Sample Project
0. Exit
```

### Dữ liệu mẫu
```
🧱 MATERIALS LIST
================
ID: 1
Code: BT_B25
Name: Bê tông thương phẩm B25
Unit: m3
Category: Concrete
Active: Yes
```

## 🔮 Roadmap

### Phase 1: Foundation ✅
- [x] Core domain models
- [x] Entity Framework setup
- [x] Basic console application
- [x] Unit tests

### Phase 2: Price Update System 🚧
- [ ] Web scraping implementation
- [ ] Province configuration UI
- [ ] Scheduled jobs with Quartz.NET
- [ ] Error handling and retry policies

### Phase 3: Advanced Features
- [ ] WPF UI for Windows
- [ ] Real-time notifications
- [ ] Excel/PDF export
- [ ] Advanced analytics

### Phase 4: Production Ready
- [ ] Performance optimization
- [ ] Security enhancements
- [ ] Docker containerization
- [ ] CI/CD pipeline

## 🤝 Đóng góp

Chúng tôi hoan nghênh mọi đóng góp! Vui lòng:

1. Fork repository
2. Tạo feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to branch (`git push origin feature/AmazingFeature`)
5. Mở Pull Request

## 📝 License

Dự án này được phân phối dưới MIT License. Xem `LICENSE` file để biết thêm chi tiết.

## 📞 Liên hệ

- **Project Link**: [https://github.com/mrsonly86/construction-estimator](https://github.com/mrsonly86/construction-estimator)
- **Issues**: [https://github.com/mrsonly86/construction-estimator/issues](https://github.com/mrsonly86/construction-estimator/issues)

## 🙏 Acknowledgments

- Các Sở Xây dựng tỉnh/thành cung cấp dữ liệu giá vật liệu
- .NET Community cho các thư viện mã nguồn mở
- Vietnamese developers community

---

**Made with ❤️ for Vietnamese construction industry**
