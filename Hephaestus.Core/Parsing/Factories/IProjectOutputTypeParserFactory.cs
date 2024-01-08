using Hephaestus.Core.Domain;

namespace Hephaestus.Core.Parsing.Factories
{
    public interface IProjectOutputTypeParserFactory
    {
        IProjectOutputTypeParser Create(ProjectFormat format);
    }
}
