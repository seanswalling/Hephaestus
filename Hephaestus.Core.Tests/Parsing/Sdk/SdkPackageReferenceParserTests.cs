using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Hephaestus.Core.Domain;
using Hephaestus.Core.Parsing.Sdk;
using Xunit;

namespace Hephaestus.Core.Tests.Parsing.Sdk
{
    public class SdkPackageReferenceParserTests : SdkFormatTestBase
    {
        private PackageReference _package = new PackageReference("Foo.Package", "1.2.3");

        [Fact]
        public void CanParsePackageReferences()
        {
            var ig = new XElement("ItemGroup");
            var pr = new XElement("PackageReference",
                new XAttribute("Include", _package.Id),
                new XAttribute("Version", _package.Version));
            ig.Add(pr);
            SdkElement.Add(ig);

            var result = new SdkPackageReferenceParser(Project).Parse();

            Assert.Equal(_package, result.Single());
        }

        [Fact]
        public void MissingIdIsInvalid()
        {
            var ig = new XElement("ItemGroup");
            var pr = new XElement("PackageReference",
                new XAttribute("Version", _package.Version));
            ig.Add(pr);
            SdkElement.Add(ig);

            Assert.Throws<InvalidDataException>(() => new SdkPackageReferenceParser(Project).Parse().ToArray());
        }

        [Fact]
        public void MissingVersionIsInvalid()
        {
            var ig = new XElement("ItemGroup");
            var pr = new XElement("PackageReference",
                new XAttribute("Include", _package.Id));
            ig.Add(pr);
            SdkElement.Add(ig);

            Assert.Throws<InvalidDataException>(() => new SdkPackageReferenceParser(Project).Parse().ToArray());
        }

        [Fact]
        public void IfNoPackageReferenceReturnEmpty()
        {
            var ig = new XElement("ItemGroup");
            SdkElement.Add(ig);

            var result = new SdkPackageReferenceParser(Project).Parse();

            Assert.Equal(Array.Empty<PackageReference>(), result);
        }

        [Fact]
        public void IfNoItemGroupReturnEmpty()
        {
            var result = new SdkPackageReferenceParser(Project).Parse();

            Assert.Equal(Array.Empty<PackageReference>(), result);
        }

        [Fact]
        public void ReturnMultiplePackages()
        {
            var package2 = new PackageReference("Bah.Package", "4.5.6");
            var ig = new XElement("ItemGroup");
            var pr1 = new XElement("PackageReference",
                new XAttribute("Include", _package.Id),
                new XAttribute("Version", _package.Version));
            var pr2 = new XElement("PackageReference",
                new XAttribute("Include", package2.Id),
                new XAttribute("Version", package2.Version));
            ig.Add(pr1);
            ig.Add(pr2);
            SdkElement.Add(ig);

            var result = new SdkPackageReferenceParser(Project).Parse();

            Assert.Equal(new[] { _package, package2 }, result);
        }
    }
}
