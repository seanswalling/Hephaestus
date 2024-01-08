using System;
using Hephaestus.Core.Domain;
using Xunit;

namespace Hephaestus.Core.Tests.Domain
{
    public class PackageReferenceTests
    {
        [Fact]
        public void IdIsRequired()
        {
            Assert.Throws<ArgumentNullException>(() => new PackageReference(null, "1.0"));
        }

        [Fact]
        public void VersionIsRequired()
        {
            Assert.Throws<ArgumentNullException>(() => new PackageReference("SomeId", null));
        }

        [Fact]
        public void SameIdAndVersionAreEqual()
        {
            Assert.True(
                new PackageReference("SomeId", "1.0").Equals(
                new PackageReference("SomeId", "1.0")));

            Assert.Equal(
                new PackageReference("SomeId", "1.0"),
                new PackageReference("SomeId", "1.0")
                );

            Assert.True(
                new PackageReference("SomeId", "1.0").Equals(
                new PackageReference("someid", "1.0")));

            Assert.Equal(
                new PackageReference("SomeId", "1.0"),
                new PackageReference("someid", "1.0")
                );
        }

        [Fact]
        public void SameIdAndVersionHaveSameHashCode()
        {
            Assert.Equal(
                new PackageReference("SomeId", "1.0").GetHashCode(),
                new PackageReference("SomeId", "1.0").GetHashCode()
                );
            Assert.Equal(
                new PackageReference("SomeId", "1.0").GetHashCode(),
                new PackageReference("someid", "1.0").GetHashCode()
                );
        }

        [Fact]
        public void DifferentIdsAreNotEqual()
        {
            Assert.False(
                new PackageReference("SomeId", "1.0").Equals(
                new PackageReference("SomeOtherId", "1.0")));

            Assert.NotEqual(
                new PackageReference("SomeId", "1.0"),
               new PackageReference("SomeOtherId", "1.0")
                );
        }

        [Fact]
        public void DifferentVersionsAreNotEqual()
        {
            Assert.False(
                new PackageReference("SomeId", "1.0").Equals(
                new PackageReference("SomeId", "2.0")));

            Assert.NotEqual(
                new PackageReference("SomeId", "1.0"),
                new PackageReference("SomeId", "2.0")
                );
        }

        [Fact]
        public void DifferentIdsHaveDifferentHasCodes()
        {
            Assert.NotEqual(
                new PackageReference("SomeId", "1.0").GetHashCode(),
                new PackageReference("SomeOtherId", "1.0").GetHashCode()
                );
        }

        [Fact]
        public void DifferentVersionsHaveDifferentHasCodes()
        {
            Assert.NotEqual(
                new PackageReference("SomeId", "1.0").GetHashCode(),
                new PackageReference("SomeId", "2.0").GetHashCode()
                );
        }

        private class SomeTestClass
        {
            public string Version;
            public string Id;
        }

        [Fact]
        public void ProjectReferenceIsNotEqualToDifferentType()
        {
            Assert.False(
                new PackageReference("SomeId", "1.0").Equals(
                new SomeTestClass
                {
                    Version = "SomeId",
                    Id = "1.0"
                }));

            Assert.False(
                new PackageReference("Test", "1.0").Equals(
                new ProjectReference("Foo/Bar/Baz")));
        }

        [Fact]
        public void ProjectReferenceIsEqualToProjectReferenceAsObject()
        {
            var unknownProj = (object)new PackageReference("Test", "1.0");

            Assert.True(new PackageReference("Test", "1.0").Equals(unknownProj));
        }

        [Fact]
        public void IsDirectIsTrueByDefault()
        {
            Assert.True(new PackageReference("Test", "1.0").IsDirect);
        }
    }
}
