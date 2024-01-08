using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
using Hephaestus.Core.Domain;

namespace Hephaestus.Core.Parsing
{
    public class ProjectFormatParser : IProjectFormatParser
    {
        public ProjectFormat Parse(XDocument project)
        {
            if (project.XPathEvaluate("//@Sdk") is not IEnumerable<object> xpathResult) throw new ArgumentException("invalid xml");

            return xpathResult.Any() ? ProjectFormat.Sdk : ProjectFormat.Framework;
        }
    }
}
