using System;
using Hephaestus.Core.Domain;
using Xunit;

namespace Hephaestus.Core.Tests.Domain
{
    public class CSharpNamespaceTests
    {

        [Fact]
        public void ContentCantBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => new CSharpNamespace(null));
        }

        [Fact]
        public void NamespaceEquals()
        {
            var ns = "Foo.Bah";
            Assert.True(new CSharpNamespace(ns)
                .Equals(new CSharpNamespace(ns)));
        }

        [Fact]
        public void NamespaceCanEqualNamespaceAsObject()
        {
            var ns1 = new CSharpNamespace("Foo.Bah");
            var ns2 = new CSharpNamespace("Foo.Bah");

            Assert.True(ns1.Equals((object)ns2));
            Assert.True(ns2.Equals((object)ns1));
            Assert.False(ns1.Equals(new { }));
        }
    }
}
