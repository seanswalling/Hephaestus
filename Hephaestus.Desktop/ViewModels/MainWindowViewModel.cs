using System;
using System.Linq;
using System.Windows.Threading;
using Hephaestus.Desktop.Models;

namespace Hephaestus.Desktop.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public FileGridViewModel FileGrid { get; set; }
        public PreviewViewModel Preview { get; set; }

        private readonly RepositoryProviderUIAdapter _adapter;
        private readonly Dispatcher _currentDispatcher;
        public MainWindowViewModel(RepositoryProviderUIAdapter adapter)
        {
            _adapter = adapter;
            _currentDispatcher = Dispatcher.CurrentDispatcher;

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

            FileGrid = new FileGridViewModel(_adapter);
            Preview = new PreviewViewModel();
            WireFileGrid(FileGrid);





            //FileGrid.SolutionGrid.PropertyChanged += (obj, args) =>
            //{
            //    if (obj is not SolutionGridViewModel solutionGridViewModel) return;

            //    if (args.PropertyName == nameof(SolutionGridViewModel.SelectedSolution))
            //    {
            //        Preview.File.Content = solutionGridViewModel.SelectedSolution == null ? string.Empty :
            //            CodeRepositoryAdapter.Instance.GetFileContent(solutionGridViewModel.SelectedSolution.Path);
            //        Preview.Usages.Projects = Array.Empty<ProjectViewModel>();
            //        Preview.References.Projects = Array.Empty<ProjectViewModel>();
            //    }
            //};



            //FileGrid.ProjectGrid.PropertyChanged += (obj, args) =>
            //{
            //    if (obj is not ProjectGridViewModel projectGridViewModel) return;

            //    if (args.PropertyName == nameof(ProjectGridViewModel.SelectedProject))
            //    {
            //        Preview.File.Content = projectGridViewModel.SelectedProject == null
            //            ? string.Empty
            //            : CodeRepositoryAdapter.Instance.GetFileContent(projectGridViewModel.SelectedProject.Path);
            //        Preview.Usages.Projects = projectGridViewModel.SelectedProject == null ?
            //            Array.Empty<ProjectViewModel>()
            //            : projectGridViewModel.SelectedProject.Usages.Select(CodeRepositoryAdapter.Instance.GetProject).ToArray();
            //        Preview.References.Projects = projectGridViewModel.SelectedProject == null ?
            //            Array.Empty<ProjectViewModel>()
            //            : projectGridViewModel.SelectedProject.ProjectReferences.Select(CodeRepositoryAdapter.Instance.GetProject).ToArray();
            //    }
            //};
        }

        public void WireFileGrid(FileGridViewModel fileGrid)
        {
            fileGrid.ProjectGrid.PropertyChanged += (obj, args) =>
            {
                if (obj is not ProjectGridViewModel projectGridViewModel) return;

                if (args.PropertyName == nameof(ProjectGridViewModel.SelectedProject))
                {
                    Preview.File.Content = projectGridViewModel.SelectedProject == null
                        ? string.Empty
                        : _adapter.GetFileContent(projectGridViewModel.SelectedProject.Path);
                    Preview.Usages.Projects = projectGridViewModel.SelectedProject == null ?
                        Array.Empty<ProjectViewModel>()
                        : projectGridViewModel.SelectedProject.Usages.Select(_adapter.GetProject).ToArray();
                    Preview.References.Projects = projectGridViewModel.SelectedProject == null ?
                        Array.Empty<ProjectViewModel>()
                        : projectGridViewModel.SelectedProject.ProjectReferences.Select(_adapter.GetProject).ToArray();
                }
            };
        }
    }
}
