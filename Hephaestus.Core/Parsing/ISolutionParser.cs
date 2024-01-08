using Hephaestus.Core.Domain;

namespace Hephaestus.Core.Parsing
{
    public interface ISolutionParser
    {
        Solution Parse(string filePath, string fileContent);
    }
}
