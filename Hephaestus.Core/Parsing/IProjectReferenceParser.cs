using System.Collections.Generic;
using Hephaestus.Core.Domain;

namespace Hephaestus.Core.Parsing
{
    public interface IProjectReferenceParser
    {
        IEnumerable<ProjectReference> Parse();
    }
}
