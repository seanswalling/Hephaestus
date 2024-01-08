using System.Collections.Generic;
using Hephaestus.Core.Domain;
using Hephaestus.Core.Version1.Domain;

namespace Hephaestus.Core.Version1.Parsing
{
    internal interface IProjectParserV1
    {
        OutputType ParseOutputType();
        Framework ParseTargetFrameworkMoniker();
        ICollection<EmbeddedResourceV1> ParseEmbeddedResources();
        ICollection<ProjectReferenceV1> ParseProjectReferences();
        ICollection<PackageReferenceV1> ParsePackageReferences();
        ICollection<string> ParseEmptyFolders();
        IEnumerable<string> ParseUsingDirectives();
        IEnumerable<string> ParseCompiledFiles();
        string ParseProjectName();
        IEnumerable<string> ParseNamespaces();
    }
}