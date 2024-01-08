using System.Xml.Linq;
using Hephaestus.Core.Domain;
using Hephaestus.Core.Parsing;
using Hephaestus.Core.Parsing.Factories;
using Xunit;

namespace Hephaestus.Core.Tests.Parsing
{
    public class ProjectMetadataParserTests
    {
        private ProjectMetadataParser _sut;
        private XNamespace _namespace = "http://schemas.microsoft.com/developer/msbuild/2003";

        public ProjectMetadataParserTests()
        {
            _sut = new ProjectMetadataParser(
                new ProjectFormatParser(),
                new ProjectFrameworkParserFactory(new TfmTranslator()),
                new ProjectOutputTypeParserFactory(new OutputTypeTranslator()),
                new AssemblyNameParserFactory(),
                new RootNamespaceParserFactory(),
                new TitleParserFactory(),
                new WarningsParserFactory()
                );
        }

        [Fact]
        public void CanParseSdkMetadata()
        {
            var projectDocument = new XDocument(
                new XElement("Project",
                    new XAttribute("Sdk", "Microsoft.NET.Sdk"),
                    new XElement("PropertyGroup",
                        new XElement("TargetFramework", "net8.0"),
                        new XElement("OutputType", "library"),
                        new XElement("Title", "MyTestProject"),
                        new XElement("AssemblyName", "MyTestProject"),
                        new XElement("RootNamespace", "MyTestProject")
                    )
                ));

            var metadata = _sut.Parse("c:\\Foo\\Bah.csproj", projectDocument);

            Assert.Equal("c:\\Foo\\Bah.csproj", metadata.ProjectPath);
            Assert.Equal(Framework.net80, metadata.Framework);
            Assert.Equal(ProjectFormat.Sdk, metadata.Format);
            Assert.Equal(OutputType.Library, metadata.OutputType);
        }

        [Fact]
        public void CanParseLegacyMetadata()
        {
            var projectDocument = new XDocument(
                new XElement("Project",
                    new XElement("PropertyGroup",
                        new XElement(_namespace + "TargetFrameworkVersion", "net48"),
                        new XElement(_namespace + "OutputType", "winexe"),
                        new XElement(_namespace + "Title", "MyTestProject"),
                        new XElement(_namespace + "AssemblyName", "MyTestProject"),
                        new XElement(_namespace + "RootNamespace", "MyTestProject")
                    )
                ));

            var metadata = _sut.Parse("c:\\Foo\\Bah.csproj", projectDocument);

            Assert.Equal("c:\\Foo\\Bah.csproj", metadata.ProjectPath);
            Assert.Equal(Framework.net48, metadata.Framework);
            Assert.Equal(ProjectFormat.Framework, metadata.Format);
            Assert.Equal(OutputType.Winexe, metadata.OutputType);
        }
    }
}



