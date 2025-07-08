using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Hephaestus.Core.Domain
{
    public class Project
    {
        public string Name { get; }
        public ProjectMetadata Metadata { get; }
        public IEnumerable<CSharpFile> Files { get; }
        public IEnumerable<EmbeddedResource> EmbeddedResources { get; }
        public ReferenceManager References { get; }

        public Project(
            string name,
            ProjectMetadata metadata,
            IEnumerable<CSharpFile> csFiles,
            IEnumerable<EmbeddedResource> embeddedResources,
            ReferenceManager references)
        {
            ArgumentNullException.ThrowIfNull(name, nameof(name));
            ArgumentNullException.ThrowIfNull(metadata, nameof(metadata));
            ArgumentNullException.ThrowIfNull(csFiles, nameof(csFiles));
            ArgumentNullException.ThrowIfNull(embeddedResources, nameof(embeddedResources));
            ArgumentNullException.ThrowIfNull(references, nameof(references));

            Name = name;
            Files = csFiles;
            EmbeddedResources = embeddedResources;
            References = references;
            Metadata = metadata;
            References = references;
        }

        public void Add(IReference reference)
        {
            References.Add(reference);
        }

        public string[] GetProjectReferenceAsAbsolutePaths()
        {
            return [.. References.ProjectReferences.Select(x =>
                Path.GetFullPath(
                        Path.Combine(
                                Directory.GetParent(Metadata.ProjectPath)?.ToString() ?? string.Empty,
                                x.RelativePath
                        )
                )
            )];
        }

        public void ChangeFramework(Framework framework)
        {
            Metadata.Framework = framework;
        }

        public void RemoveUsing(CSharpUsing usingDirective)
        {
            foreach (var file in Files)
                file.RemoveUsing(usingDirective);
        }
    }
}
