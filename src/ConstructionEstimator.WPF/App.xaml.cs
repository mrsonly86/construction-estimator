using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Windows;
using ConstructionEstimator.WPF.ViewModels;
using ConstructionEstimator.WPF.Views;
using ConstructionEstimator.Core.Services;
using ConstructionEstimator.Data;

namespace ConstructionEstimator.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IHost _host;

        public App()
        {
            _host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    // Register services
                    services.AddSingleton<MainWindow>();
                    services.AddSingleton<MainWindowViewModel>();
                    
                    // Register core services
                    services.AddSingleton<IProjectService, ProjectService>();
                    services.AddSingleton<IMaterialService, MaterialService>();
                    services.AddSingleton<IEstimateService, EstimateService>();
                    
                    // Register data services
                    services.AddSingleton<IDataContext, SqliteDataContext>();
                })
                .Build();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            await _host.StartAsync();

            var mainWindow = _host.Services.GetRequiredService<MainWindow>();
            mainWindow.Show();

            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            using (_host)
            {
                await _host.StopAsync();
            }

            base.OnExit(e);
        }
    }
}