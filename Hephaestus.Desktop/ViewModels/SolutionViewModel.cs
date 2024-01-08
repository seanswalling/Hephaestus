namespace Hephaestus.Desktop.ViewModels
{
    public class SolutionViewModel
    {
        public string Name { get; }
        public string Path { get; }

        public SolutionViewModel(string name, string path)
        {
            Name = name;
            Path = path;
        }
    }
}