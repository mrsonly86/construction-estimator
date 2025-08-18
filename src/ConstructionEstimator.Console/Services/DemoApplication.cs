using ConstructionEstimator.Services.Interfaces;
using ConstructionEstimator.Core.Models;
using ConstructionEstimator.Core.Enums;

namespace ConstructionEstimator.Console.Services;

public class DemoApplication
{
    private readonly IProjectService _projectService;
    private readonly ICalculationService _calculationService;
    private readonly IMaterialService _materialService;

    public DemoApplication(
        IProjectService projectService, 
        ICalculationService calculationService,
        IMaterialService materialService)
    {
        _projectService = projectService;
        _calculationService = calculationService;
        _materialService = materialService;
    }

    public async Task RunDemoAsync()
    {
        System.Console.WriteLine("🚀 Starting Construction Estimator Demo...");
        System.Console.WriteLine();

        // 1. Show existing materials
        await ShowMaterialsAsync();

        // 2. Create a sample project
        await CreateSampleProjectAsync();

        // 3. Show all projects
        await ShowProjectsAsync();

        // 4. Demonstrate calculations
        await DemonstrateCalculationsAsync();

        System.Console.WriteLine();
        System.Console.WriteLine("✅ Demo completed successfully!");
        System.Console.WriteLine("📂 Database file: ConstructionEstimator.db");
        System.Console.WriteLine("📝 Log files: logs/construction-estimator-*.log");
    }

    private async Task ShowMaterialsAsync()
    {
        System.Console.WriteLine("📦 MATERIALS DATABASE:");
        System.Console.WriteLine("".PadRight(60, '-'));

        try
        {
            var materials = await _materialService.GetAllMaterialsAsync();
            
            if (!materials.Any())
            {
                System.Console.WriteLine("❌ No materials found");
                return;
            }

            System.Console.WriteLine($"{"Code",-10} {"Name",-25} {"Category",-15} {"Price",-15} {"Unit",-10}");
            System.Console.WriteLine("".PadRight(85, '-'));
            
            foreach (var material in materials)
            {
                System.Console.WriteLine($"{material.Code,-10} {material.Name,-25} {material.Category,-15} {material.UnitPrice,12:N0} {material.UnitOfMeasure,-10}");
            }
        }
        catch (Exception ex)
        {
            System.Console.WriteLine($"❌ Error loading materials: {ex.Message}");
        }

        System.Console.WriteLine();
    }

