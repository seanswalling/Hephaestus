using System;

namespace Hephaestus.Core.Domain
{
    public class ProjectMetadata
    {
        public string ProjectPath { get; set; }
        public ProjectFormat Format { get; set; }
        public Framework Framework { get; }
        public OutputType OutputType { get; }

        public ProjectMetadata(string projectPath, Framework framework, OutputType outputType, ProjectFormat format)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(projectPath, nameof(projectPath));

            ProjectPath = projectPath;
            Framework = framework;
            OutputType = outputType;
            Format = format;
        }
    }
}
