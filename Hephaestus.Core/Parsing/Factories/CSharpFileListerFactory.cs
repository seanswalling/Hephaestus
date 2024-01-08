using System;
using System.Xml.Linq;
using Hephaestus.Core.Domain;
using Hephaestus.Core.Parsing.Legacy;
using Hephaestus.Core.Parsing.Sdk;

namespace Hephaestus.Core.Parsing.Factories
{
    public class CSharpFileListerFactory : ICSharpFileListerFactory
    {
        private readonly IFileCollection _fileCollection;

        public CSharpFileListerFactory(IFileCollection fileCollection)
        {
            _fileCollection = fileCollection;
        }

        public ICSharpFileLister Create(ProjectMetadata metadata, XDocument projectContent)
        {
            return metadata.Format switch
            {
                ProjectFormat.Sdk => new SdkCSharpFileLister(_fileCollection, metadata),
                ProjectFormat.Framework => new LegacyCSharpFileLister(_fileCollection, projectContent, metadata),
                _ => throw new ArgumentException(null, nameof(metadata.Format)),
            };
        }
    }
}
