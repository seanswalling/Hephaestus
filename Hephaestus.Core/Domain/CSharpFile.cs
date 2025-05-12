using System;
using System.Collections.Generic;

namespace Hephaestus.Core.Domain
{
    public class CSharpFile
    {
        private readonly string _filePath;
        public readonly List<CSharpUsing> UsingDirectives;
        public readonly CSharpNamespace NamespaceDeclaration;

        public CSharpFile(
            string filePath,
            CSharpNamespace namespaceDeclaration,
            IEnumerable<CSharpUsing> usingDirectives)
        {
            ArgumentNullException.ThrowIfNull(filePath, nameof(filePath));
            ArgumentNullException.ThrowIfNull(namespaceDeclaration, nameof(namespaceDeclaration));

            _filePath = filePath;
            UsingDirectives = usingDirectives != null ? [.. usingDirectives] : [];
            NamespaceDeclaration = namespaceDeclaration;
        }

        public override bool Equals(object? obj)
        {
            if (obj is CSharpFile file)
            {
                return Equals(file);
            }

            return false;
        }

        public bool Equals(CSharpFile file)
        {
            return _filePath.Equals(file._filePath, StringComparison.OrdinalIgnoreCase) &&
                NamespaceDeclaration.Equals(file.NamespaceDeclaration);
        }

        public override int GetHashCode()
        {
            return StringComparer.OrdinalIgnoreCase.GetHashCode(_filePath) ^
                NamespaceDeclaration.GetHashCode();
        }

        public void RemoveUsing(CSharpUsing usingDirective)
        {
            UsingDirectives.RemoveAll(ud => ud.Equals(usingDirective));
        }

        public void AddUsing(CSharpUsing usingDirective)
        {
            UsingDirectives.Add(usingDirective);
        }
    }
}
