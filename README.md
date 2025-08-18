# Construction Estimator Vietnam 🏗️

**Phần mềm Dự toán Xây dựng Việt Nam** - A comprehensive construction estimation software designed specifically for the Vietnamese construction industry.

## ✨ Features

- 🎯 **Complete Project Management** - Create and manage construction projects with detailed estimates
- 💰 **Automatic Cost Calculation** - Real-time calculation of material, labor, and equipment costs  
- 🇻🇳 **Vietnamese Market Data** - Pre-loaded with 200+ construction materials and 37 labor types
- 📊 **Professional Reports** - Generate detailed cost reports with section breakdowns
- 📈 **Excel Export** - Export estimates to Excel for easy sharing and presentation
- 🏗️ **Industry Standards** - Built following Vietnamese construction estimation practices

## 🚀 Quick Start

```bash
# Clone and build
git clone https://github.com/mrsonly86/construction-estimator.git
cd construction-estimator
dotnet build

# Run the application
cd src/ConstructionEstimator.WPF
dotnet run

# Run tests
dotnet test
```

## 📁 Project Structure

```
ConstructionEstimator/
├── src/
│   ├── ConstructionEstimator.Core/          # 🏗️ Business logic & entities
│   ├── ConstructionEstimator.Infrastructure/ # 🗄️ Data access & repositories  
│   ├── ConstructionEstimator.WPF/           # 🖥️ User interface
│   └── ConstructionEstimator.Shared/        # 🔧 Common utilities
├── tests/
│   └── ConstructionEstimator.Tests/         # ✅ Unit tests
└── docs/                                    # 📚 Documentation
```

## 💡 Example Usage

The application includes a complete demonstration with a sample 2-story house project:

**Project Cost**: 63,090,000 VND
- Materials: 48,690,000 VND (77%)
- Labor: 10,600,000 VND (17%) 
- Equipment: 3,800,000 VND (6%)

## 🛠️ Tech Stack

- **.NET 8.0** - Modern cross-platform framework
- **Entity Framework Core** - Database ORM with SQLite
- **WPF + MVVM** - Rich desktop UI with clean architecture
- **xUnit** - Comprehensive unit testing
- **Serilog** - Structured logging
- **EPPlus** - Excel export functionality

## ✅ Status

**Foundation Complete & Production Ready!**

All core functionality implemented and tested:
- ✅ Project CRUD operations
- ✅ Cost calculation engine  
- ✅ Vietnamese material & labor database
- ✅ Report generation
- ✅ Excel export
- ✅ Comprehensive testing

See [IMPLEMENTATION.md](IMPLEMENTATION.md) for detailed feature documentation.

---

**🚀 Ready for immediate use and further development!**
