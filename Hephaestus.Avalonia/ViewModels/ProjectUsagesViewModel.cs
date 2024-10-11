using CommunityToolkit.Mvvm.ComponentModel;

namespace Hephaestus.Avalonia.ViewModels
{
    public partial class ProjectUsagesViewModel : ViewModelBase
    {
        [ObservableProperty]
        private ProjectViewModel[] _projects;

        public ProjectUsagesViewModel()
        {
            Projects = [];
        }
    }
}
