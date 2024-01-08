using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Hephaestus.Core.Domain;

namespace Hephaestus.Core.Parsing.Legacy
{
    public class LegacyPackageReferenceParser : LegacyFormat, IPackageReferenceParser
    {
        private readonly XDocument _packages;

        public LegacyPackageReferenceParser(XDocument packages)
        {
            ArgumentNullException.ThrowIfNull(packages, nameof(packages));
            _packages = packages;
        }

        public IEnumerable<PackageReference> Parse()
        {
            return _packages.Descendants("package")
                .Select(x =>
                {
                    var idAttribute = x.Attribute("id");
                    var versionAttribute = x.Attribute("version");

                    if (idAttribute == null)
                    {
                        throw new InvalidDataException("Package Reference Id was null");
                    }

                    if (versionAttribute == null)
                    {
                        throw new InvalidDataException("Package Reference Version was null");
                    }

                    return new PackageReference(idAttribute.Value, versionAttribute.Value);
                });
        }
    }
}
