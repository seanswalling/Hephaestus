using System.Collections.Generic;
using System.Linq;

namespace Hephaestus.Core.Version1.Domain
{
    public static class ReferenceListEx
    {
        public static void AddReference(this List<PackageReferenceV1> packages, PackageReferenceV1 package)
        {
            if (!packages.Contains(package, new PackageReferenceV1Comparer()))
                packages.Add(package);
        }

        public static void AddReference(this List<ProjectReferenceV1> projects, ProjectReferenceV1 project)
        {
            if (!projects.Contains(project, new ProjectReferenceV1Comparer()))
                projects.Add(project);
        }
    }
}