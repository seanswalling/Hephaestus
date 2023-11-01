using System.Collections.Generic;
using Hephaestus.Core.Domain;

namespace Hephaestus.Core.Parsing
{
    internal interface IProjectParser
    {
        OutputType ParseOutputType();
        Framework ParseTargetFrameworkMoniker();
        ICollection<EmbeddedResource> ParseEmbeddedResources();
        ICollection<ProjectReference> ParseProjectReferences();
        ICollection<PackageReference> ParsePackageReferences();
        ICollection<string> ParseEmptyFolders();
        IEnumerable<string> ParseUsingDirectives();
        IEnumerable<string> ParseCompiledFiles();
        string ParseProjectName();
        IEnumerable<string> ParseNamespaces();
    }
}