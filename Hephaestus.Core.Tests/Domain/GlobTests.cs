using Hephaestus.Core.Domain;
using System;
using Xunit;

namespace Hephaestus.Core.Tests.Domain
{
    public class GlobTests
    {
        [Fact]
        public void IncludesFileWithCorrectPathAndExtension()
        {
            var glob = new Glob(".bah", "c:/");
            Assert.True(glob.IncludesFile("C:/foo.bah"));
        }

        [Fact]
        public void DoesNotIncludeFileWithWrongExtension()
        {
            var glob = new Glob(".foo", "d:/");
            Assert.False(glob.IncludesFile("d:/foo.bah"));
            Assert.False(glob.IncludesFile("d:/foo.baz"));
            Assert.False(glob.IncludesFile("d:/foo.cs"));
        }

        [Fact]
        public void DoesNotIncludeFileWithWrongBasePath()
        {
            var glob = new Glob(".cs", "c:/");
            Assert.False(glob.IncludesFile("d:/foo.cs"));
        }

        [Fact]
        public void HasCorrectExtension()
        {
            var glob = new Glob(".cs", "c:/");
            Assert.True(glob.HasCorrectExtension("foo.cs"));
        }

        [Fact]
        public void DoesNotHaveCorrectExtension()
        {
            var glob = new Glob(".cs", "c:/");
            Assert.False(glob.HasCorrectExtension("foo.exe"));
        }

        [Fact]
        public void IsBaseOf()
        {
            var glob = new Glob(".cs", "c:/");
            Assert.True(glob.IsBaseOf("c:/foo"));
        }

        [Fact]
        public void IsNotBaseOf()
        {
            var glob = new Glob(".cs", "c:/");
            Assert.False(glob.IsBaseOf("d:/foo"));
        }

        [Fact]
        public void IsBaseOfAllowsForGrandchildren()
        {
            var glob = new Glob(".cs", "c:/");
            Assert.True(glob.IsBaseOf("c:/foo/bah"));
        }

        [Fact]
        public void IsBaseOfAllowsForAnyBase()
        {
            var glob = new Glob(".cs", "c:/foo/bah/baz");
            Assert.True(glob.IsBaseOf("c:/foo/bah/baz/test"));
        }

        [Fact]
        public void IsBaseOfHasEndingSeperator()
        {
            var glob = new Glob(".cs", "c:/foo/bah/baz");
            Assert.False(glob.IsBaseOf("c:/foo/bah/baza"));
        }

        [Fact]
        public void RootMustBeValidUri()
        {
            Assert.Throws<UriFormatException>(() => new Glob(".cs", "foo"));
            Assert.Throws<UriFormatException>(() => new Glob(".cs", "foo/baz/bah"));
        }
    }
}