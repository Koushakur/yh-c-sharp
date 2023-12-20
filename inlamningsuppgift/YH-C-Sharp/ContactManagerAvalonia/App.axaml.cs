using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;

using ContactManagerAvalonia.ViewModels;
using ContactManagerAvalonia.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SharedLogic.Services;

namespace ContactManagerAvalonia;

public partial class App : Application
{
    private static IHost? builder;

    public App() {
        builder = Host.CreateDefaultBuilder()
           .ConfigureServices(services => {
               services.AddSingleton<ContactService>();
               services.AddSingleton<MainView>();
               services.AddSingleton<MainWindow>();
               services.AddSingleton<MainViewModel>();
           }).Build();
    }

    public override void Initialize() {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted() {
        // Line below is needed to remove Avalonia data validation.
        // Without this line you will get duplicate validations from both Avalonia and CT
        BindingPlugins.DataValidators.RemoveAt(0);

        builder!.Start();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop) {
            desktop.MainWindow = builder!.Services.GetRequiredService<MainWindow>();
            //builder!.Services.GetRequiredService<MainWindow>().Show();
            //desktop.MainWindow = new MainWindow {
            //    DataContext = new MainViewModel()
            //};
        } else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform) {
            singleViewPlatform.MainView = builder!.Services.GetRequiredService<MainView>();
        }

        base.OnFrameworkInitializationCompleted();
    }
}
