using System.Collections.Generic;
using System.Linq;

namespace Hephaestus.Core.Domain
{
    public static class ReferenceListEx
    {
        public static void AddReference(this List<PackageReference> packages, PackageReference package)
        {
            if (!packages.Contains(package))
                packages.Add(package);
        }

        public static void AddReference(this List<ProjectReference> projects, ProjectReference project)
        {
            if (!projects.Contains(project, new ProjectComparer()))
                projects.Add(project);
        }
    }
}