using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Hephaestus.Core.Domain;
using Hephaestus.Core.Parsing.Factories;

namespace Hephaestus.Core.Parsing
{
    public class ProjectParser : IProjectParser
    {
        private readonly IReferenceParserFactory _referenceParserFactory;
        private readonly IEmbeddedResourceParserFactory _embeddedResourceParserFactory;
        private readonly IProjectMetadataParser _projectMetadataParser;
        private readonly ICSharpFileListerFactory _cSharpFileListerFactory;
        private readonly ICSharpFileParser _cSharpFileParser;
        private readonly IFileCollection _fileCollection;

        //Project sharing between Slns, can't leed to a massive blow out in processing for
        //every re-processed project.
        private readonly Dictionary<string, Project> _projectCache = [];

        public ProjectParser(
            IReferenceParserFactory referenceParserFactory,
            IEmbeddedResourceParserFactory embeddedResourceParserFactory,
            IProjectMetadataParser projectMetadataParser,
            ICSharpFileListerFactory cSharpFileListerFactory,
            ICSharpFileParser cSharpFileParser,
            IFileCollection fileCollection)
        {
            _referenceParserFactory = referenceParserFactory;
            _embeddedResourceParserFactory = embeddedResourceParserFactory;
            _projectMetadataParser = projectMetadataParser;
            _cSharpFileListerFactory = cSharpFileListerFactory;
            _cSharpFileParser = cSharpFileParser;
            _fileCollection = fileCollection;
        }

        public Project Parse(string filePath, XDocument document)
        {
            if (_projectCache.ContainsKey(filePath))
            {
                return _projectCache[filePath];
            }
            else
            {
                var project = InnerParse(filePath, document);
                _projectCache.Add(filePath, project);
                return project;
            }
        }

        private Project InnerParse(string filePath, XDocument document)
        {
            var fileName = Path.GetFileName(filePath);
            var metadata = _projectMetadataParser.Parse(filePath, document);
            var embeddedResources = _embeddedResourceParserFactory.Create(metadata.Format).Parse(document);
            var csFiles = _cSharpFileListerFactory
                .Create(metadata, document)
                .ListFiles()
                .Select((kvp) => _cSharpFileParser.ParseFile(kvp.Key, kvp.Value))
                .ToList();

            //The below block is hacky, need a better way but low priority atm.
            XDocument packages = null;
            if (metadata.Format == ProjectFormat.Framework)
            {
                var parent = Directory.GetParent(metadata.ProjectPath)!.ToString();
                var root = Path.GetFullPath(parent);

                //if (!Path.EndsInDirectorySeparator(root))
                //{
                //    root += Path.DirectorySeparatorChar;
                //}

                // var glob = new Glob("packages.config", root);
                if (_fileCollection.Exists(parent + "\\packages.config"))
                {
                    packages = XDocument.Parse(_fileCollection.GetContent(parent + "\\packages.config"));
                }
                else
                {
                    packages = new XDocument();
                }
            }

            var references = new ReferenceManager();
            var parser = _referenceParserFactory.Create(metadata.Format, document, packages);

            foreach (var reference in parser.Parse())
            {
                references.Add(reference);
            }

            var project = new Project(fileName, metadata, csFiles, embeddedResources, references);

            return project;
        }
    }
}
