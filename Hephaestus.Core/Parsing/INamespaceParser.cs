using Hephaestus.Core.Domain;

namespace Hephaestus.Core.Parsing
{
    public interface INamespaceParser
    {
        CSharpNamespace ParseNamespace(string input);
    }
}
