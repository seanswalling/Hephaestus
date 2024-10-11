using System.Linq;
using Hephaestus.Avalonia.Models;

namespace Hephaestus.Avalonia.ViewModels
{
    public class ProjectsViewModel : ViewModelBase
    {
        private RepositoryProviderUIAdapter _adapter;

        public ProjectDataGridViewModel DataGridViewModel { get; set; }
        public FileContentViewModel FileContentViewModel { get; set; }
        public ProjectUsagesViewModel UsagesViewModel { get; set; }
        public ProjectReferencesViewModel ReferencesViewModel { get; set; }

        public ProjectsViewModel(RepositoryProviderUIAdapter adapter)
        {
            _adapter = adapter;
            DataGridViewModel = new ProjectDataGridViewModel(_adapter);
            FileContentViewModel = new FileContentViewModel();
            UsagesViewModel = new ProjectUsagesViewModel();
            ReferencesViewModel = new ProjectReferencesViewModel();
            WireDataGrid();
        }

        public void WireDataGrid()
        {
            DataGridViewModel.PropertyChanged += (obj, args) =>
            {
                if (obj is not ProjectDataGridViewModel projectGridViewModel) return;

                if (args.PropertyName == nameof(ProjectDataGridViewModel.SelectedProject))
                {
                    FileContentViewModel.Content = projectGridViewModel.SelectedProject == null
                        ? string.Empty
                        : _adapter.GetFileContent(projectGridViewModel.SelectedProject.Path);
                    UsagesViewModel.Projects = projectGridViewModel.SelectedProject == null ? []
                        : projectGridViewModel.SelectedProject.Usages.Select(_adapter.GetProject).ToArray();
                    ReferencesViewModel.Projects = projectGridViewModel.SelectedProject == null ? []
                        : projectGridViewModel.SelectedProject.ProjectReferences.Select(_adapter.GetProject).ToArray();
                }
            };
        }
    }
}
