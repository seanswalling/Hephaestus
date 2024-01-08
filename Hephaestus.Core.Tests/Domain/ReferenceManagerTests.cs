using System.Collections.Generic;
using System.Linq;
using Hephaestus.Core.Domain;
using Xunit;

namespace Hephaestus.Core.Tests.Domain
{
    public class ReferenceManagerTests
    {
        [Fact]
        public void CanAddProjectReference()
        {
            var manager = new ReferenceManager();
            var reference = new ProjectReference("Foo/Bar/Baz");
            manager.Add(reference);
            Assert.Contains(reference, manager.ProjectReferences);
        }

        [Fact]
        public void CanAddPackageReference()
        {
            var manager = new ReferenceManager();
            var reference = new PackageReference("SomeId", "1.0");
            manager.Add(reference);
            Assert.Contains(reference, manager.PackageReferences);
        }

        [Fact]
        public void CannotAddDuplicateProjects()
        {
            var manager = new ReferenceManager();
            var reference = new ProjectReference("Foo/Bar/Baz");
            var reference2 = new ProjectReference("Foo/Bar/Baz");
            manager.Add(reference);
            manager.Add(reference);
            manager.Add(reference2);
            Assert.Contains(reference, manager.ProjectReferences);
            Assert.Single(manager.ProjectReferences);
            Assert.NotEqual(3, manager.ProjectReferences.Count);
        }

        [Fact]
        public void CannotAddDuplicatePackages()
        {
            var manager = new ReferenceManager();
            var reference = new PackageReference("SomeId", "1.0");
            var reference2 = new PackageReference("SomeId", "1.0");
            manager.Add(reference);
            manager.Add(reference);
            manager.Add(reference2);
            Assert.Contains(reference, manager.PackageReferences);
            Assert.Single(manager.PackageReferences);
            Assert.NotEqual(3, manager.PackageReferences.Count);
        }

        [Fact]
        public void CanMakeProjectReferenceTransient()
        {
            var manager = new ReferenceManager();
            var reference = new ProjectReference("Foo/Bar/Baz");
            manager.Add(reference);
            Assert.True(manager.ProjectReferences.Single().IsDirect);
            manager.MakeTransient(reference);
            Assert.False(manager.ProjectReferences.Single().IsDirect);
        }

        [Fact]
        public void CanMakePackageReferenceTransient()
        {
            var manager = new ReferenceManager();
            var reference = new PackageReference("SomeId", "1.0");
            manager.Add(reference);
            Assert.True(manager.PackageReferences.Single().IsDirect);
            manager.MakeTransient(reference);
            Assert.False(manager.PackageReferences.Single().IsDirect);
        }

        [Fact]
        public void CanAddRangeOfReferences()
        {
            List<IReference> references = new();
            var project1 = new ProjectReference("Foo/Bah");
            var project2 = new ProjectReference("Baz/Foo");
            var package1 = new PackageReference("ThirdParty.Foo", "1.0");
            var package2 = new PackageReference("ThirdParty.Bah", "1.2");


            references.Add(project1);
            references.Add(package1);
            references.Add(project2);
            references.Add(package2);
            //Add some dupes as well
            references.Add(package1);
            references.Add(project1);

            var manager = new ReferenceManager();
            manager.AddRange(references);

            Assert.Equal(2, manager.ProjectReferences.Count);
            Assert.Equal(2, manager.PackageReferences.Count);

            Assert.Contains(project1, manager.ProjectReferences);
            Assert.Contains(project2, manager.ProjectReferences);
            Assert.Contains(package1, manager.PackageReferences);
            Assert.Contains(package2, manager.PackageReferences);
        }
    }
}
