using System;
using System.Collections.Generic;

namespace Hephaestus.Core.Version1.Domain
{
    public class PackageReferenceV1Comparer : IEqualityComparer<PackageReferenceV1>
    {
        public bool Equals(PackageReferenceV1 x, PackageReferenceV1 y)
        {
            return x.Name == y.Name && x.Version == y.Version;
        }

        public int GetHashCode(PackageReferenceV1 obj)
        {
            return HashCode.Combine(obj.Name, obj.Version);
        }
    }
}