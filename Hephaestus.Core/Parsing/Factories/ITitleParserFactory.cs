using Hephaestus.Core.Domain;

namespace Hephaestus.Core.Parsing.Factories
{
    public interface ITitleParserFactory
    {
        ITitleParser Create(ProjectFormat format);
    }
}
