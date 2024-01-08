using Hephaestus.Core.Domain;

namespace Hephaestus.Core.Parsing
{
    public interface ICodeRepositoryParser
    {
        CodeRepository Parse(string name, string path);
    }
}
