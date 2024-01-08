using Hephaestus.Core.Domain;

namespace Hephaestus.Core.Parsing.Factories
{
    public interface IAssemblyNameParserFactory
    {
        IAssemblyNameParser Create(ProjectFormat format);
    }
}
