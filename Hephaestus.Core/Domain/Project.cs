using System;
using System.Collections.Generic;

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
    }
}
