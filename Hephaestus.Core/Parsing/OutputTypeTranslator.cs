using System;
using Hephaestus.Core.Domain;

namespace Hephaestus.Core.Parsing
{
    public class OutputTypeTranslator : IOutputTypeTranslator
    {
        public OutputType Translate(string? input)
        {
            return string.IsNullOrWhiteSpace(input) ? OutputType.Library : TranslateInner(input);
        }

        public string Translate(OutputType input)
        {
            return input switch
            {
                OutputType.Library => "Library",
                OutputType.Exe => "Exe",
                OutputType.Module => "Module",
                OutputType.Winexe => "WinExe",
                _ => throw new ArgumentOutOfRangeException(nameof(input), $"{input}")
            };
        }

        private OutputType TranslateInner(string value)
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
