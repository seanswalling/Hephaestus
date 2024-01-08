using System;
using Hephaestus.Core.Domain;
using Hephaestus.Core.Parsing;
using Xunit;

namespace Hephaestus.Core.Tests.Parsing
{
    public class UsingDirectiveParserTests
    {
        [Fact]
        public void ValueCannotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => new CSharpFileUsingDirectiveParser().ParseUsingDirectives(null));
        }

        [Theory]
        [InlineData("using foo = Foo.Bah;", "Foo.Bah")]
        [InlineData("using foo=Foo.Bah;", "Foo.Bah")]
        [InlineData("using foo = Foo.Bah ;", "Foo.Bah")]
        [InlineData("using foo =  Foo.Bah ;", "Foo.Bah")]
        [InlineData("using foo =  FooBah;", "FooBah")]
        [InlineData("using foo = FooBah;", "FooBah")]
        [InlineData("using    foo = FooBah;", "FooBah")]
        [InlineData("using foo = FooBah;   ", "FooBah")]
        [InlineData("using foo =  Foo. Bah;   ", "Foo. Bah")]
        [InlineData("using foo =  Foo . Bah;   ", "Foo . Bah")]
        public void MatchesAliases(string input, string output)
        {
            var parser = new CSharpFileUsingDirectiveParser();
            Assert.Equal(
                new[] { new CSharpUsing(new CSharpNamespace(output)) },
                parser.ParseUsingDirectives(input)
                );
        }

        [Theory]
        [InlineData("using static Foo.Bah;", "Foo.Bah")]
        [InlineData("using staticFoo.Bah;", "Foo.Bah")]
        [InlineData("using staticFoo.Bah ;", "Foo.Bah")]
        [InlineData("using static Foo.Bah ;", "Foo.Bah")]
        [InlineData("using static FooBah;", "FooBah")]
        [InlineData("using staticFooBah;", "FooBah")]
        [InlineData("using    staticFooBah;", "FooBah")]
        [InlineData("using staticFooBah;   ", "FooBah")]
        [InlineData("using static Foo. Bah;   ", "Foo. Bah")]
        [InlineData("using static Foo . Bah;   ", "Foo . Bah")]
        public void MatchesStatics(string input, string output)
        {
            var parser = new CSharpFileUsingDirectiveParser();
            Assert.Equal(
                new[] { new CSharpUsing(new CSharpNamespace(output)) },
                parser.ParseUsingDirectives(input)
                );
        }

        [Theory]
        [InlineData("using Foo.Bah;", "Foo.Bah")]
        [InlineData("usingFoo.Bah;", "Foo.Bah")]
        [InlineData("using Foo.Bah ;", "Foo.Bah")]
        [InlineData("using  Foo.Bah ;", "Foo.Bah")]
        [InlineData("using  FooBah;", "FooBah")]
        [InlineData("using FooBah;", "FooBah")]
        [InlineData("   using FooBah;", "FooBah")]
        [InlineData("using FooBah;   ", "FooBah")]
        [InlineData("using  Foo. Bah;   ", "Foo. Bah")]
        [InlineData("using  Foo . Bah;   ", "Foo . Bah")]
        public void MatchesNormalUsings(string input, string output)
        {
            var parser = new CSharpFileUsingDirectiveParser();
            Assert.Equal(
                new[] { new CSharpUsing(new CSharpNamespace(output)) },
                parser.ParseUsingDirectives(input)
                );
        }
    }
}
