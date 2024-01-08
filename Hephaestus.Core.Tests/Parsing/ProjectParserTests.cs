using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Hephaestus.Core.Application;
using Hephaestus.Core.Domain;
using Hephaestus.Core.Parsing;
using Hephaestus.Core.Parsing.Factories;
using Xunit;

namespace Hephaestus.Core.Tests.Parsing
{
    public class ProjectParserTests
    {
        private ProjectParser _sut;
        private BasicFileCollection _files;
        private XNamespace Namespace = "http://schemas.microsoft.com/developer/msbuild/2003";
        public ProjectParserTests()
        {
            _files = new BasicFileCollection(CacheManager.Build("Test", Files()));
            _sut = new ProjectParser(
                new ReferenceParserFactory(),
                new EmbeddedResourceParserFactory(),
                new ProjectMetadataParser(
                    new ProjectFormatParser(),
                    new ProjectFrameworkParserFactory(new TfmTranslator()),
                    new ProjectOutputTypeParserFactory(new OutputTypeTranslator())
                    ),
                new CSharpFileListerFactory(_files),
                new CSharpFileParser(
                    new CSharpFileNamespaceDeclarationParser(),
                    new CSharpFileUsingDirectiveParser()),
                _files
                );
        }

        [Fact]
        public void CanParseSdkProject()
        {
            var project = _sut.Parse("C:\\Foo\\Bar\\Baz\\TestProjectSdk.csproj", SdkProjectDocument());

            Assert.Equal(2, project.EmbeddedResources.Count());
            Assert.Equal(3, project.References.PackageReferences.Count());
            Assert.Equal(2, project.References.ProjectReferences.Count());
            Assert.Equal(ProjectFormat.Sdk, project.Metadata.Format);
            Assert.Equal(Framework.net80, project.Metadata.Framework);
            Assert.Equal(OutputType.Library, project.Metadata.OutputType);
            Assert.Equal(2, project.Files.Count());
        }

        [Fact]
        public void CanParseLegacyProject()
        {
            var project = _sut.Parse("C:\\Foo\\Bar\\Baz\\TestProjectLegacy.csproj", LegacyProjectDocument());

            Assert.Equal(2, project.EmbeddedResources.Count());
            Assert.Equal(3, project.References.PackageReferences.Count());
            Assert.Equal(2, project.References.ProjectReferences.Count());
            Assert.Equal(ProjectFormat.Framework, project.Metadata.Format);
            Assert.Equal(Framework.net48, project.Metadata.Framework);
            Assert.Equal(OutputType.Winexe, project.Metadata.OutputType);
            Assert.Equal(2, project.Files.Count());
        }

        public Dictionary<string, string> Files()
        {
            return new Dictionary<string, string>
            {
                {"C:\\Foo\\Bar\\Baz\\FileToCompileOne.cs", TestFileOne()},
                {"C:\\Foo\\Bar\\Baz\\FileToCompileTwo.cs", TestFileTwo()},
                {"C:\\Foo\\Bar\\Baz\\packages.config", PackagesDocument().ToString()},
                { "C:\\Foo\\Bar\\Baz\\TestProjectLegacy.csproj", LegacyProjectDocument().ToString() },
                { "C:\\Foo\\Bar\\Baz\\TestProjectSdk.csproj", SdkProjectDocument().ToString() }
            };
        }

        public XDocument PackagesDocument()
        {
            return new XDocument(
                new XElement("packages",
                    new XElement("package", new XAttribute("id", "Third.Party"), new XAttribute("version", "1.2.3")),
                    new XElement("package", new XAttribute("id", "Third.Party.Static.Lib"), new XAttribute("version", "4.5.6")),
                    new XElement("package", new XAttribute("id", "Third.Party.Aliased.Lib"), new XAttribute("version", "7.8.9"))
                ));
        }

        public XDocument LegacyProjectDocument()
        {
            return new XDocument(
               new XElement(Namespace + "Project",
                   new XElement("PropertyGroup",
                       new XElement(Namespace + "TargetFrameworkVersion", "net48"),
                       new XElement(Namespace + "OutputType", "winexe")
                   ),
                   new XElement("ItemGroup",
                       new XElement(Namespace + "EmbeddedResource", new XAttribute("Include", "..\\..\\Er1")),
                       new XElement(Namespace + "EmbeddedResource", new XAttribute("Include", "..\\Er2"))
                   ),
                   new XElement("ItemGroup",
                       new XElement(Namespace + "ProjectReference", new XAttribute("Include", "..\\AnotherProjectOne.csproj")),
                       new XElement(Namespace + "ProjectReference", new XAttribute("Include", "..\\..\\AnotherProjectTwo.csproj"))
                   ),
                   new XElement("ItemGroup",
                       new XElement(Namespace + "Compile", new XAttribute("Include", "FileToCompileOne.cs")),
                       new XElement(Namespace + "Compile", new XAttribute("Include", "FileToCompileTwo.cs"))
                   )
               ));
        }

        public XDocument SdkProjectDocument()
        {
            return new XDocument(
                new XElement("Project",
                    new XAttribute("Sdk", "Microsoft.NET.Sdk"),
                    new XElement("PropertyGroup",
                        new XElement("TargetFramework", "net8.0"),
                        new XElement("OutputType", "library")
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
