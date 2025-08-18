# Construction Estimator - Phần mềm dự toán xây dựng

Phần mềm dự toán xây dựng chuyên nghiệp được xây dựng bằng .NET 8, Entity Framework Core và SQLite.

## ✨ Tính năng chính

### 🏗️ Quản lý dự án
- ✅ Tạo, chỉnh sửa và quản lý dự án xây dựng
- ✅ Thông tin khách hàng chi tiết
- ✅ Theo dõi trạng thái dự án (Nháp, Đang thực hiện, Hoàn thành, etc.)
- ✅ Lưu trữ thông tin địa điểm và thời gian

### 💰 Dự toán chi phí
- ✅ Quản lý hạng mục dự toán chi tiết
- ✅ Tính toán tự động (vật liệu, nhân công, thiết bị)
- ✅ Phần trăm lợi nhuận và dự phòng có thể tùy chỉnh
- ✅ Tổng hợp chi phí theo từng loại
- ✅ Báo cáo phân tích chi phí

### 📦 Cơ sở dữ liệu vật liệu
- ✅ Quản lý vật liệu xây dựng
- ✅ Phân loại theo danh mục (bê tông, thép, gạch, etc.)
- ✅ Cập nhật giá cả theo thời gian
- ✅ Thông tin nhà cung cấp và thương hiệu

### 👷 Quản lý nhân công
- ✅ Cơ sở dữ liệu nhân công
- ✅ Phân loại theo kỹ năng (phổ thông, có kỹ năng, chuyên gia)
- ✅ Đơn giá theo giờ/ngày/tháng
- ✅ Hệ số năng suất

### 🏭 Quản lý thiết bị
- ✅ Cơ sở dữ liệu máy móc thiết bị
- ✅ Chi phí thuê theo thời gian
- ✅ Thông tin kỹ thuật và công suất

## 🏛️ Kiến trúc phần mềm

### Cấu trúc dự án
```
ConstructionEstimator/
├── src/
│   ├── ConstructionEstimator.Core/          # Entities, Models, Interfaces
│   ├── ConstructionEstimator.Data/          # Entity Framework, Repositories
│   ├── ConstructionEstimator.Services/      # Business Logic Services
│   ├── ConstructionEstimator.Reports/       # Export & Reporting
│   └── ConstructionEstimator.Console/       # Console Demo Application
├── tests/
│   └── ConstructionEstimator.Tests.Unit/    # Unit Tests
└── README.md
```

### 🛠️ Công nghệ sử dụng

#### Backend & Data
- **.NET 8.0** - Platform chính
- **Entity Framework Core 8.0** - ORM cho database
- **SQLite** - Database nhẹ, dễ triển khai
- **AutoMapper** - Object mapping
- **FluentValidation** - Validation rules

#### Dependency Injection & Configuration
- **Microsoft.Extensions.DependencyInjection** - IoC Container
- **Microsoft.Extensions.Configuration** - Configuration management
- **Microsoft.Extensions.Hosting** - Application hosting

#### Logging
- **Serilog** - Structured logging
- Log ra file và console

#### Testing
- **xUnit** - Unit testing framework
- **Entity Framework InMemory** - In-memory database for testing

#### Reports & Export
- **EPPlus** - Excel export
- **iTextSharp** - PDF generation
- **DocumentFormat.OpenXml** - Word documents

## 🚀 Cài đặt và chạy

### Yêu cầu hệ thống
- .NET 8.0 SDK
- Visual Studio 2022 hoặc VS Code (khuyến nghị)

### Clone và Build
```bash
git clone <repository-url>
cd construction-estimator
dotnet restore
dotnet build
```

### Chạy ứng dụng demo
```bash
dotnet run --project src/ConstructionEstimator.Console
```

### Chạy tests
```bash
dotnet test
```

## 📊 Demo Features

Ứng dụng console demo sẽ:

1. **Khởi tạo database** với dữ liệu mẫu
2. **Hiển thị vật liệu** có sẵn trong hệ thống
3. **Tạo dự án mẫu** "Nhà ở gia đình 2 tầng" với:
   - 7 hạng mục dự toán
   - Vật liệu: Xi măng, thép, gạch (97M VND)
   - Nhân công: Công nhân xây dựng, thợ hàn (19.7M VND)  
   - Thiết bị: Cần cẩu, máy trộn (8.5M VND)
4. **Tính toán chi phí** tự động:
   - Chi phí trực tiếp: 125.3M VND
   - Chi phí quản lý (15%): 3M VND
   - Lợi nhuận (12%): 15.4M VND
   - Dự phòng (8%): 11.5M VND
   - **Tổng cộng: 155.1M VND**

## 📁 Cơ sở dữ liệu

### Schema chính
- **Projects** - Dự án
- **EstimateItems** - Hạng mục dự toán
- **Materials** - Vật liệu xây dựng
- **Labor** - Nhân công
- **Equipment** - Thiết bị máy móc
- **Standards** - Định mức xây dựng
- **PriceLists** - Bảng giá
- **Categories** - Danh mục phân loại

### Dữ liệu mẫu
- Xi măng PCB40: 2.5M VND/tấn
- Thép CB300-V: 18M VND/tấn  
- Gạch ống 4 lỗ: 1,500 VND/viên
- Công nhân xây dựng: 350K VND/ngày
- Thợ hàn: 500K VND/ngày
- Kỹ sư giám sát: 800K VND/ngày

## 🧮 Công thức tính toán

### Cấu trúc chi phí
1. **Chi phí trực tiếp** = Vật liệu + Nhân công + Thiết bị
2. **Chi phí quản lý** = Nhân công × 15%
3. **Chi phí trước lợi nhuận** = Chi phí trực tiếp + Chi phí quản lý
4. **Lợi nhuận** = Chi phí trước lợi nhuận × % Lợi nhuận
5. **Chi phí trước dự phòng** = Chi phí trước lợi nhuận + Lợi nhuận
6. **Dự phòng** = Chi phí trước dự phòng × % Dự phòng
7. **Tổng giá trị** = Chi phí trước dự phòng + Dự phòng

## 📈 Kế hoạch phát triển

### Version 2.0 (Planned)
- [ ] WPF Desktop Application với Material Design
- [ ] Import/Export Excel nâng cao
- [ ] Báo cáo PDF chuyên nghiệp
- [ ] Tích hợp BIM (Building Information Modeling)
- [ ] AI prediction cho giá vật liệu
- [ ] Cloud sync và backup
- [ ] Multi-language support
- [ ] Advanced reporting dashboard

### Version 3.0 (Future)
- [ ] Web application với Blazor
- [ ] Mobile app support
- [ ] Project collaboration features
- [ ] Integration với phần mềm kế toán
- [ ] Advanced analytics và machine learning

## 🤝 Đóng góp

Chúng tôi hoan nghênh mọi đóng góp! Vui lòng:

1. Fork repository
2. Tạo feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to branch (`git push origin feature/AmazingFeature`)
5. Tạo Pull Request

## 📝 License

Dự án này được phân phối dưới giấy phép MIT. Xem file `LICENSE` để biết thêm chi tiết.

## 📞 Liên hệ

- Email: support@constructionestimator.vn
- Website: https://constructionestimator.vn
- Issues: https://github.com/mrsonly86/construction-estimator/issues

---

**© 2024 Construction Estimator - Phần mềm dự toán xây dựng Việt Nam**
