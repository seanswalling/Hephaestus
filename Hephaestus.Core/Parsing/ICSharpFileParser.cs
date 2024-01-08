using Hephaestus.Core.Domain;

namespace Hephaestus.Core.Parsing
{
    public interface ICSharpFileParser
    {
        CSharpFile ParseFile(string filePath, string fileContent);
    }
}
