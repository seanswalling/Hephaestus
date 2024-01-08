using System.Windows;
using Hephaestus.Desktop.Models;
using Hephaestus.Desktop.Views;

namespace Hephaestus.Desktop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        void App_Startup(object sender, StartupEventArgs e)
        {

            var coreApp = new Core.Application.Application();
            coreApp.SetRepository(new Core.Application.KnownRepository("Mercury", @"c:\source\mercury"));
            coreApp.LoadRepository();
            var uiAdapter = new RepositoryProviderUIAdapter(coreApp);

            new MainWindow(uiAdapter).Show();

        }
    }
}
