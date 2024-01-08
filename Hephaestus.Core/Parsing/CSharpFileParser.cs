using System;
using Hephaestus.Core.Domain;

namespace Hephaestus.Core.Parsing
{
    public class CSharpFileParser : ICSharpFileParser
    {
        private readonly INamespaceParser _namespaceDeclarationParser;
        private readonly IUsingDirectiveParser _usingDirectiveParser;

        public CSharpFileParser(
            INamespaceParser namespaceDeclarationParser,
            IUsingDirectiveParser usingDirectiveParser)
        {
            _namespaceDeclarationParser = namespaceDeclarationParser;
            _usingDirectiveParser = usingDirectiveParser;
        }

        public CSharpFile ParseFile(string filePath, string fileContent)
        {
            ArgumentNullException.ThrowIfNull(fileContent, nameof(fileContent));
            ArgumentNullException.ThrowIfNull(filePath, nameof(filePath));

            var usings = _usingDirectiveParser.ParseUsingDirectives(fileContent);
            var nspace = _namespaceDeclarationParser.ParseNamespace(fileContent);

            return new CSharpFile(filePath, nspace, usings);
        }
    }
}