    private async Task CreateSampleProjectAsync()
    {
        System.Console.WriteLine("🏗️ CREATING SAMPLE PROJECT:");
        System.Console.WriteLine("".PadRight(60, '-'));

        var sampleProject = new ProjectDto
        {
            Name = "Nhà ở gia đình 2 tầng - Demo",
            Description = "Xây dựng nhà ở gia đình 2 tầng, diện tích 8x12m (Dự án mẫu)",
            ClientName = "Gia đình Nguyễn Văn A",
            ClientAddress = "123 Đường ABC, Quận 1, TP.HCM",
            ClientPhone = "0901234567",
            ClientEmail = "nguyenvana@email.com",
            Location = "TP. Hồ Chí Minh",
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddMonths(6),
            Currency = "VND",
            ProfitPercentage = 12,
            ContingencyPercentage = 8,
            Status = ProjectStatus.Draft,
            EstimateItems = new List<EstimateItemDto>
            {
                new EstimateItemDto
                {
                    Name = "Xây tường gạch ống",
                    Code = "XY001",
                    Type = EstimateItemType.Material,
                    UnitOfMeasure = UnitOfMeasure.SquareMeter,
                    Quantity = 120.5m,
                    UnitPrice = 85000,
                    TotalPrice = 120.5m * 85000,
                    Description = "Xây tường gạch ống 4 lỗ, dày 10cm, bao gồm vữa xây",
                    SortOrder = 1
                },
                new EstimateItemDto
                {
                    Name = "Đổ bê tông sàn tầng",
                    Code = "BE001", 
                    Type = EstimateItemType.Material,
                    UnitOfMeasure = UnitOfMeasure.CubicMeter,
                    Quantity = 15.8m,
                    UnitPrice = 2800000,
                    TotalPrice = 15.8m * 2800000,
                    Description = "Đổ bê tông sàn tầng 1 và tầng 2, cấp độ bền B20",
                    SortOrder = 2
                },
                new EstimateItemDto
                {
                    Name = "Lắp đặt cốt thép",
                    Code = "TH001",
                    Type = EstimateItemType.Material,
                    UnitOfMeasure = UnitOfMeasure.Ton,
                    Quantity = 2.3m,
                    UnitPrice = 18500000,
                    TotalPrice = 2.3m * 18500000,
                    Description = "Cốt thép CB300-V, đã bao gồm gia công và lắp đặt",
                    SortOrder = 3
                },
                new EstimateItemDto
                {
                    Name = "Công nhân xây dựng",
                    Code = "CN001",
                    Type = EstimateItemType.Labor,
                    UnitOfMeasure = UnitOfMeasure.Day,
                    Quantity = 45,
                    UnitPrice = 350000,
                    TotalPrice = 45 * 350000,
                    Description = "Công nhân xây dựng cấp độ 3/7, làm việc 8 giờ/ngày",
                    SortOrder = 4
                },
                new EstimateItemDto
                {
                    Name = "Thợ hàn chuyên nghiệp",
                    Code = "TH002",
                    Type = EstimateItemType.Labor,
                    UnitOfMeasure = UnitOfMeasure.Day,
                    Quantity = 8,
                    UnitPrice = 500000,
                    TotalPrice = 8 * 500000,
                    Description = "Thợ hàn cấp 5/7 cho việc hàn cốt thép",
                    SortOrder = 5
                },
                new EstimateItemDto
                {
                    Name = "Thuê cần cẩu",
                    Code = "CC001",
                    Type = EstimateItemType.Equipment,
                    UnitOfMeasure = UnitOfMeasure.Day,
                    Quantity = 3,
                    UnitPrice = 1500000,
                    TotalPrice = 3 * 1500000,
                    Description = "Thuê cần cẩu 8 tấn, bao gồm lái xe và nhiên liệu",
                    SortOrder = 6
                },
                new EstimateItemDto
                {
                    Name = "Máy trộn bê tông",
                    Code = "BT001",
                    Type = EstimateItemType.Equipment,
                    UnitOfMeasure = UnitOfMeasure.Day,
                    Quantity = 5,
                    UnitPrice = 800000,
                    TotalPrice = 5 * 800000,
                    Description = "Thuê máy trộn bê tông 350L/mẻ",
                    SortOrder = 7
                }
            }
        };

        try
        {
            var createdProject = await _projectService.CreateProjectAsync(sampleProject);
            
            System.Console.WriteLine($"✅ Project created successfully!");
            System.Console.WriteLine($"   📋 Project ID: {createdProject.Id}");
            System.Console.WriteLine($"   🏠 Name: {createdProject.Name}");
            System.Console.WriteLine($"   👤 Client: {createdProject.ClientName}");
            System.Console.WriteLine($"   📍 Location: {createdProject.Location}");
            System.Console.WriteLine($"   📅 Start Date: {createdProject.StartDate:dd/MM/yyyy}");
            System.Console.WriteLine($"   📊 Items: {createdProject.EstimateItems.Count} estimate items");
        }
        catch (Exception ex)
        {
            System.Console.WriteLine($"❌ Error creating project: {ex.Message}");
        }

        System.Console.WriteLine();
    }

