using Hephaestus.Core.Domain;

namespace Hephaestus.Core.Parsing.Factories
{
    public interface IEmbeddedResourceParserFactory
    {
        IEmbeddedResourceParser Create(ProjectFormat format);
    }
}
