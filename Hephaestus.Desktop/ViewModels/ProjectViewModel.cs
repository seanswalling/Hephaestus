using Hephaestus.Core.Domain;

namespace Hephaestus.Desktop.ViewModels
{
    public class ProjectViewModel
    {
        public string Name { get; }
        public ProjectFormat Format { get; }
        public OutputType OutputType { get; }
        public Framework Framework { get; }
        public string[] ProjectReferences { get; }
        public string[] Usages { get; }
        public string Path { get; }
        public int FrameworkReferences { get; }
        public int FrameworkUsages { get; }
        public WorkState WorkStateValue { get; }

        public ProjectViewModel(string name, string path, ProjectFormat format, OutputType outputType, Framework framework, string[] projectReferences, string[] usages, int frameworkReferences, int frameworkUsages)
        {
            Name = name;
            Path = path;
            Format = format;
            OutputType = outputType;
            Framework = framework;
            ProjectReferences = projectReferences;
            Usages = usages;
            FrameworkReferences = frameworkReferences;
            FrameworkUsages = frameworkUsages;

            if (Format == ProjectFormat.Sdk)
            {
                WorkStateValue = WorkState.Done;
            }
            else if (FrameworkReferences == 0)
            {
                WorkStateValue = WorkState.ReadyToDo;
            }
            else
            {
                WorkStateValue = WorkState.NotDone;
            }

        }

        public enum WorkState
        {
            NotDone,
            Done,
            ReadyToDo
        }
    }
}