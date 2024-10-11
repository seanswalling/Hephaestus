using System;
using System.IO;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Hephaestus.Avalonia.Models;
using Hephaestus.Avalonia.ViewModels;
using Hephaestus.Avalonia.Views;

namespace Hephaestus.Avalonia
{
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

                var applicationRoot = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Hephaestus");
                var coreApp = new Core.Application.Application(applicationRoot);
                coreApp.SetRepository(new Core.Application.KnownRepository("Mercury", @"c:\source\mercury"));
                coreApp.LoadRepository();
                var uiAdapter = new RepositoryProviderUIAdapter(coreApp);

                desktop.MainWindow = new MainWindow
                {
                    DataContext = new MainWindowViewModel(uiAdapter),
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}