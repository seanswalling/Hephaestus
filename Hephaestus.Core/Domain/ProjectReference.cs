using System;

namespace Hephaestus.Core.Domain
{
    public class ProjectReference : IReference, IEquatable<ProjectReference>
    {
        public readonly string RelativePath;

        public bool IsDirect { get; set; }

        public ProjectReference(string relativePath)
        {
            ArgumentNullException.ThrowIfNull(relativePath, nameof(relativePath));
            RelativePath = relativePath;
            IsDirect = true;
        }

        public bool Equals(ProjectReference? other)
        {
            if (other is null) return false;
            return other.RelativePath.Equals(RelativePath, StringComparison.OrdinalIgnoreCase);
        }

        public override int GetHashCode()
        {
            return StringComparer.OrdinalIgnoreCase.GetHashCode(RelativePath);
        }

        public override bool Equals(object? obj)
        {
            if (obj is not ProjectReference) return false;
            return Equals(obj as ProjectReference);
        }
    }
}
