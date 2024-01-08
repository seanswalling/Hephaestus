using System;
using Hephaestus.Core.Domain;
using Xunit;

namespace Hephaestus.Core.Tests.Domain
{
    public class ProjectReferenceTests
    {
        [Fact]
        public void RelativePathRequired()
        {
            Assert.Throws<ArgumentNullException>(() => new ProjectReference(null));
        }

        [Fact]
        public void SameRelativePathsAreEqual()
        {
            Assert.True(
                new ProjectReference("Foo/Bar/Baz").Equals(
                new ProjectReference("Foo/Bar/Baz")));

            Assert.Equal(
                new ProjectReference("Foo/Bar/Baz"),
                new ProjectReference("Foo/Bar/Baz")
                );

            Assert.True(
                new ProjectReference("Foo/Bar/Baz").Equals(
                new ProjectReference("foo/bar/baz")));

            Assert.Equal(
                new ProjectReference("Foo/Bar/Baz"),
                new ProjectReference("foo/bar/baz")
                );
        }

        [Fact]
        public void SameRelativePathsHaveSameHashCode()
        {
            Assert.Equal(
                new ProjectReference("Foo/Bar/Baz").GetHashCode(),
                new ProjectReference("Foo/Bar/Baz").GetHashCode()
                );
            Assert.Equal(
                new ProjectReference("Foo/Bar/Baz").GetHashCode(),
                new ProjectReference("foo/bar/baz").GetHashCode()
                );
        }

        [Fact]
        public void DifferentRelativePathsAreNotEqual()
        {
            Assert.False(
                new ProjectReference("Foo/Bar/Baz").Equals(
                new ProjectReference("Baz/Bar/Foo")));

            Assert.NotEqual(
                new ProjectReference("Foo/Bar/Baz"),
                new ProjectReference("Baz/Bar/Foo")
                );
        }

        [Fact]
        public void DifferentRelativePathsHaveDifferentHasCodes()
        {
            Assert.NotEqual(
                new ProjectReference("Foo/Bar/Baz").GetHashCode(),
                new ProjectReference("Baz/Bar/Foo").GetHashCode()
                );
        }

        private class SomeTestClass
        {
            public string RelativePath;
        }

        [Fact]
        public void ProjectReferenceIsNotEqualToDifferentType()
        {
            Assert.False(
                new ProjectReference("Foo/Bar/Baz").Equals(
                new SomeTestClass
                {
                    RelativePath = "Foo/Bar/Baz"
                }));

            Assert.False(
                new ProjectReference("Foo/Bar/Baz").Equals(
                new PackageReference("Test", "1.0")));
        }

        [Fact]
        public void ProjectReferenceIsEqualToProjectReferenceAsObject()
        {
            var unknownProj = (object)new ProjectReference("Foo/Bar/Baz");

            Assert.True(new ProjectReference("Foo/Bar/Baz").Equals(unknownProj));
        }

        [Fact]
        public void IsDirectIsTrueByDefault()
        {
            Assert.True(new ProjectReference("Foo/Bar/Baz").IsDirect);
        }
    }
}
