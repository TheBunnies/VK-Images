using System;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using VK_Images.API;
using VK_Images.ViewModels;

namespace VK_Images
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            IServiceProvider serviceProvider = CreateServiceProvider();

            Window window = serviceProvider.GetRequiredService<MainWindow>();
            window.Show();

            base.OnStartup(e);
        }

        public IServiceProvider CreateServiceProvider()
        {
            IServiceCollection services = new ServiceCollection();

            services.AddSingleton<Album>();
            services.AddSingleton<IDataService, Wall>();

            services.AddScoped<ShellViewModel>();
            services.AddScoped<MainWindow>(s => new MainWindow(s.GetRequiredService<ShellViewModel>()));

            return services.BuildServiceProvider();
        }
    }
}
