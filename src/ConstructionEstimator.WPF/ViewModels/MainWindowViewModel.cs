// MainWindowViewModel.cs - MVVM ViewModel for Main Window
global using System;
global using System.Threading.Tasks;
global using System.Windows.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ConstructionEstimator.Core.Entities;
using ConstructionEstimator.Core.Interfaces;

namespace ConstructionEstimator.WPF.ViewModels;

public class MainWindowViewModel : INotifyPropertyChanged
{
    private readonly IProjectService _projectService;
    private readonly IEstimationService _estimationService;
    private readonly IMaterialService _materialService;
    private readonly ILaborService _laborService;

    private Project? _selectedProject;
    private EstimateReport? _report;
    private string _statusMessage = "Sẵn sàng";

    public MainWindowViewModel(
        IProjectService projectService,
        IEstimationService estimationService,
        IMaterialService materialService,
        ILaborService laborService)
    {
        _projectService = projectService;
        _estimationService = estimationService;
        _materialService = materialService;
        _laborService = laborService;

        Projects = new ObservableCollection<Project>();
        EstimateItems = new ObservableCollection<EstimateItem>();

        InitializeCommands();
        LoadDataAsync();
    }

    public ObservableCollection<Project> Projects { get; }
    public ObservableCollection<EstimateItem> EstimateItems { get; }

    public Project? SelectedProject
    {
        get => _selectedProject;
        set
        {
            _selectedProject = value;
            OnPropertyChanged();
            LoadProjectItems();
        }
    }

    public EstimateReport? Report
    {
        get => _report;
        set
        {
            _report = value;
            OnPropertyChanged();
        }
    }

    public string StatusMessage
    {
        get => _statusMessage;
        set
        {
            _statusMessage = value;
            OnPropertyChanged();
        }
    }

    public string CurrentDateTime => DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

    // Commands
    public ICommand CreateProjectCommand { get; private set; } = null!;
    public ICommand OpenProjectCommand { get; private set; } = null!;
    public ICommand SaveProjectCommand { get; private set; } = null!;
    public ICommand ExitCommand { get; private set; } = null!;
    public ICommand AddSectionCommand { get; private set; } = null!;
    public ICommand AddItemCommand { get; private set; } = null!;
    public ICommand CalculateCommand { get; private set; } = null!;
    public ICommand ViewReportCommand { get; private set; } = null!;
    public ICommand ExportExcelCommand { get; private set; } = null!;
    public ICommand ExportPdfCommand { get; private set; } = null!;

    private void InitializeCommands()
    {
        CreateProjectCommand = new RelayCommand(async () => await CreateProjectAsync());
        OpenProjectCommand = new RelayCommand(async () => await OpenProjectAsync());
        SaveProjectCommand = new RelayCommand(async () => await SaveProjectAsync());
        ExitCommand = new RelayCommand(() => Environment.Exit(0));
        AddSectionCommand = new RelayCommand(async () => await AddSectionAsync());
        AddItemCommand = new RelayCommand(async () => await AddItemAsync());
        CalculateCommand = new RelayCommand(async () => await CalculateAsync());
        ViewReportCommand = new RelayCommand(async () => await ViewReportAsync());
        ExportExcelCommand = new RelayCommand(async () => await ExportExcelAsync());
        ExportPdfCommand = new RelayCommand(async () => await ExportPdfAsync());
    }

    private async void LoadDataAsync()
    {
        try
        {
            StatusMessage = "Đang tải dữ liệu...";
            var projects = await _projectService.GetAllProjectsAsync();
            
            Projects.Clear();
            foreach (var project in projects)
            {
                Projects.Add(project);
            }
            
            StatusMessage = "Sẵn sàng";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Lỗi: {ex.Message}";
        }
    }

    private void LoadProjectItems()
    {
        EstimateItems.Clear();
        if (SelectedProject?.Sections != null)
        {
            foreach (var section in SelectedProject.Sections)
            {
                foreach (var item in section.Items)
                {
                    EstimateItems.Add(item);
                }
            }
        }
    }

    private async Task CreateProjectAsync()
    {
        try
        {
            var project = new Project
            {
                Name = "Dự án mới",
                Description = "Mô tả dự án",
                Client = "Khách hàng",
                Location = "Địa điểm",
                Status = Core.Enums.ProjectStatus.Draft
            };

            var createdProject = await _projectService.CreateProjectAsync(project);
            Projects.Add(createdProject);
            SelectedProject = createdProject;
            StatusMessage = "Đã tạo dự án mới";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Lỗi tạo dự án: {ex.Message}";
        }
    }

    private async Task OpenProjectAsync()
    {
        // Implementation for opening project dialog
        StatusMessage = "Chức năng mở dự án";
        await Task.CompletedTask;
    }

    private async Task SaveProjectAsync()
    {
        if (SelectedProject == null) return;

        try
        {
            await _projectService.UpdateProjectAsync(SelectedProject);
            StatusMessage = "Đã lưu dự án";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Lỗi lưu dự án: {ex.Message}";
        }
    }

    private async Task AddSectionAsync()
    {
        if (SelectedProject == null) return;

        try
        {
            var section = new EstimateSection
            {
                Name = "Hạng mục mới",
                Code = "HM" + (SelectedProject.Sections.Count + 1).ToString("D3"),
                ProjectId = SelectedProject.Id
            };

            SelectedProject.Sections.Add(section);
            StatusMessage = "Đã thêm hạng mục mới";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Lỗi thêm hạng mục: {ex.Message}";
        }

        await Task.CompletedTask;
    }

    private async Task AddItemAsync()
    {
        // Implementation for adding estimate item
        StatusMessage = "Chức năng thêm công việc";
        await Task.CompletedTask;
    }

    private async Task CalculateAsync()
    {
        if (SelectedProject == null) return;

        try
        {
            var totalCost = _estimationService.CalculateProjectCost(SelectedProject);
            SelectedProject.TotalEstimatedCost = totalCost;
            StatusMessage = $"Đã tính toán: {totalCost:N0} VND";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Lỗi tính toán: {ex.Message}";
        }

        await Task.CompletedTask;
    }

    private async Task ViewReportAsync()
    {
        if (SelectedProject == null) return;

        try
        {
            Report = await _estimationService.GenerateReportAsync(SelectedProject);
            StatusMessage = "Đã tạo báo cáo";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Lỗi tạo báo cáo: {ex.Message}";
        }
    }

    private async Task ExportExcelAsync()
    {
        StatusMessage = "Chức năng xuất Excel";
        await Task.CompletedTask;
    }

    private async Task ExportPdfAsync()
    {
        StatusMessage = "Chức năng xuất PDF";
        await Task.CompletedTask;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

// Simple RelayCommand implementation
public class RelayCommand : ICommand
{
    private readonly Func<Task> _execute;
    private readonly Func<bool>? _canExecute;

    public RelayCommand(Func<Task> execute, Func<bool>? canExecute = null)
    {
        _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        _canExecute = canExecute;
    }

    public RelayCommand(Action execute, Func<bool>? canExecute = null)
        : this(() => { execute(); return Task.CompletedTask; }, canExecute)
    {
    }

    public event EventHandler? CanExecuteChanged
    {
        add { }
        remove { }
    }

    public bool CanExecute(object? parameter)
    {
        return _canExecute?.Invoke() ?? true;
    }

    public async void Execute(object? parameter)
    {
        await _execute();
    }
}