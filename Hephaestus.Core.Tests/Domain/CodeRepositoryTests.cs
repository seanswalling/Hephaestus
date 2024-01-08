using System;
using Hephaestus.Core.Domain;
using Xunit;

namespace Hephaestus.Core.Tests.Domain
{
    public class CodeRepositoryTests
    {
        [Fact]
        public void NameCannotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => new CodeRepository(null, Array.Empty<Solution>()));
        }

        [Fact]
        public void SolutionsCannotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => new Solution("c:\\Foo", null));
        }
    }
}
