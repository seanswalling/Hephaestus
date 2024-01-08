using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Hephaestus.Core.Domain;

namespace Hephaestus.Core.Parsing.Sdk
{
    public class SdkPackageReferenceParser : IPackageReferenceParser
    {
        private XDocument _project;
        public SdkPackageReferenceParser(XDocument project)
        {
            ArgumentNullException.ThrowIfNull(project, nameof(project));
            _project = project;
        }

        public IEnumerable<PackageReference> Parse()
        {
            return _project.Descendants("ItemGroup").Descendants("PackageReference")
                .Select(x =>
                {
                    var name = x.Attribute("Include")?.Value ?? x.Attribute("include")?.Value ?? throw new InvalidDataException();
                    var version = x.Attribute("Version")?.Value ?? x.Attribute("version")?.Value ?? throw new InvalidDataException();

                    return new PackageReference(name, version);
                });
        }
    }
}
