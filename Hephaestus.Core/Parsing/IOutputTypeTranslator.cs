using Hephaestus.Core.Domain;

namespace Hephaestus.Core.Parsing
{
    public interface IOutputTypeTranslator
    {
        OutputType Translate(string? input);
    }
}
