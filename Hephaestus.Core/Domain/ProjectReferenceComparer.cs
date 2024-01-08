using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Hephaestus.Core.Domain
{
    public class ProjectReferenceComparer : IEqualityComparer<ProjectReference>
    {
        public bool Equals(ProjectReference? x, ProjectReference? y)
        {
            if (x == null || y == null) return false;
            return x.Equals(y);
        }

        public int GetHashCode([DisallowNull] ProjectReference obj)
        {
            return obj.GetHashCode();
        }
    }
}