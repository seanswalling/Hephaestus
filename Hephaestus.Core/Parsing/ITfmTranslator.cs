using Hephaestus.Core.Domain;

namespace Hephaestus.Core.Parsing
{
    public interface ITfmTranslator
    {
        public Framework Translate(string? moniker);
    }
}
