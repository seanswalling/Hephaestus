using System;

namespace Hephaestus.Core.Domain
{
    public class CSharpUsing
    {
        public CSharpNamespace Value { get; set; }

        public CSharpUsing(CSharpNamespace ns)
        {
            ArgumentNullException.ThrowIfNull(ns);
            Value = ns;
        }

        public override bool Equals(object? obj)
        {
            if (obj is CSharpUsing us)
            {
                return Equals(us);
            }

            if (obj is CSharpNamespace ns)
            {
                return Equals(ns);
            }

            return false;
        }

        public bool Equals(CSharpUsing? other)
        {
            return other != null && other.Value.Equals(Value);
        }

        public bool Equals(CSharpNamespace? other)
        {
            return other != null && other.Equals(Value);
        }

        public override int GetHashCode()
        {
            return StringComparer.OrdinalIgnoreCase.GetHashCode(Value);
        }
    }
}
