using System;
using Hephaestus.Core.Domain;
using Xunit;

namespace Hephaestus.Core.Tests.Domain
{
    public class SolutionTests
    {
        [Fact]
        public void NameCannotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => new Solution(null, Array.Empty<Project>()));
        }

        [Fact]
        public void ProjectsCannotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => new Solution("Foo.sln", null));
        }
    }
}
