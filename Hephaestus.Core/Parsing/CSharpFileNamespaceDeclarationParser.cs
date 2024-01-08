using System;
using System.Linq;
using System.Text.RegularExpressions;
using Hephaestus.Core.Domain;

namespace Hephaestus.Core.Parsing
{
    public class CSharpFileNamespaceDeclarationParser : INamespaceParser
    {
        private static readonly RegexOptions _options = RegexOptions.Compiled;

        /*
         * first 0 or more ;
         * then 0 or more whitespace
         * then exactly "namespace "
         * then the capture for the namespace
         * then 0 or more whitespace
         * then exactly "{" or exactly ";"
         */
        private static readonly Regex _normalForm = new(@"(?:;*\s*)namespace (?<namespace>(?:\w+\.)*\w+){1}\s*{", _options);
        private static readonly Regex _fileScoped = new(@"(?:;*\s*)namespace (?<namespace>(?:\w+\.)*\w+){1}\s*;", _options);

        public CSharpNamespace ParseNamespace(string fileContent)
        {
            ArgumentNullException.ThrowIfNull(fileContent, nameof(fileContent));

            if (_fileScoped.IsMatch(fileContent))
            {
                var matches = _fileScoped.Matches(fileContent);

                if (matches.Count > 1)
                {
                    throw new ArgumentException("Cannot have more than 1 File Scoped Namespace");
                }

                var value = matches.Single().Groups["namespace"].Value;

                return new CSharpNamespace(value);
            }

            if (_normalForm.IsMatch(fileContent))
            {
                var matches = _normalForm.Matches(fileContent);
                //currently only support the basic form, unsure how to support nesting.
                var value = matches[0].Groups["namespace"].Value;

                return new CSharpNamespace(value);
            }

            return new CSharpNamespace(string.Empty);
        }
    }
}