    private async Task ShowProjectsAsync()
    {
        System.Console.WriteLine("📊 ALL PROJECTS:");
        System.Console.WriteLine("".PadRight(60, '-'));

        try
        {
            var projects = await _projectService.GetAllProjectsAsync();
            
            if (!projects.Any())
            {
                System.Console.WriteLine("❌ No projects found");
                return;
            }

            foreach (var project in projects)
            {
                System.Console.WriteLine($"🏗️ Project #{project.Id}: {project.Name}");
                System.Console.WriteLine($"   Client: {project.ClientName}");
                System.Console.WriteLine($"   Location: {project.Location}");
                System.Console.WriteLine($"   Status: {project.Status}");
                System.Console.WriteLine($"   Items: {project.EstimateItems.Count}");
                System.Console.WriteLine($"   Created: {project.CreatedAt:dd/MM/yyyy HH:mm}");
                System.Console.WriteLine();
            }
        }
        catch (Exception ex)
        {
            System.Console.WriteLine($"❌ Error loading projects: {ex.Message}");
        }
    }

    private async Task DemonstrateCalculationsAsync()
    {
        System.Console.WriteLine("🧮 COST CALCULATION DEMO:");
        System.Console.WriteLine("".PadRight(60, '-'));

        try
        {
            var projects = await _projectService.GetAllProjectsAsync();
            var project = projects.FirstOrDefault();
            
            if (project == null)
            {
                System.Console.WriteLine("❌ No project found for calculation");
                return;
            }

            System.Console.WriteLine($"📋 Calculating costs for: {project.Name}");
            System.Console.WriteLine();

            // Calculate by category
            var materialCost = project.EstimateItems
                .Where(i => i.Type == EstimateItemType.Material)
                .Sum(i => i.TotalPrice);
            
            var laborCost = project.EstimateItems
                .Where(i => i.Type == EstimateItemType.Labor)
                .Sum(i => i.TotalPrice);
            
            var equipmentCost = project.EstimateItems
                .Where(i => i.Type == EstimateItemType.Equipment)
                .Sum(i => i.TotalPrice);

            // Use calculation service
            var breakdown = _calculationService.CalculateCostBreakdown(
                materialCost, laborCost, equipmentCost,
                project.ProfitPercentage, project.ContingencyPercentage);

            // Display results
            System.Console.WriteLine("💰 COST BREAKDOWN:");
            System.Console.WriteLine($"   Materials:      {breakdown["MaterialCost"],15:N0} VND");
            System.Console.WriteLine($"   Labor:          {breakdown["LaborCost"],15:N0} VND");
            System.Console.WriteLine($"   Equipment:      {breakdown["EquipmentCost"],15:N0} VND");
            System.Console.WriteLine("   " + "".PadRight(35, '-'));
            System.Console.WriteLine($"   Subtotal:       {breakdown["Subtotal"],15:N0} VND");
            System.Console.WriteLine($"   Overhead (15%): {breakdown["OverheadCost"],15:N0} VND");
            System.Console.WriteLine($"   Profit ({project.ProfitPercentage}%):     {breakdown["ProfitAmount"],15:N0} VND");
            System.Console.WriteLine($"   Contingency ({project.ContingencyPercentage}%): {breakdown["ContingencyAmount"],15:N0} VND");
            System.Console.WriteLine("   " + "".PadRight(35, '='));
            System.Console.WriteLine($"   TOTAL:          {breakdown["TotalCost"],15:N0} VND");
            
            System.Console.WriteLine();
            System.Console.WriteLine($"💡 Project Items Breakdown:");
            foreach (var item in project.EstimateItems.OrderBy(i => i.SortOrder))
            {
                System.Console.WriteLine($"   {item.Code}: {item.Name}");
                System.Console.WriteLine($"      {item.Quantity:N2} {item.UnitOfMeasure} × {item.UnitPrice:N0} = {item.TotalPrice:N0} VND");
            }

        }
        catch (Exception ex)
        {
            System.Console.WriteLine($"❌ Error in calculations: {ex.Message}");
        }

        System.Console.WriteLine();
    }
}