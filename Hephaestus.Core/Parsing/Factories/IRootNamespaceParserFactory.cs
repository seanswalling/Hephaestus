using Hephaestus.Core.Domain;

namespace Hephaestus.Core.Parsing
{
    public interface IRootNamespaceParserFactory
    {
        IRootNamespaceParser Create(ProjectFormat format);
    }
}
