using System.Xml.Linq;
using Hephaestus.Core.Domain;
using Hephaestus.Core.Parsing.Factories;

namespace Hephaestus.Core.Parsing
{
    public partial class ProjectMetadataParser : IProjectMetadataParser
    {
        private readonly IProjectFormatParser _formatParser;
        private readonly IProjectFrameworkParserFactory _projectFrameworkParserFactory;
        private readonly IProjectOutputTypeParserFactory _projectOutputTypeParserFactory;
        private readonly IAssemblyNameParserFactory _assemblyNameParserFactory;
        private readonly IRootNamespaceParserFactory _rootNamespaceParserFactory;
        private readonly ITitleParserFactory _titleParserFactory;
        private readonly IWarningsParserFactory _warningsParserFactory;

        public ProjectMetadataParser(
            IProjectFormatParser formatParser,
            IProjectFrameworkParserFactory projectFrameworkParserFactory,
            IProjectOutputTypeParserFactory projectOutputTypeParserFactory,
            IAssemblyNameParserFactory assemblyNameParserFactory,
            IRootNamespaceParserFactory rootNamespaceParserFactory,
            ITitleParserFactory titleParserFactory,
            IWarningsParserFactory warningsParserFactory)
        {
            _formatParser = formatParser;
            _projectFrameworkParserFactory = projectFrameworkParserFactory;
            _projectOutputTypeParserFactory = projectOutputTypeParserFactory;
            _assemblyNameParserFactory = assemblyNameParserFactory;
            _rootNamespaceParserFactory = rootNamespaceParserFactory;
            _titleParserFactory = titleParserFactory;
            _warningsParserFactory = warningsParserFactory;
        }

        public ProjectMetadata Parse(string projectPath, XDocument project)
        {
            var format = _formatParser.Parse(project);
            var framework = _projectFrameworkParserFactory.Create(format).Parse(project);
            var outputType = _projectOutputTypeParserFactory.Create(format).Parse(project);
            var assemblyName = _assemblyNameParserFactory.Create(format).Parse(project);
            var rootNamespace = _rootNamespaceParserFactory.Create(format).Parse(project);
            var title = _titleParserFactory.Create(format).Parse(project);
            var warnings = _warningsParserFactory.Create(format).Parse(project);

            return new ProjectMetadata(projectPath, framework, outputType, format, assemblyName, rootNamespace, title ?? assemblyName, warnings);
        }
    }


}
