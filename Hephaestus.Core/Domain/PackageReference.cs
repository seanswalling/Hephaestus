using System;

namespace Hephaestus.Core.Domain
{
    public class PackageReference : IReference, IEquatable<PackageReference>
    {
        public readonly string Version;
        public readonly string Id;

        public bool IsDirect { get; set; }

        public PackageReference(string id, string version)
        {
            ArgumentNullException.ThrowIfNull(id, nameof(id));
            ArgumentNullException.ThrowIfNull(version, nameof(version));
            Version = version;
            Id = id;
            IsDirect = true;
        }

        public bool Equals(PackageReference? other)
        {
            if (other is null) return false;
            return other.Version.Equals(Version, StringComparison.OrdinalIgnoreCase) &&
                other.Id.Equals(Id, StringComparison.OrdinalIgnoreCase);
        }

        public override int GetHashCode()
        {
            return StringComparer.OrdinalIgnoreCase.GetHashCode(Id) ^
                 StringComparer.OrdinalIgnoreCase.GetHashCode(Version);
        }

        public override bool Equals(object? obj)
        {
            if (obj is not PackageReference) return false;
            return Equals(obj as PackageReference);
        }
    }
}
