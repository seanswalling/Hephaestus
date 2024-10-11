using System.Linq;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using Hephaestus.Avalonia.Models;

namespace Hephaestus.Avalonia.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        public SelectedMode SelectedMode { get; set; }
        public static SelectedMode[] AvailableModes => [SelectedMode.Projects, SelectedMode.Packages];

        public FileGridViewModel FileGrid { get; set; }
        public PreviewViewModel Preview { get; set; }
        //public ProjectViewModel[] Projects { get; set; }
        //public PackageViewModel[] Packages { get; set; }
        [ObservableProperty]
        private ViewModelBase _selectedView;
        public ProjectsViewModel ProjectsViewModel { get; set; }

        private readonly RepositoryProviderUIAdapter _adapter;
        private readonly Dispatcher _currentDispatcher;

        public MainWindowViewModel() { }

        public MainWindowViewModel(RepositoryProviderUIAdapter adapter)
        {
            _adapter = adapter;
            _currentDispatcher = Dispatcher.UIThread;

            _adapter.Subscribe(() =>
            {
                _currentDispatcher.Invoke(() =>
                {
                    FileGrid = new FileGridViewModel(_adapter);
                    Preview = new PreviewViewModel();
                    OnPropertyChanged(nameof(FileGrid));
                    OnPropertyChanged(nameof(Preview));
                    WireFileGrid(FileGrid);
                });
            });

            //FileGrid = new FileGridViewModel(_adapter);
            Preview = new PreviewViewModel();
            ProjectsViewModel = new ProjectsViewModel(_adapter);
            //Projects = _adapter.GetProjects();
            SelectedView = ProjectsViewModel;
            //WireFileGrid(FileGrid);
        }

        public void WireFileGrid(FileGridViewModel fileGrid)
        {
            fileGrid.ProjectGrid.PropertyChanged += (obj, args) =>
            {
                if (obj is not ProjectDataGridViewModel projectGridViewModel) return;

                if (args.PropertyName == nameof(ProjectDataGridViewModel.SelectedProject))
                {
                    Preview.File.Content = projectGridViewModel.SelectedProject == null
                        ? string.Empty
                        : _adapter.GetFileContent(projectGridViewModel.SelectedProject.Path);
                    Preview.Usages.Projects = projectGridViewModel.SelectedProject == null ? []
                        : projectGridViewModel.SelectedProject.Usages.Select(_adapter.GetProject).ToArray();
                    Preview.References.Projects = projectGridViewModel.SelectedProject == null ? []
                        : projectGridViewModel.SelectedProject.ProjectReferences.Select(_adapter.GetProject).ToArray();
                }
            };
        }



    }

    public enum SelectedMode
    {
        Projects,
        Packages
    }
}
