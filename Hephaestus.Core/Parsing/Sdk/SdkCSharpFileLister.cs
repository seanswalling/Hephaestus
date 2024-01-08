using System.Collections.Generic;
using Hephaestus.Core.Domain;

namespace Hephaestus.Core.Parsing.Sdk
{
    public class SdkCSharpFileLister : ICSharpFileLister
    {
        private readonly IFileCollection _fileCollection;
        private readonly ProjectMetadata _projectMetadata;

        public SdkCSharpFileLister(IFileCollection fileCollection, ProjectMetadata projectMetadata)
        {
            _fileCollection = fileCollection;
            _projectMetadata = projectMetadata;
        }

        public IDictionary<string, string> ListFiles()
        {
            //var parent = Directory.GetParent(_projectMetadata.ProjectPath)!.ToString();
            //var root = Path.GetFullPath(parent);

            //if (!Path.EndsInDirectorySeparator(root))
            //    root += Path.DirectorySeparatorChar;

            //var glob = new Glob(".cs", root);
            return _fileCollection.GetFiles(_projectMetadata.ProjectPath);
        }
    }
}
