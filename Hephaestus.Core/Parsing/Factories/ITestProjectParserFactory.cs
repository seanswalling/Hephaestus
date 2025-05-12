using Hephaestus.Core.Domain;

namespace Hephaestus.Core.Parsing.Factories
{
    public interface ITestProjectParserFactory
    {
        ITestProjectParser Create(ProjectFormat format);
    }
}
