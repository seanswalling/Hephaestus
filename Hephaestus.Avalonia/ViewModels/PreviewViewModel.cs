namespace Hephaestus.Avalonia.ViewModels
{
    public class PreviewViewModel : ViewModelBase
    {
        public FileContentViewModel File { get; set; }
        public ProjectReferencesViewModel References { get; set; }
        public ProjectUsagesViewModel Usages { get; set; }

        public PreviewViewModel()
        {
            File = new FileContentViewModel();
            References = new ProjectReferencesViewModel();
            Usages = new ProjectUsagesViewModel();
        }
    }
}
