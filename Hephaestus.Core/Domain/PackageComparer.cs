using System;
using System.Collections.Generic;

namespace Hephaestus.Core.Domain
{
    public class PackageComparer : IEqualityComparer<PackageReference>
    {
        public bool Equals(PackageReference x, PackageReference y)
        {
            return x.Name == y.Name && x.Version == y.Version;
        }

        public int GetHashCode(PackageReference obj)
        {
            return HashCode.Combine(obj.Name, obj.Version);
        }
    }
}