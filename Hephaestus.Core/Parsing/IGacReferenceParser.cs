using System.Collections.Generic;
using Hephaestus.Core.Domain;

namespace Hephaestus.Core.Parsing
{
    public interface IGacReferenceParser
    {
        IEnumerable<GacReference> Parse();
    }
}
