using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Hephaestus.Core.Domain;

namespace Hephaestus.Core.Parsing.Sdk
{
    public class SdkProjectReferenceParser : IProjectReferenceParser
    {
        private XDocument _project;
        public SdkProjectReferenceParser(XDocument project)
        {
            ArgumentNullException.ThrowIfNull(project, nameof(project));
            _project = project;
        }

        public IEnumerable<ProjectReference> Parse()
        {
            return _project.Descendants("ProjectReference")
                .Select(x =>
                {
                    var relativePath = x.Attribute("Include")?.Value ?? throw new InvalidDataException();

                    return new ProjectReference(relativePath);
                });
        }
    }
}
