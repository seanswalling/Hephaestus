using System.Collections.Generic;

namespace Hephaestus.Core.Domain
{
    public class ProjectComparer : IEqualityComparer<ProjectReference>
    {
        public bool Equals(ProjectReference x, ProjectReference y)
        {
            return x.RelativePath == y.RelativePath;
        }

        public int GetHashCode(ProjectReference obj)
        {
            return obj.RelativePath.GetHashCode();
        }
    }
}