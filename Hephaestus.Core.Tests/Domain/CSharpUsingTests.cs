using System;
using Hephaestus.Core.Domain;
using Xunit;

namespace Hephaestus.Core.Tests.Domain
{
    public class CSharpUsingTests
    {
        [Fact]
        public void ThrowsWhenNull()
        {
            Assert.Throws<ArgumentNullException>(() => new CSharpUsing(null));
        }

        [Fact]
        public void UsingCanEqualNamespace()
        {
            var us = new CSharpUsing(new CSharpNamespace("Foo.Bah"));
            var ns = new CSharpNamespace("Foo.Bah");
            Assert.True(us.Equals(ns));
        }

        [Fact]
        public void UsingCanHashEqualsNamespace()
        {
            var us = new CSharpUsing(new CSharpNamespace("Foo.Bah"));
            var ns = new CSharpNamespace("Foo.Bah");
            Assert.Equal(us.GetHashCode(), ns.GetHashCode());
        }

        [Fact]
        public void UsingCanEqualUsingOrNamespaceAsObject()
        {
            var us1 = new CSharpUsing(new CSharpNamespace("Foo.Bah"));
            var us2 = new CSharpUsing(new CSharpNamespace("Foo.Bah"));
            var ns = new CSharpNamespace("Foo.Bah");

            Assert.True(us1.Equals((object)us2));
            Assert.True(us2.Equals((object)us1));
            Assert.True(us1.Equals((object)ns));
            Assert.False(us1.Equals(new { }));
        }
    }
}
