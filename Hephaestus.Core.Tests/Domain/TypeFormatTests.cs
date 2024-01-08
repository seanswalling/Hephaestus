using System;
using Hephaestus.Core.Domain;
using Xunit;

namespace Hephaestus.Core.Tests.Domain
{
    public class TypeFormatTests
    {
        [Theory]
        [InlineData("Foo.Bah.Baz")]
        [InlineData("Foo3.Bah2.Baz1")]
        [InlineData("Foo")]
        [InlineData("_Foo")]
        [InlineData("_Foo._bah")]
        public void ValidFormats(string content)
        {
            Assert.Equal(content, new TypeFormat(content).Value);
        }

        [Theory]
        [InlineData("1Foo")]
        [InlineData("Foo.")]
        [InlineData("Foo*")]
        [InlineData("Foo-")]
        [InlineData("Foo!")]
        [InlineData("Foo?")]
        [InlineData("Foo Bah")]
        [InlineData("Foo-.Bah")]
        [InlineData("Foo#")]//..so on so forth
        public void InvalidFormats(string content)
        {
            var exception = Assert.Throws<ArgumentException>(() => new TypeFormat(content));
            Assert.Equal("invalid format", exception.Message);
        }

        [Fact]
        public void FormatEquals()
        {
            Assert.True(new TypeFormat("Foo.Bah")
                .Equals(new TypeFormat("Foo.Bah")));
            Assert.True(new TypeFormat("Foo.Bah")
                .Equals(new TypeFormat("foo.bah")));
        }

        [Fact]
        public void FormatEqualsHasCode()
        {
            Assert.Equal(
                new TypeFormat("Foo.Bah").GetHashCode(),
                new TypeFormat("Foo.Bah").GetHashCode()
                );
            Assert.Equal(
                new TypeFormat("Foo.Bah").GetHashCode(),
                new TypeFormat("foo.bah").GetHashCode()
                );
        }

        [Fact]
        public void FormatNotEquals()
        {
            Assert.False(new TypeFormat("Foo.Bah")
                .Equals(new TypeFormat("Foo.Baz")));
            Assert.False(new TypeFormat("Foo.Bah")
                .Equals(new object()));
        }
    }
}
