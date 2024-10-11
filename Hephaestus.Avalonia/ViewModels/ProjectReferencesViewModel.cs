using CommunityToolkit.Mvvm.ComponentModel;

namespace Hephaestus.Avalonia.ViewModels
{
    public partial class ProjectReferencesViewModel : ViewModelBase
    {
        [ObservableProperty]
        private ProjectViewModel[] _projects;

        public ProjectReferencesViewModel()
        {
            Projects = [];
        }
    }
}
