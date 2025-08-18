# Construction Estimator
**Phần mềm dự toán miễn phí Việt Nam**

## Tổng quan
Construction Estimator là hệ thống quản lý giá vật liệu xây dựng với khả năng tự động cập nhật giá từ 63 tỉnh/thành phố trên toàn quốc. Hệ thống được xây dựng theo kiến trúc MVVM với WPF UI và backend services mạnh mẽ.

## Kiến trúc hệ thống

### 1. ConstructionEstimator.Core
**Models & Service Interfaces**
- `MaterialPrice`: Model quản lý thông tin giá vật liệu theo tỉnh/thành
- `IMaterialPriceUpdateService`: Interface cho việc fetch/update giá từ nguồn bên ngoài
- `IProvinceConfigService`: Quản lý cấu hình 63 tỉnh/thành và nguồn dữ liệu
- `IPriceHistoryService`: Lưu trữ và truy vấn lịch sử biến động giá
- `INotificationService`: Thông báo khi có thay đổi giá quan trọng

### 2. ConstructionEstimator.Infrastructure
**Service Implementations**
- `MaterialPriceUpdateService`: Service chính để crawl/fetch giá vật liệu với mock data
- `ProvinceConfigService`: Cấu hình nguồn dữ liệu cho 63 tỉnh/thành Việt Nam
- `PriceHistoryService`: Quản lý lịch sử giá (in-memory storage)
- `NotificationService`: Gửi thông báo về biến động giá qua console/debug

### 3. ConstructionEstimator.WPF
**MVVM UI Layer**
- `MaterialPriceViewModel`: Quản lý bảng giá vật liệu với command refresh
- `PriceHistoryViewModel`: Hiển thị lịch sử biến động giá
- `NotificationViewModel`: Quản lý danh sách thông báo
- `DashboardViewModel`: Tổng quan biến động giá và vật liệu quan trọng
- `MaterialPriceDataProvider`: Bridge giữa backend service và frontend ViewModel
- XAML Views với modern UI design

## Tính năng chính

### ✅ Material Price Auto-Update System
- Hỗ trợ 63 tỉnh/thành Việt Nam với mock data endpoints
- Tự động phát hiện và thông báo biến động giá >2%
- Lưu trữ lịch sử giá in-memory với khả năng mở rộng database
- Async operations cho smooth user experience

### ✅ WPF UI Enhancement
- DataGrid hiển thị giá vật liệu: Tên, Tỉnh/Thành, Giá, Đơn vị, Ngày cập nhật
- Command binding với RefreshCommand
- ObservableCollection cho real-time UI updates
- MVVM pattern với PropertyChanged implementation
- Dashboard với thống kê tổng quan

### ✅ Demo Console Application
- Menu-driven interface bằng tiếng Việt
- Hiển thị bảng giá vật liệu theo tỉnh hoặc toàn quốc
- Cập nhật giá theo tỉnh hoặc toàn bộ
- Real-time notifications về biến động giá
- Dashboard summary với thống kê

## Vật liệu được hỗ trợ
- Xi măng PCB40 (tấn)
- Thép CB240-T, CB300-V (tấn)  
- Gạch block (viên)
- Cát xây dựng (m³)
- Đá 1x2, 4x6 (m³)
- Gỗ thông, cao su (m³)
- Đinh 2.5 (kg)

## Chạy ứng dụng

### Console Demo
```bash
dotnet run --project ConstructionEstimator.WPF
```

### WPF Application (Windows only)
```bash
# Modify ConstructionEstimator.WPF.csproj to use WPF SDK
<UseWPF>true</UseWPF>
<TargetFramework>net8.0-windows</TargetFramework>
dotnet run --project ConstructionEstimator.WPF
```

## Mở rộng tương lai
- Real web crawling implementation
- Database integration (Entity Framework)
- REST API exposure
- Chart/graph visualization với Live Charts
- Advanced filtering và search
- Export to Excel/PDF
- Real-time notifications via SignalR
- Mobile app với Xamarin/MAUI

## Công nghệ sử dụng
- .NET 8.0
- WPF (Windows Presentation Foundation)
- MVVM Pattern
- Async/Await programming
- Observer Pattern for notifications
- Clean Architecture principles

## Screenshots

### Console Application
- ✅ Menu chính với 5 chức năng
- ✅ Bảng giá vật liệu theo tỉnh/thành
- ✅ Cập nhật giá với thông báo real-time
- ✅ Hệ thống thông báo
- ✅ Dashboard tổng quan

### WPF UI (Design)
- Modern TabControl interface
- DataGrid với alternating row colors
- Summary cards với color-coded statistics
- Real-time notification list
- Province filter dropdown
