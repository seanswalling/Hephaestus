using System;

namespace Hephaestus.Core.Domain
{
    public class GacReference : IReference, IEquatable<GacReference>
    {
        public readonly string Id;
        public bool IsDirect { get => true; set => Noop(); }
        public GacReference(string id)
        {
            ArgumentNullException.ThrowIfNull(id, nameof(id));
            Id = id;
        }

        private void Noop() { }

        public bool Equals(GacReference? other)
        {
            if (other is null) return false;
            return other.Id.Equals(Id, StringComparison.OrdinalIgnoreCase);
        }

        public override int GetHashCode()
        {
            return StringComparer.OrdinalIgnoreCase.GetHashCode(Id);
        }

        public override bool Equals(object? obj)
        {
            if (obj is not GacReference) return false;
            return Equals(obj as GacReference);
        }
    }
}
