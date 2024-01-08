using Hephaestus.Desktop.Models;

namespace Hephaestus.Desktop.ViewModels
{
    public class FileGridViewModel : ViewModelBase
    {
        public ProjectGridViewModel ProjectGrid { get; set; }

        public FileGridViewModel(RepositoryProviderUIAdapter adapter)
        {
            ProjectGrid = new ProjectGridViewModel(adapter);
        }
    }
}
