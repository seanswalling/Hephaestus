using System;
using Hephaestus.Core.Domain;
using Hephaestus.Core.Parsing;
using Xunit;

namespace Hephaestus.Core.Tests.Parsing
{
    public class CSharpFileNamespaceParserTests
    {
        [Fact]
        public void ContentCannotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => new CSharpFileNamespaceDeclarationParser().ParseNamespace(null));
        }

        [Theory]
        [InlineData("namespace Foo.Bah.Core{")]
        [InlineData("namespace Foo.Bah.Core\n{")]
        [InlineData("namespace Foo.Bah.Core\r\n{")]
        [InlineData("namespace Foo.Bah.Core {")]
        [InlineData("namespace Foo.Bah.Core \r\n{")]
        [InlineData("namespace Foo.Bah.Core \r\n {")]
        public void CanParseValidNamespaces(string validNamespaces)
        {
            var ns = new CSharpFileNamespaceDeclarationParser().ParseNamespace(validNamespaces);
            Assert.Equal(new CSharpNamespace("Foo.Bah.Core"), ns);
        }

        [Theory]
        [InlineData("namespace Foo.Bah.Baz;")]
        [InlineData("namespace Foo.Bah.Baz\n;")]
        [InlineData("namespace Foo.Bah.Baz\r\n;")]
        [InlineData("namespace Foo.Bah.Baz ;")]
        [InlineData("namespace Foo.Bah.Baz \r\n;")]
        [InlineData("namespace Foo.Bah.Baz \r\n ;")]
        public void CanParseValidFileScopedNamespace(string validFileScopedNamespaces)
        {
            var ns = new CSharpFileNamespaceDeclarationParser().ParseNamespace(validFileScopedNamespaces);
            Assert.Equal(new CSharpNamespace("Foo.Bah.Baz"), ns);
        }

        [Fact]
        public void CanParseNamespaceFromSurroundingContent()
        {
            var ns = new CSharpFileNamespaceDeclarationParser().ParseNamespace("helloworldnamespace Foo.Bah.Baz\r\n{somemoretext");
            Assert.Equal(new CSharpNamespace("Foo.Bah.Baz"), ns);
        }

        [Fact]
        public void DoesNotSupportMoreThanOneFileScopedNamespace()
        {
            Assert.Throws<ArgumentException>(() =>
                new CSharpFileNamespaceDeclarationParser()
                .ParseNamespace("namespace Foo.Bah.Baz;\r\nnamespace Foo.Bah;"));
        }


        [Fact]
        public void ContentWithoutNamespaceTreatedAsEmptyNamespace()
        {
            var ns = new CSharpFileNamespaceDeclarationParser().ParseNamespace("Foo");
            Assert.Equal(string.Empty, ns.Value);
        }
    }
}
