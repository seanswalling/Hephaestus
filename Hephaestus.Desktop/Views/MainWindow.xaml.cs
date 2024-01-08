using System.Windows;
using Hephaestus.Desktop.Models;
using Hephaestus.Desktop.ViewModels;

namespace Hephaestus.Desktop.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(RepositoryProviderUIAdapter adapter)
        {
            DataContext = new MainWindowViewModel(adapter);
            InitializeComponent();
        }
    }
}
