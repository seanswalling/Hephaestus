using System;
using Hephaestus.Core.Domain;

namespace Hephaestus.Core.Parsing
{
    internal static class OutputTypeParser
    {

        internal static OutputType Parse(string? input)
        {
            return string.IsNullOrWhiteSpace(input) ? OutputType.Library : input.ToOutputType();
        }

        private static OutputType ToOutputType(this string value)
        {
            var val = value.ToLowerInvariant();

            var result = val switch
            {
                "library" => OutputType.Library,
                "exe" => OutputType.Exe,
                "module" => OutputType.Module,
                "winexe" => OutputType.Winexe,
                _ => throw new ArgumentOutOfRangeException(nameof(value), $"{value}")
            };

            return result;
        }
    }
}
