using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Hephaestus.Core.Domain;

namespace Hephaestus.Core.Parsing.Legacy
{
    public class LegacyGacReferenceParser : IGacReferenceParser
    {
        private XDocument _project;

        public LegacyGacReferenceParser(XDocument project)
        {
            ArgumentNullException.ThrowIfNull(project, nameof(project));
            _project = project;
        }

        public IEnumerable<GacReference> Parse()
        {
            return [];//Todo add support later when I feel like it
        }
    }
}