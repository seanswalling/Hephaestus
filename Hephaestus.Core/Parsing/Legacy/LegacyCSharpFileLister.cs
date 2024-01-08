using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Hephaestus.Core.Domain;

namespace Hephaestus.Core.Parsing.Legacy
{
    public class LegacyCSharpFileLister : LegacyFormat, ICSharpFileLister
    {
        private readonly IFileCollection _fileCollection;
        private readonly XDocument _projectContent;
        private readonly ProjectMetadata _metadata;

        public LegacyCSharpFileLister(IFileCollection fileCollection, XDocument projectContent, ProjectMetadata metadata)
        {
            _fileCollection = fileCollection;
            _projectContent = projectContent;
            _metadata = metadata;
        }

        public IDictionary<string, string> ListFiles()
        {
            var parent = Directory.GetParent(_metadata.ProjectPath)!.ToString();
            var files = _projectContent.Descendants(Namespace + "Compile")
                .Select(x =>
                {
                    return x.Attribute("Include")?.Value ?? throw new InvalidDataException();
                })
                .Select(x => Path.GetFullPath(parent + $"\\{x}"));

            return _fileCollection.GetFiles(files);
        }
    }
}
