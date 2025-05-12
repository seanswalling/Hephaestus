using System;

namespace Hephaestus.Core.Domain
{
    public class ProjectMetadata
    {
        public string ProjectPath { get; set; }
        public ProjectFormat Format { get; set; }
        public Framework Framework { get; set; }
        public OutputType OutputType { get; }
        public string AssemblyName { get; }
        public string RootNamespace { get; }
        public string Title { get; }
        public Warnings Warnings { get; }
        public bool IsTestProject { get; set; }

        public ProjectMetadata(
            string projectPath,
            Framework framework,
            OutputType outputType,
            ProjectFormat format,
            string assemblyName,
            string rootNamespace,
            string title,
            Warnings warnings,
            bool isTestProject)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(projectPath, nameof(projectPath));

            ProjectPath = projectPath;
            Framework = framework;
            OutputType = outputType;
            Format = format;
            AssemblyName = assemblyName;
            RootNamespace = rootNamespace;
            Title = title;
            Warnings = warnings;
            IsTestProject = isTestProject;
        }
    }
}
