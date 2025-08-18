# Contributing to Construction Estimator

Cảm ơn bạn đã quan tâm đến việc đóng góp cho dự án Construction Estimator! Đây là hướng dẫn chi tiết về cách tham gia phát triển.

## 🚀 Bắt đầu nhanh

### Prerequisites
1. [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
2. [Visual Studio 2022](https://visualstudio.microsoft.com/) hoặc [VS Code](https://code.visualstudio.com/)
3. [Git](https://git-scm.com/)

### Setup môi trường phát triển
```bash
# Clone repository
git clone https://github.com/mrsonly86/construction-estimator.git
cd construction-estimator

# Restore packages
dotnet restore

# Build solution
dotnet build

# Run tests
dotnet test
```

## 📋 Quy trình đóng góp

### 1. Fork và Clone
1. Fork repository trên GitHub
2. Clone fork về máy local
3. Tạo remote upstream:
```bash
git remote add upstream https://github.com/mrsonly86/construction-estimator.git
```

### 2. Tạo feature branch
```bash
git checkout -b feature/ten-tinh-nang-moi
```

### 3. Development
- Tuân thủ coding standards
- Viết tests cho code mới
- Commit thường xuyên với messages rõ ràng

### 4. Testing
```bash
# Run all tests
dotnet test

# Run specific test project
dotnet test src/ConstructionEstimator.Tests/
```

### 5. Submit Pull Request
1. Push branch lên fork của bạn
2. Tạo Pull Request trên GitHub
3. Điền đầy đủ thông tin trong PR template

## 🎯 Loại đóng góp chúng tôi tìm kiếm

### 🐛 Bug Fixes
- Sửa lỗi được báo cáo trong Issues
- Cải thiện performance
- Sửa lỗi giao diện

### ✨ New Features
- Tính năng mới theo roadmap
- Cải thiện UX/UI
- Tích hợp với external services

### 📚 Documentation
- Cải thiện README
- Thêm code comments
- Tạo tutorials và guides

### 🧪 Testing
- Thêm unit tests
- Integration tests
- UI automation tests

## 📏 Coding Standards

### C# Code Style
```csharp
// ✅ Good
public class MaterialService : IMaterialService
{
    private readonly IDataContext _context;
    
    public MaterialService(IDataContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }
    
    public async Task<Material?> GetMaterialByIdAsync(int id)
    {
        return await _context.Materials
            .FirstOrDefaultAsync(m => m.Id == id);
    }
}

// ❌ Bad
public class materialservice : IMaterialService
{
    IDataContext context;
    
    public materialservice(IDataContext ctx)
    {
        context = ctx;
    }
    
    public Material GetMaterialById(int id)
    {
        return context.Materials.FirstOrDefault(m => m.Id == id);
    }
}
```

### Naming Conventions
- **Classes**: PascalCase (`MaterialService`)
- **Methods**: PascalCase (`GetMaterialById`)
- **Variables**: camelCase (`materialId`)
- **Constants**: PascalCase (`DefaultVatRate`)
- **Private fields**: _camelCase (`_context`)

### Comments và Documentation
```csharp
/// <summary>
/// Tính toán tổng chi phí vật liệu cho một hạng mục
/// </summary>
/// <param name="estimateItemId">ID của hạng mục dự toán</param>
/// <returns>Tổng chi phí vật liệu (VND)</returns>
public async Task<decimal> CalculateMaterialCostAsync(int estimateItemId)
{
    // Lấy danh sách vật liệu của hạng mục
    var materials = await GetEstimateItemMaterialsAsync(estimateItemId);
    
    // Tính tổng chi phí bao gồm hao hụt
    return materials.Sum(m => m.TotalCost * (1 + m.WasteFactor / 100));
}
```

## 🧪 Testing Guidelines

### Unit Tests
```csharp
[Fact]
public async Task GetMaterialByIdAsync_ExistingId_ReturnsMaterial()
{
    // Arrange
    var materialId = 1;
    var expected = new Material { Id = materialId, Name = "Test Material" };
    
    // Act
    var result = await _materialService.GetMaterialByIdAsync(materialId);
    
    // Assert
    Assert.NotNull(result);
    Assert.Equal(expected.Id, result.Id);
    Assert.Equal(expected.Name, result.Name);
}

[Fact]
public async Task GetMaterialByIdAsync_NonExistingId_ReturnsNull()
{
    // Arrange
    var materialId = 999;
    
    // Act
    var result = await _materialService.GetMaterialByIdAsync(materialId);
    
    // Assert
    Assert.Null(result);
}
```

### Test Organization
```
src/ConstructionEstimator.Tests/
├── Core/
│   ├── Services/
│   │   ├── MaterialServiceTests.cs
│   │   ├── ProjectServiceTests.cs
│   │   └── EstimateServiceTests.cs
│   └── Models/
│       ├── MaterialTests.cs
│       └── ProjectTests.cs
├── Data/
│   ├── DataContextTests.cs
│   └── RepositoryTests.cs
└── Fixtures/
    ├── DatabaseFixture.cs
    └── TestData.cs
```

## 📝 Commit Messages

### Format
```
type(scope): description

body (optional)

footer (optional)
```

### Types
- `feat`: Tính năng mới
- `fix`: Sửa bug
- `docs`: Cập nhật documentation
- `style`: Thay đổi code style (không ảnh hưởng logic)
- `refactor`: Refactor code
- `test`: Thêm hoặc sửa tests
- `chore`: Maintenance tasks

### Examples
```bash
feat(materials): add material price history tracking

Implement price history functionality to track material cost changes over time.
Includes database schema updates and service methods.

Closes #123

fix(estimates): correct calculation for waste factor

The waste factor was being applied incorrectly in material cost calculations.
Changed from additive to multiplicative formula.

docs(readme): update installation instructions

Add detailed steps for setting up development environment on Windows and macOS.

test(services): add unit tests for MaterialService

Achieve 90% code coverage for MaterialService with comprehensive test cases.
```

## 🐛 Bug Reports

### Sử dụng Issue Template
Khi báo cáo bug, vui lòng cung cấp:
- **Environment**: OS, .NET version, app version
- **Steps to reproduce**: Các bước để tái hiện lỗi
- **Expected behavior**: Kết quả mong đợi
- **Actual behavior**: Kết quả thực tế
- **Screenshots**: Nếu có
- **Logs**: Error messages hoặc stack traces

### Example Bug Report
```markdown
**Environment:**
- OS: Windows 11
- .NET: 8.0
- App Version: 1.0.0

**Steps to reproduce:**
1. Open new project
2. Add estimate item
3. Add material with quantity 0
4. Click Calculate

**Expected:** Show validation error
**Actual:** Application crashes with NullReferenceException

**Error Log:**
```
System.NullReferenceException: Object reference not set to an instance of an object.
   at ConstructionEstimator.Core.Services.EstimateService.CalculateTotal()
```

## 💡 Feature Requests

### Template
- **Feature description**: Mô tả tính năng
- **Use case**: Trường hợp sử dụng
- **Proposed solution**: Giải pháp đề xuất
- **Alternatives**: Các lựa chọn khác
- **Priority**: Mức độ ưu tiên

## 🎨 UI/UX Guidelines

### Material Design Principles
- Sử dụng MaterialDesignInXamlToolkit
- Tuân thủ Material Design guidelines
- Consistent spacing và typography
- Accessible color contrast

### Vietnamese Localization
- Sử dụng tiếng Việt có dấu đúng chính tả
- Thuật ngữ xây dựng chuẩn
- Format số theo locale Việt Nam
- Date/time format dd/MM/yyyy

## 🏆 Recognition

Chúng tôi ghi nhận mọi đóng góp:
- Contributors được liệt kê trong README
- Active contributors có thể trở thành maintainers
- Outstanding contributions được featured

## 📞 Liên hệ

- **GitHub Issues**: Technical questions và bug reports
- **GitHub Discussions**: General discussions và feature ideas
- **Email**: maintainers@constructionestimator.vn

## 📄 License

Bằng việc đóng góp, bạn đồng ý rằng contributions sẽ được licensed dưới MIT License.