# Construction Estimator - Technical Architecture

## Overview
Construction Estimator is a modern WPF application built with .NET 8, following clean architecture principles and MVVM pattern.

## Architecture Layers

### 1. Presentation Layer (ConstructionEstimator.WPF)
- **Technology**: WPF with Material Design
- **Pattern**: MVVM with CommunityToolkit.Mvvm
- **Responsibilities**:
  - User interface and user experience
  - View models and data binding
  - Command handling
  - Navigation

### 2. Business Logic Layer (ConstructionEstimator.Core)
- **Responsibilities**:
  - Domain models and entities
  - Business rules and validation
  - Service interfaces
  - Domain services

### 3. Data Access Layer (ConstructionEstimator.Data)
- **Technology**: Entity Framework Core with SQLite
- **Responsibilities**:
  - Database context and configurations
  - Repository implementations
  - Data migrations
  - Database seeding

### 4. Reporting Layer (ConstructionEstimator.Reports)
- **Responsibilities**:
  - Report generation
  - Export to Excel, PDF, Word
  - Template management
  - Chart and visualization

### 5. Testing Layer (ConstructionEstimator.Tests)
- **Technology**: xUnit
- **Responsibilities**:
  - Unit tests
  - Integration tests
  - Test data and fixtures

## Domain Models

### Core Entities
1. **Project** - Construction projects
2. **Material** - Construction materials with pricing
3. **Labor** - Labor types and costs
4. **Equipment** - Construction equipment and rental costs
5. **EstimateItem** - Individual work items in estimates
6. **Standard** - Construction standards and norms

### Junction Tables
- **EstimateItemMaterial** - Materials used in estimate items
- **EstimateItemLabor** - Labor required for estimate items
- **EstimateItemEquipment** - Equipment needed for estimate items
- **StandardMaterial/Labor/Equipment** - Standards compositions

## Database Design

### Key Features
- SQLite for local storage
- Entity Framework Core for ORM
- Code-first migrations
- Support for regional pricing
- Historical data tracking

### Performance Considerations
- Indexed foreign keys
- Optimized queries for large datasets
- Efficient pagination
- Caching strategies

## Design Patterns

### MVVM Pattern
- **Models**: Domain entities in Core layer
- **Views**: XAML files with Material Design
- **ViewModels**: Business logic and UI state management

### Dependency Injection
- Microsoft.Extensions.DependencyInjection
- Service registration in App.xaml.cs
- Interface-based dependency management

### Repository Pattern
- Abstraction over data access
- Unit of work implementation
- Testable data layer

## Technology Stack

### Frontend
- **WPF**: Windows Presentation Foundation
- **Material Design**: MaterialDesignInXamlToolkit
- **MVVM**: CommunityToolkit.Mvvm
- **Dependency Injection**: Microsoft.Extensions.DependencyInjection

### Backend
- **.NET 8**: Latest .NET framework
- **Entity Framework Core**: ORM for data access
- **SQLite**: Embedded database
- **System.ComponentModel.DataAnnotations**: Model validation

### Development Tools
- **Visual Studio 2022**: Primary IDE
- **Git**: Version control
- **xUnit**: Testing framework
- **Entity Framework Tools**: Database migrations

## Security Considerations

### Data Protection
- Local database encryption
- Secure connection strings
- User authentication (future)
- Role-based access control (future)

### Input Validation
- Model validation attributes
- Client-side validation
- Business rule validation
- SQL injection prevention

## Performance Optimization

### Database
- Efficient queries with LINQ
- Proper indexing strategy
- Connection pooling
- Asynchronous operations

### UI
- Virtualization for large lists
- Background threading for long operations
- Responsive UI with async/await
- Memory management

## Localization

### Multi-language Support
- Resource files for strings
- Culture-specific formatting
- Vietnamese as primary language
- English as secondary language

## Future Enhancements

### Phase 2
- Advanced reporting features
- Excel integration improvements
- Cloud synchronization

### Phase 3
- BIM integration
- AI-powered cost prediction
- Mobile companion app
- Web-based version

### Phase 4
- Machine learning for price trends
- Advanced analytics dashboard
- Third-party integrations
- Enterprise features