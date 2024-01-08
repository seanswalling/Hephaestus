using Hephaestus.Core.Domain;

namespace Hephaestus.Core.Parsing.Factories
{
    public interface IProjectFrameworkParserFactory
    {
        IProjectFrameworkParser Create(ProjectFormat format);
    }
}
