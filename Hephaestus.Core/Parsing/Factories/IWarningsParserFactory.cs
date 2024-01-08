using Hephaestus.Core.Domain;

namespace Hephaestus.Core.Parsing.Factories
{
    public interface IWarningsParserFactory
    {
        IWarningsParser Create(ProjectFormat format);
    }


}
