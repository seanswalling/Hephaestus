using System.Collections.Generic;

namespace Hephaestus.Core.Version1.Domain
{

    public class ProjectReferenceV1Comparer : IEqualityComparer<ProjectReferenceV1>
    {
        public bool Equals(ProjectReferenceV1 x, ProjectReferenceV1 y)
        {
            return x.RelativePath == y.RelativePath;
        }

        public int GetHashCode(ProjectReferenceV1 obj)
        {
            return obj.RelativePath.GetHashCode();
        }
    }
}