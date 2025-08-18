using Spectre.Console;
using ConstructionEstimator.Services.Interfaces;
using ConstructionEstimator.Core.Models;
using ConstructionEstimator.Core.Enums;

namespace ConstructionEstimator.Console.Services;

public class ConsoleApplication
{
    private readonly IProjectService _projectService;
    private readonly ICalculationService _calculationService;

    public ConsoleApplication(IProjectService projectService, ICalculationService calculationService)
    {
        _projectService = projectService;
        _calculationService = calculationService;
    }

    public async Task RunAsync()
    {
        while (true)
        {
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Chọn chức năng:")
                    .PageSize(10)
                    .AddChoices(new[] {
                        "Tạo dự án mới",
                        "Xem danh sách dự án", 
                        "Tạo dự án mẫu",
                        "Hiển thị thống kê",
                        "Thoát"
                    }));

            switch (choice)
            {
                case "Tạo dự án mới":
                    await CreateNewProjectAsync();
                    break;
                case "Xem danh sách dự án":
                    await ShowProjectListAsync();
                    break;
                case "Tạo dự án mẫu":
                    await CreateSampleProjectAsync();
                    break;
                case "Hiển thị thống kê":
                    await ShowStatisticsAsync();
                    break;
                case "Thoát":
                    AnsiConsole.MarkupLine("[yellow]Tạm biệt![/]");
                    return;
            }

            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine("[dim]Nhấn Enter để tiếp tục...[/]");
            System.Console.ReadLine();
            AnsiConsole.Clear();
        }
    }

    private async Task CreateNewProjectAsync()
    {
        AnsiConsole.MarkupLine("[bold blue]Tạo dự án mới[/]");
        AnsiConsole.WriteLine();

        var projectName = AnsiConsole.Ask<string>("Tên dự án:");
        var clientName = AnsiConsole.Ask<string>("Tên khách hàng:");
        var location = AnsiConsole.Ask<string>("Địa điểm:");

        var project = new ProjectDto
        {
            Name = projectName,
            ClientName = clientName,
            Location = location,
            StartDate = DateTime.Now,
            Currency = "VND",
            ProfitPercentage = 10,
            ContingencyPercentage = 5,
            Status = ProjectStatus.Draft
        };

        try
        {
            var createdProject = await _projectService.CreateProjectAsync(project);
            
            AnsiConsole.MarkupLine($"[green]✓[/] Đã tạo dự án thành công với ID: {createdProject.Id}");
            
            // Show project details
            var table = new Table();
            table.AddColumn("Thuộc tính");
            table.AddColumn("Giá trị");
            
            table.AddRow("ID", createdProject.Id.ToString());
            table.AddRow("Tên dự án", createdProject.Name);
            table.AddRow("Khách hàng", createdProject.ClientName);
            table.AddRow("Địa điểm", createdProject.Location);
            table.AddRow("Ngày bắt đầu", createdProject.StartDate.ToString("dd/MM/yyyy"));
            table.AddRow("Trạng thái", createdProject.Status.ToString());
            
            AnsiConsole.Write(table);
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]✗[/] Lỗi tạo dự án: {ex.Message}");
        }
    }

    private async Task ShowProjectListAsync()
    {
        AnsiConsole.MarkupLine("[bold blue]Danh sách dự án[/]");
        AnsiConsole.WriteLine();

        try
        {
            var projects = await _projectService.GetAllProjectsAsync();
            
            if (!projects.Any())
            {
                AnsiConsole.MarkupLine("[yellow]Chưa có dự án nào[/]");
                return;
            }

            var table = new Table();
            table.AddColumn("ID");
            table.AddColumn("Tên dự án");
            table.AddColumn("Khách hàng");
            table.AddColumn("Địa điểm");
            table.AddColumn("Ngày tạo");
            table.AddColumn("Trạng thái");
            table.AddColumn("Tổng chi phí");

            foreach (var project in projects)
            {
                table.AddRow(
                    project.Id.ToString(),
                    project.Name,
                    project.ClientName,
                    project.Location,
                    project.CreatedAt.ToString("dd/MM/yyyy"),
                    GetStatusMarkup(project.Status),
                    $"{project.TotalCost:N0} {project.Currency}"
                );
            }

            AnsiConsole.Write(table);
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]✗[/] Lỗi tải danh sách dự án: {ex.Message}");
        }
    }

    private async Task CreateSampleProjectAsync()
    {
        AnsiConsole.MarkupLine("[bold blue]Tạo dự án mẫu[/]");
        AnsiConsole.WriteLine();

        var sampleProject = new ProjectDto
        {
            Name = "Nhà ở gia đình 2 tầng",
            Description = "Xây dựng nhà ở gia đình 2 tầng, diện tích 8x12m",
            ClientName = "Gia đình Nguyễn Văn A",
            ClientAddress = "123 Đường ABC, Quận 1, TP.HCM",
            ClientPhone = "0901234567",
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
                    Name = "Xây tường gạch",
                    Code = "XY001",
                    Type = EstimateItemType.Material,
                    UnitOfMeasure = UnitOfMeasure.SquareMeter,
                    Quantity = 120,
                    UnitPrice = 85000,
                    TotalPrice = 120 * 85000,
                    Description = "Xây tường gạch ống 4 lỗ, dày 10cm"
                },
                new EstimateItemDto
                {
                    Name = "Đổ bê tông sàn",
                    Code = "BE001", 
                    Type = EstimateItemType.Material,
                    UnitOfMeasure = UnitOfMeasure.CubicMeter,
                    Quantity = 15,
                    UnitPrice = 2800000,
                    TotalPrice = 15 * 2800000,
                    Description = "Đổ bê tông sàn tầng 1 và tầng 2"
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
                    Description = "Công nhân xây dựng làm việc 45 ngày"
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
                    Description = "Thuê cần cẩu 8 tấn trong 3 ngày"
                }
            }
        };

        try
        {
            var createdProject = await _projectService.CreateProjectAsync(sampleProject);
            
            AnsiConsole.MarkupLine($"[green]✓[/] Đã tạo dự án mẫu thành công!");

            // Calculate costs
            var materialCost = sampleProject.EstimateItems
                .Where(i => i.Type == EstimateItemType.Material)
                .Sum(i => i.TotalPrice);
            
            var laborCost = sampleProject.EstimateItems
                .Where(i => i.Type == EstimateItemType.Labor)
                .Sum(i => i.TotalPrice);
            
            var equipmentCost = sampleProject.EstimateItems
                .Where(i => i.Type == EstimateItemType.Equipment)
                .Sum(i => i.TotalPrice);

            var breakdown = _calculationService.CalculateCostBreakdown(
                materialCost, laborCost, equipmentCost,
                sampleProject.ProfitPercentage, sampleProject.ContingencyPercentage);

            // Show cost breakdown
            var tree = new Tree("📊 Phân tích chi phí dự án mẫu");
            
            var materialsNode = tree.AddNode($"💰 Vật liệu: [green]{breakdown["MaterialCost"]:N0} VND[/]");
            var laborNode = tree.AddNode($"👷 Nhân công: [blue]{breakdown["LaborCost"]:N0} VND[/]");
            var equipmentNode = tree.AddNode($"🏗️ Thiết bị: [yellow]{breakdown["EquipmentCost"]:N0} VND[/]");
            var overheadNode = tree.AddNode($"📈 Chi phí quản lý: [orange3]{breakdown["OverheadCost"]:N0} VND[/]");
            var profitNode = tree.AddNode($"💼 Lợi nhuận ({sampleProject.ProfitPercentage}%): [purple]{breakdown["ProfitAmount"]:N0} VND[/]");
            var contingencyNode = tree.AddNode($"⚠️ Dự phòng ({sampleProject.ContingencyPercentage}%): [red]{breakdown["ContingencyAmount"]:N0} VND[/]");
            var totalNode = tree.AddNode($"🎯 [bold green]Tổng cộng: {breakdown["TotalCost"]:N0} VND[/]");

            AnsiConsole.Write(tree);
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]✗[/] Lỗi tạo dự án mẫu: {ex.Message}");
        }
    }

    private async Task ShowStatisticsAsync()
    {
        AnsiConsole.MarkupLine("[bold blue]Thống kê hệ thống[/]");
        AnsiConsole.WriteLine();

        try
        {
            var projects = await _projectService.GetAllProjectsAsync();
            
            var totalProjects = projects.Count();
            var draftProjects = projects.Count(p => p.Status == ProjectStatus.Draft);
            var inProgressProjects = projects.Count(p => p.Status == ProjectStatus.InProgress);
            var completedProjects = projects.Count(p => p.Status == ProjectStatus.Completed);
            var totalValue = projects.Sum(p => p.TotalCost);

            var chart = new BarChart()
                .Width(60)
                .Label("[bold]Thống kê dự án theo trạng thái[/]")
                .CenterLabel();

            if (draftProjects > 0)
                chart.AddItem("Nháp", draftProjects, Color.Yellow);
            if (inProgressProjects > 0)
                chart.AddItem("Đang thực hiện", inProgressProjects, Color.Blue);
            if (completedProjects > 0)
                chart.AddItem("Hoàn thành", completedProjects, Color.Green);

            if (totalProjects > 0)
            {
                AnsiConsole.Write(chart);
                AnsiConsole.WriteLine();
            }

            var panel = new Panel(
                new Markup($"""
                📊 [bold]Tổng quan hệ thống[/]
                
                🗂️  Tổng số dự án: [yellow]{totalProjects}[/]
                💰 Tổng giá trị: [green]{totalValue:N0} VND[/]
                📋 Dự án nháp: [yellow]{draftProjects}[/]
                🚧 Đang thực hiện: [blue]{inProgressProjects}[/]
                ✅ Hoàn thành: [green]{completedProjects}[/]
                """))
            {
                Header = new PanelHeader("📈 Thống kê"),
                Border = BoxBorder.Rounded
            };

            AnsiConsole.Write(panel);
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]✗[/] Lỗi tải thống kê: {ex.Message}");
        }
    }

    private static string GetStatusMarkup(ProjectStatus status)
    {
        return status switch
        {
            ProjectStatus.Draft => "[yellow]Nháp[/]",
            ProjectStatus.InProgress => "[blue]Đang thực hiện[/]",
            ProjectStatus.Review => "[orange3]Đang duyệt[/]",
            ProjectStatus.Approved => "[purple]Đã duyệt[/]",
            ProjectStatus.Completed => "[green]Hoàn thành[/]",
            ProjectStatus.Cancelled => "[red]Đã hủy[/]",
            _ => status.ToString()
        };
    }
}