using System.Collections.Generic;

namespace Hephaestus.Core.Domain
{
    public interface ITestDependencyProvider
    {
        IEnumerable<PackageReference> ProvidePackageReferences(Project project);
        IEnumerable<ProjectReference> ProvideProjectReferences(Project project);
    }
}