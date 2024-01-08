using System;

namespace Hephaestus.Core.Domain
{
    public class CSharpNamespace
    {
        public string Value { get; set; }
        public CSharpNamespace(string value)
        {
            ArgumentNullException.ThrowIfNull(value);
            Value = value;
        }

        public override bool Equals(object? obj)
        {
            return obj is CSharpNamespace ns && Equals(ns);
        }

        public bool Equals(CSharpNamespace? other)
        {
            return other != null && other.Value.Equals(Value, StringComparison.OrdinalIgnoreCase);
        }

        public override int GetHashCode()
        {
            return StringComparer.OrdinalIgnoreCase.GetHashCode(Value);
        }
    }
}
