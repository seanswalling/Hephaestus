using System.Collections.Generic;
using Hephaestus.Core.Version1.Domain;

namespace Hephaestus.Core.Domain
{
    public interface ITestDependencyProvider
    {
        IEnumerable<PackageReferenceV1> ProvidePackageReferences(ProjectV1 project);
        IEnumerable<ProjectReferenceV1> ProvideProjectReferences(ProjectV1 project);
    }
}