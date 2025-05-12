using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Hephaestus.Core.Application;
using Hephaestus.Core.Parsing;
using Hephaestus.Core.Parsing.Factories;
using Xunit;

namespace Hephaestus.Core.Tests.Parsing
{
    public class CodeRepositoryParserTests
    {
        private BasicFileCollection _files;
        private CodeRespositoryParser _sut;

        public CodeRepositoryParserTests()
        {
            _files = new BasicFileCollection(CacheManager.Build("Test", Files()));
            _sut = new CodeRespositoryParser(
                new SolutionParser(
                    new ProjectParser(
                        new ReferenceParserFactory(),
                        new EmbeddedResourceParserFactory(),
                        new ProjectMetadataParser(
                            new ProjectFormatParser(),
                            new ProjectFrameworkParserFactory(new TfmTranslator()),
                            new ProjectOutputTypeParserFactory(new OutputTypeTranslator()),
                            new AssemblyNameParserFactory(),
                            new RootNamespaceParserFactory(),
                            new TitleParserFactory(),
                            new WarningsParserFactory(),
                            new TestProjectParserFactory()
                            ),
                        new CSharpFileListerFactory(_files),
                        new CSharpFileParser(
                            new CSharpFileNamespaceDeclarationParser(),
                            new CSharpFileUsingDirectiveParser()),
                    _files),
                _files),
                _files);
        }

        [Fact]
        public void CanParseRepository()
        {
            var repo = _sut.Parse("Foo", "C:\\Foo");

            Assert.Equal("Foo", repo.Name);
            Assert.Single(repo.Solutions);
            Assert.Single(repo.Solutions.SelectMany(x => x.Projects));
            Assert.Equal(2, repo.Solutions.SelectMany(x => x.Projects).SelectMany(x => x.Files).Count());
        }

        public Dictionary<string, string> Files()
        {
            return new Dictionary<string, string>
            {
                { "C:\\Foo\\Bar\\FileToCompileOne.cs", TestFileOne() },
                { "C:\\Foo\\Bar\\FileToCompileTwo.cs", TestFileTwo() },
                { "C:\\Foo\\MyTestProject.csproj", ProjectOne().ToString() },
                { "C:\\Foo\\Foo.sln", Solution().ToString() }
            };
        }

        public StringBuilder Solution()
        {
            StringBuilder solutionFile = new StringBuilder();
            solutionFile.AppendLine($"Project(\"{{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}}\") = \"MyTestProject\", \"MyTestProject.csproj\", {{{Guid.NewGuid()}}}");
            solutionFile.AppendLine("EndProject");
            return solutionFile;
        }

        private XDocument ProjectOne()
        {
            return new XDocument(
                    new XElement("Project",
                        new XAttribute("Sdk", "Microsoft.NET.Sdk"),
                        new XElement("PropertyGroup",
                            new XElement("TargetFramework", "net8.0"),
                            new XElement("OutputType", "library"),
                            new XElement("Title", "MyTestProject"),
                            new XElement("AssemblyName", "MyTestProject"),
                            new XElement("RootNamespace", "MyTestProject")
                        ),
                        new XElement("ItemGroup",
                            new XElement("EmbeddedResource", new XAttribute("Include", "..\\..\\Er1")),
                            new XElement("EmbeddedResource", new XAttribute("Include", "..\\Er2"))
                        ),
                        new XElement("ItemGroup",
                            new XElement("PackageReference", new XAttribute("Include", "Third.Party"), new XAttribute("Version", "1.2.3")),
                            new XElement("PackageReference", new XAttribute("Include", "Third.Party.Static.Lib"), new XAttribute("Version", "4.5.6")),
                            new XElement("PackageReference", new XAttribute("Include", "Third.Party.Aliased.Lib"), new XAttribute("Version", "7.8.9"))
                        ),
                        new XElement("ItemGroup",
                            new XElement("ProjectReference", new XAttribute("Include", "..\\AnotherProjectOne.csproj")),
                            new XElement("ProjectReference", new XAttribute("Include", "..\\..\\AnotherProjectTwo.csproj"))
                        )
                    ));
        }

        private static string TestFileOne()
        {
            return @"
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Foo.Bah.AnotherProjectOne;
using Third.Party;
using static Third.Party.Static.Lib;
using MyAlias = Third.Party.Aliased.Lib;

namespace Foo.Bah.Baz
{
    public class Foo : IFooable
    {
    }
}
";
        }

        public static string TestFileTwo()
        {
            return @"
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using Foo.Bah.AnotherProjectTwo;

namespace Foo.Bah.Baz
{
    public class Foo2 : IFooable
    {
    }
}
";
        }

    }
}
