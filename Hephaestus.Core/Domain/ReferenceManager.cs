using System;
using System.Collections.Generic;

namespace Hephaestus.Core.Domain
{
    public class ReferenceManager
    {
        public HashSet<ProjectReference> ProjectReferences;
        public HashSet<PackageReference> PackageReferences;
        public HashSet<GacReference> GacReferences;

        public ReferenceManager()
        {
            ProjectReferences = [];
            PackageReferences = [];
            GacReferences = [];
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

        public void Upgrade(PackageReference oldReference, PackageReference upgradedReference)
        {
            if (!PackageReferences.Contains(oldReference))
                return;

            PackageReferences.Remove(oldReference);
            PackageReferences.Add(upgradedReference);
        }

        public void Remove(PackageReference oldReference)
        {
            if (!PackageReferences.Contains(oldReference))
                return;

            PackageReferences.Remove(oldReference);
        }

        private void Add(GacReference reference)
        {
            if (!GacReferences.Contains(reference))
            {
                GacReferences.Add(reference);
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


            if (reference is GacReference gac)
            {
                Add(gac);
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
