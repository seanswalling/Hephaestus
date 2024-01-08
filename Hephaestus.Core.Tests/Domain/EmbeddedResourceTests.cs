using System;
using Hephaestus.Core.Domain;
using Xunit;

namespace Hephaestus.Core.Tests.Domain
{
    public class EmbeddedResourceTests
    {
        [Fact]
        public void CanCreateEmbeddedResource()
        {
            var resource = new EmbeddedResource("Foo\\Bar");
            Assert.Equal("Foo\\Bar", resource.FilePath);
        }

        [Fact]
        public void CanLinkEmbeddedResource()
        {
            var resource = new EmbeddedResource("Foo\\Bar");
            resource.Link("Bah\\Baz");
            Assert.Equal("Bah\\Baz", resource.LinkedPath);
        }

        [Fact]
        public void FilePathIsRequired()
        {
            Assert.Throws<ArgumentNullException>(() => new EmbeddedResource(null));
            Assert.Throws<ArgumentException>(() => new EmbeddedResource(string.Empty));
            Assert.Throws<ArgumentException>(() => new EmbeddedResource(" "));
        }

        [Fact]
        public void LinkedPathIsRequiredWhenLinking()
        {
            var resource = new EmbeddedResource("Foo\\Bar");
            Assert.Throws<ArgumentNullException>(() => resource.Link(null));
            Assert.Throws<ArgumentException>(() => resource.Link(string.Empty));
            Assert.Throws<ArgumentException>(() => resource.Link(" "));
        }

        [Fact]
        public void EmbeddedResourcesAreEqualIfPathIsEqual()
        {
            var resource1 = new EmbeddedResource("Foo\\Bah");
            var resource2 = new EmbeddedResource("Foo\\Bah");

            Assert.True(resource1.Equals(resource2));
        }

        [Fact]
        public void EmbedResourcesHaveEqualHashes()
        {
            var resource1 = new EmbeddedResource("Foo\\Bah");
            var resource2 = new EmbeddedResource("Foo\\Bah");

            Assert.Equal(resource1.GetHashCode(), resource2.GetHashCode());
        }

        [Fact]
        public void EmbeddedResourcesAreEqualIsPathAndLinkedPathAreEqual()
        {
            var resource1 = new EmbeddedResource("Foo\\Bah");
            var resource2 = new EmbeddedResource("Foo\\Bah");

            resource1.Link("Bah\\Baz");
            resource2.Link("Bah\\Baz");

            Assert.True(resource1.Equals(resource2));
        }

        [Fact]
        public void EmbeddedResourcesAreNotEqualIfLinkedPathsAreDifferent()
        {
            var resource1 = new EmbeddedResource("Foo\\Bah");
            var resource2 = new EmbeddedResource("Foo\\Bah");

            resource1.Link("Bah\\Baz");
            resource2.Link("Baz\\Bah");

            Assert.False(resource1.Equals(resource2));
        }

        [Fact]
        public void EmbeddedResourceHashCodesAreEqualForFilePathsAndLinkedPaths()
        {
            var resource1 = new EmbeddedResource("Foo\\Bah");
            var resource2 = new EmbeddedResource("Foo\\Bah");

            resource1.Link("Bah\\Baz");
            resource2.Link("Bah\\Baz");

            Assert.Equal(resource1.GetHashCode(), resource2.GetHashCode());
        }

        [Fact]
        public void EmbeddedResourcesHashCodesAreNotEqualIfLinkedPathsAreDifferent()
        {
            var resource1 = new EmbeddedResource("Foo\\Bah");
            var resource2 = new EmbeddedResource("Foo\\Bah");

            resource1.Link("Bah\\Baz");
            resource2.Link("Baz\\Bah");

            Assert.NotEqual(resource1.GetHashCode(), resource2.GetHashCode());
        }

        [Fact]
        public void EmbeddedResourcesAreEqualAsObjects()
        {
            var resource1 = new EmbeddedResource("Foo\\Bah");
            var resource2 = new EmbeddedResource("Foo\\Bah");

            Assert.True(resource1.Equals((object)resource2));
        }
    }
}
