using System;
using System.Text.RegularExpressions;

namespace Hephaestus.Core.Domain
{
    public class TypeFormat : IEquatable<TypeFormat>
    {
        private static readonly RegexOptions _options = RegexOptions.Compiled | RegexOptions.IgnoreCase;
        private static readonly Regex _wellFormed = new(@"^([A-Za-z_]\w*(\.[A-Za-z_]\w*)*)?$", _options);
        public string Value { get; set; }
        public TypeFormat(string line)
        {
            ArgumentException.ThrowIfNullOrEmpty(line);
            if (!_wellFormed.IsMatch(line)) throw new ArgumentException("invalid format");
            Value = line;
        }

        public bool Equals(TypeFormat? other)
        {
            return other != null &&
                other.Value.Equals(Value, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object? obj)
        {
            return obj is TypeFormat format && Equals(format);
        }

        public override int GetHashCode()
        {
            return StringComparer.OrdinalIgnoreCase.GetHashCode(Value);
        }
    }
}
