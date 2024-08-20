using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using H2MServerBrowser.Services;
using H2MServerBrowser.ViewModels;
using H2MServerBrowser.Views;

namespace H2MServerBrowser;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Line below is needed to remove Avalonia data validation.
            // Without this line you will get duplicate validations from both Avalonia and CT
            BindingPlugins.DataValidators.RemoveAt(0);
            var serverService = new ServerService();
            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainWindowViewModel(serverService),
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}