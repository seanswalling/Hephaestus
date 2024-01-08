using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using Hephaestus.Core.Application;
using Hephaestus.Core.Domain;
using Hephaestus.Core.Parsing;
using Hephaestus.Core.Parsing.Legacy;
using Xunit;

namespace Hephaestus.Core.Tests.Parsing.Legacy
{
    public class LegacyCSharpFileListerTests
    {
        private BasicFileCollection _collection;
        private XNamespace _namespace = "http://schemas.microsoft.com/developer/msbuild/2003";
        private ProjectMetadata _metadata;

        public LegacyCSharpFileListerTests()
        {
            _collection = new BasicFileCollection(CacheManager.Build("Test", Files()));
            _metadata = new ProjectMetadata(
                "C:\\Foo\\thisProject.csproj",
                Framework.net48,
                OutputType.Library,
                ProjectFormat.Framework
                );
        }

        [Fact]
        public void CanListFiles()
        {
            var projectDocument = new XDocument(
               new XElement("Project",
                   new XElement("ItemGroup",
                       new XElement(_namespace + "Compile", new XAttribute("Include", "Bar.cs")),
                       new XElement(_namespace + "Compile", new XAttribute("Include", "..\\Wiz\\Bang.cs"))
                   )
               ));


            var files = new LegacyCSharpFileLister(_collection, projectDocument, _metadata).ListFiles();

            Assert.Equal(2, files.Count);
            Assert.True(files.ContainsKey("C:\\Foo\\Bar.cs"));
            Assert.Equal("The Foo Bar Test File!", files["C:\\Foo\\Bar.cs"]);
            Assert.True(files.ContainsKey("C:\\Wiz\\Bang.cs"));
            Assert.Equal("The Wiz Bang Test File!", files["C:\\Wiz\\Bang.cs"]);
        }

        [Fact]
        public void ThrowsOnMissingInclude()
        {
            var projectDocument = new XDocument(
              new XElement("Project",
                  new XElement("ItemGroup",
                      new XElement(_namespace + "Compile")
                  )
              ));

            Assert.Throws<InvalidDataException>(() => new LegacyCSharpFileLister(_collection, projectDocument, _metadata).ListFiles());
        }

        [Fact]
        public void ReturnsNothingIfFilesNotInCollection()
        {
            var projectDocument = new XDocument(
               new XElement("Project",
                   new XElement("ItemGroup",
                       new XElement(_namespace + "Compile", new XAttribute("Include", "..\\Wiz\\Bang2.cs"))
                   )
               ));

            var result = new LegacyCSharpFileLister(_collection, projectDocument, _metadata).ListFiles();

            Assert.Empty(result);
        }

        public Dictionary<string, string> Files()
        {
            return new Dictionary<string, string>
            {
                { "C:\\Foo\\Bar.cs", "The Foo Bar Test File!" },
                { "C:\\Wiz\\Bang.cs", "The Wiz Bang Test File!" },
            };
        }
    }
}
