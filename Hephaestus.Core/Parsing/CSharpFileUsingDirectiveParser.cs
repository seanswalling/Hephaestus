using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Hephaestus.Core.Domain;

namespace Hephaestus.Core.Parsing
{
    public class CSharpFileUsingDirectiveParser : IUsingDirectiveParser
    {
        private static readonly RegexOptions _options = RegexOptions.Compiled;
        private static readonly Regex _normalForm = new(@"(?:;*\s*)(?:using\s*){1}(?:static\s*)*(?<using>(?:\w+\s*\.\s*)*\w+){1}\s*;", _options);
        private static readonly Regex _aliasForm = new(@"(?:;*\s*)(?:using\s*){1}(?:(?<alias>\w+)\s*={1}\s*)(?<using>(?:\w+\s*\.\s*)*\w+){1}\s*;", _options);

        public IEnumerable<CSharpUsing> ParseUsingDirectives(string input)
        {
            ArgumentNullException.ThrowIfNull(input, nameof(input));

            var namespaces = _aliasForm.Matches(input).Concat(_normalForm.Matches(input));

            return namespaces.Select(x => new CSharpUsing(new CSharpNamespace(x.Groups["using"].Value)));
        }
    }
}
