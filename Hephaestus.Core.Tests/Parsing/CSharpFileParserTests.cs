using System.Linq;
using Hephaestus.Core.Domain;
using Hephaestus.Core.Parsing;
using Xunit;

namespace Hephaestus.Core.Tests.Parsing
{
    public class CSharpFileParserTests
    {
        [Fact]
        public void CanParseNamespaceFromTestFile()
        {
            var parser = new CSharpFileParser(
                new CSharpFileNamespaceDeclarationParser(),
                new CSharpFileUsingDirectiveParser()
                );

            var csFile = parser.ParseFile("Foo/File.cs", TestFile());

            Assert.Equal(
                new CSharpNamespace("Foo.Bah.Baz"),
                csFile.NamespaceDeclaration
                );

        }

        [Fact]
        public void CanParseUsingDirectivesFromTestFile()
        {
            var parser = new CSharpFileParser(
               new CSharpFileNamespaceDeclarationParser(),
               new CSharpFileUsingDirectiveParser()
               );

            var csFile = parser.ParseFile("Foo/File.cs", TestFile());

            var expectedUsings = new CSharpUsing[]
            {
                new(new("System")),
                new(new("System.Text")),
                new(new("System.Collections")),
                new(new("System.Collections.Generic")),
                new(new("System.Runtime.CompilerServices")),
                new(new("Foo.Bah.Qwop")),
                new(new("Third.Party.Library")),
                new(new("Third.Party.Static.Lib")),
                new(new("Third.Party.Aliased.Lib")),
            };

            Assert.Equal(
                expectedUsings.OrderBy(x => x.Value.Value),
                csFile.UsingDirectives.OrderBy(x => x.Value.Value)
                );
        }

        private static string TestFile()
        {
            return @"
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Foo.Bah.Qwop;
using Third.Party.Library;
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
    }

}
