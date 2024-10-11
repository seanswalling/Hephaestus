using Hephaestus.Avalonia.Models;

namespace Hephaestus.Avalonia.ViewModels
{
    public class FileGridViewModel : ViewModelBase
    {
        public ProjectDataGridViewModel ProjectGrid { get; set; }

        public FileGridViewModel(RepositoryProviderUIAdapter adapter)
        {
            ProjectGrid = new ProjectDataGridViewModel(adapter);
        }
    }
}
