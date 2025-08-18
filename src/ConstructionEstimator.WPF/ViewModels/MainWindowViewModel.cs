using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows.Input;
using ConstructionEstimator.Core.Models;
using ConstructionEstimator.Core.Services;

namespace ConstructionEstimator.WPF.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject
    {
        private readonly IProjectService _projectService;
        private readonly IMaterialService _materialService;
        private readonly IEstimateService _estimateService;

        [ObservableProperty]
        private string statusMessage = "Sẵn sàng";

        [ObservableProperty]
        private string currentProjectName = "Chưa có dự án";

        [ObservableProperty]
        private object? currentView;

        [ObservableProperty]
        private ObservableCollection<ProjectTreeNode> projectTree = new();

        public MainWindowViewModel(
            IProjectService projectService,
            IMaterialService materialService,
            IEstimateService estimateService)
        {
            _projectService = projectService;
            _materialService = materialService;
            _estimateService = estimateService;

            InitializeCommands();
        }

        private void InitializeCommands()
        {
            StatusMessage = "Phần mềm dự toán xây dựng khởi động thành công";
        }

        [RelayCommand]
        private void NewProject()
        {
            StatusMessage = "Tạo dự án mới...";
            // TODO: Implement new project functionality
        }

        [RelayCommand]
        private void OpenProject()
        {
            StatusMessage = "Mở dự án...";
            // TODO: Implement open project functionality
        }

        [RelayCommand]
        private void SaveProject()
        {
            StatusMessage = "Lưu dự án...";
            // TODO: Implement save project functionality
        }

        [RelayCommand]
        private void SaveAsProject()
        {
            StatusMessage = "Lưu dự án thành...";
            // TODO: Implement save as project functionality
        }

        [RelayCommand]
        private void ExportExcel()
        {
            StatusMessage = "Xuất Excel...";
            // TODO: Implement Excel export functionality
        }

        [RelayCommand]
        private void ExportPdf()
        {
            StatusMessage = "Xuất PDF...";
            // TODO: Implement PDF export functionality
        }

        [RelayCommand]
        private void Exit()
        {
            System.Windows.Application.Current.Shutdown();
        }

        [RelayCommand]
        private void Undo()
        {
            StatusMessage = "Hoàn tác...";
            // TODO: Implement undo functionality
        }

        [RelayCommand]
        private void Redo()
        {
            StatusMessage = "Làm lại...";
            // TODO: Implement redo functionality
        }

        [RelayCommand]
        private void ManageMaterials()
        {
            StatusMessage = "Quản lý vật liệu...";
            // TODO: Implement materials management
        }

        [RelayCommand]
        private void ManageStandards()
        {
            StatusMessage = "Quản lý định mức...";
            // TODO: Implement standards management
        }

        [RelayCommand]
        private void ManagePrices()
        {
            StatusMessage = "Quản lý đơn giá...";
            // TODO: Implement price management
        }

        [RelayCommand]
        private void SummaryReport()
        {
            StatusMessage = "Tạo báo cáo tổng hợp...";
            // TODO: Implement summary report
        }

        [RelayCommand]
        private void DetailedReport()
        {
            StatusMessage = "Tạo báo cáo chi tiết...";
            // TODO: Implement detailed report
        }

        [RelayCommand]
        private void Help()
        {
            StatusMessage = "Mở trợ giúp...";
            // TODO: Implement help functionality
        }

        [RelayCommand]
        private void About()
        {
            StatusMessage = "Thông tin về phần mềm...";
            // TODO: Implement about dialog
        }

        [RelayCommand]
        private void FileMenuOpened()
        {
            // Update file menu state when opened
        }
    }

    public class ProjectTreeNode
    {
        public string Name { get; set; } = string.Empty;
        public string IconKind { get; set; } = "Folder";
        public ObservableCollection<ProjectTreeNode> Children { get; set; } = new();
    }
}