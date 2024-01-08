using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Hephaestus.Core.Domain;

namespace Hephaestus.Core.Parsing.Sdk
{
    public class SdkGacReferenceParser : IGacReferenceParser
    {
        private XDocument _project;
        public SdkGacReferenceParser(XDocument project)
        {
            ArgumentNullException.ThrowIfNull(project, nameof(project));
            _project = project;
        }

        public IEnumerable<GacReference> Parse()
        {
            return _project.Descendants("Reference")
                .Select(x =>
                {
                    var id = x.Attribute("Include")?.Value ?? throw new InvalidDataException();

                    return new GacReference(id);
                });
        }
    }
}