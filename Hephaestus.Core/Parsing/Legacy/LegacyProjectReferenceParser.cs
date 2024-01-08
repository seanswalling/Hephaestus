using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Hephaestus.Core.Domain;

namespace Hephaestus.Core.Parsing.Legacy
{
    public class LegacyProjectReferenceParser : LegacyFormat, IProjectReferenceParser
    {
        private XDocument _project;

        public LegacyProjectReferenceParser(XDocument project)
        {
            _project = project;
        }

        public IEnumerable<ProjectReference> Parse()
        {
            return _project.Descendants(Namespace + "ProjectReference")
                .Select(x =>
                {
                    var relativePath = x.Attribute("Include")?.Value ?? throw new InvalidDataException();
                    return new ProjectReference(relativePath);
                });
        }
    }
}
