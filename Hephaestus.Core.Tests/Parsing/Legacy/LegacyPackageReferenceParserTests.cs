using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Hephaestus.Core.Domain;
using Hephaestus.Core.Parsing.Legacy;
using Xunit;

namespace Hephaestus.Core.Tests.Parsing.Legacy
{
    public class LegacyPackageReferenceParserTests : LegacyFormatTestBase
    {
        [Fact]
        public void CanParsePackageReferences()
        {
            var pr = new XElement("package",
                new XAttribute("id", "Foo.Package"),
                new XAttribute("version", "1.2.3"),
                new XAttribute("targetFramework", "net48"));

            PackagesRoot.Add(pr);

            var result = new LegacyPackageReferenceParser(Packages).Parse();

            Assert.Equal(new PackageReference("Foo.Package", "1.2.3"), result.Single());
        }

        [Fact]
        public void CanParseMultiplePackages()
        {
            var pr1 = new XElement("package",
                new XAttribute("id", "Foo.Package"),
                new XAttribute("version", "1.2.3"),
                new XAttribute("targetFramework", "net48"));

            var pr2 = new XElement("package",
                new XAttribute("id", "Bah.Package"),
                new XAttribute("version", "4.5.6"),
                new XAttribute("targetFramework", "net48"));

            PackagesRoot.Add(pr1);
            PackagesRoot.Add(pr2);

            var result = new LegacyPackageReferenceParser(Packages).Parse();

            Assert.Equal(new[] {
                new PackageReference("Foo.Package", "1.2.3"),
                new PackageReference("Bah.Package", "4.5.6")
            }, result);
        }

        [Fact]
        public void DocumentIsRequired()
        {
            Assert.Throws<ArgumentNullException>(() => new LegacyPackageReferenceParser(null).Parse());
        }

        [Fact]
        public void IdIsRequiredForPackage()
        {
            var pr = new XElement("package",
                new XAttribute("version", "1.2.3"),
                new XAttribute("targetFramework", "net48"));

            PackagesRoot.Add(pr);

            Assert.Throws<InvalidDataException>(() => new LegacyPackageReferenceParser(Packages).Parse().ToArray());
        }

        [Fact]
        public void VersionIsRequiredForPackage()
        {
            var pr = new XElement("package",
                new XAttribute("id", "Foo.Package"),
                new XAttribute("targetFramework", "net48"));

            PackagesRoot.Add(pr);

            Assert.Throws<InvalidDataException>(() => new LegacyPackageReferenceParser(Packages).Parse().ToArray());
        }

        [Fact]
        public void IfNoPackagesReturnEmptyCollection()
        {
            var result = new LegacyPackageReferenceParser(Packages).Parse();

            Assert.Equal(Array.Empty<PackageReference>(), result);
        }

    }
}
