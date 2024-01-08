using System;
using System.Xml.Linq;
using Hephaestus.Core.Domain;
using Hephaestus.Core.Parsing.Legacy;
using Hephaestus.Core.Parsing.Sdk;

namespace Hephaestus.Core.Parsing.Factories
{
    public class ReferenceParserFactory : IReferenceParserFactory
    {
        public IReferenceParser Create(ProjectFormat format, XDocument projectDocument, XDocument? packageDocument)
        {
            return format switch
            {
                ProjectFormat.Sdk => new ReferenceParser(new SdkPackageReferenceParser(projectDocument), new SdkProjectReferenceParser(projectDocument)),
                ProjectFormat.Framework => new ReferenceParser(new LegacyPackageReferenceParser(packageDocument!), new LegacyProjectReferenceParser(projectDocument)),
                _ => throw new ArgumentException(null, nameof(format)),
            };
        }
    }
}
