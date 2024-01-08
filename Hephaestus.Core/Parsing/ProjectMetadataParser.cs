using System.Xml.Linq;
using Hephaestus.Core.Domain;
using Hephaestus.Core.Parsing.Factories;

namespace Hephaestus.Core.Parsing
{
    public class ProjectMetadataParser : IProjectMetadataParser
    {
        private readonly IProjectFormatParser _formatParser;
        private readonly IProjectFrameworkParserFactory _projectFrameworkParserFactory;
        private readonly IProjectOutputTypeParserFactory _projectOutputTypeParserFactory;

        public ProjectMetadataParser(
            IProjectFormatParser formatParser,
            IProjectFrameworkParserFactory projectFrameworkParserFactory,
            IProjectOutputTypeParserFactory projectOutputTypeParserFactory)
        {
            _formatParser = formatParser;
            _projectFrameworkParserFactory = projectFrameworkParserFactory;
            _projectOutputTypeParserFactory = projectOutputTypeParserFactory;
        }

        public ProjectMetadata Parse(string projectPath, XDocument project)
        {
            var format = _formatParser.Parse(project);
            var framework = _projectFrameworkParserFactory.Create(format).Parse(project);
            var outputType = _projectOutputTypeParserFactory.Create(format).Parse(project);

            return new ProjectMetadata(projectPath, framework, outputType, format);
        }
    }
}
