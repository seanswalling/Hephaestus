using System;
using System.Collections.Generic;

namespace Hephaestus.Core.Domain
{
    public class ReferenceManager
    {
        public HashSet<ProjectReference> ProjectReferences;
        public HashSet<PackageReference> PackageReferences;

        public ReferenceManager()
        {
            ProjectReferences = [];
            PackageReferences = [];
        }

        private void Add(ProjectReference reference)
        {
            if (!ProjectReferences.Contains(reference))
            {
                ProjectReferences.Add(reference);
            }
        }

        private void Add(PackageReference reference)
        {
            if (!PackageReferences.Contains(reference))
            {
                PackageReferences.Add(reference);
            }
        }

        public void Add(IReference reference)
        {
            ArgumentNullException.ThrowIfNull(reference, nameof(reference));

            if (reference is ProjectReference project)
            {
                Add(project);
                return;
            }

            if (reference is PackageReference package)
            {
                Add(package);
                return;
            }
        }

        public void AddRange(IEnumerable<IReference> references)
        {
            foreach (var reference in references)
            {
                Add(reference);
            }
        }

        public void MakeTransient(ProjectReference reference)
        {
            if (ProjectReferences.TryGetValue(reference, out var storedValue))
            {
                storedValue.IsDirect = false;
            }
        }

        public void MakeTransient(PackageReference reference)
        {
            if (PackageReferences.TryGetValue(reference, out var storedValue))
            {
                storedValue.IsDirect = false;
            }
        }
    }
}
