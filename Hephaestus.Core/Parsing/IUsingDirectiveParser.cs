using System.Collections.Generic;
using Hephaestus.Core.Domain;

namespace Hephaestus.Core.Parsing
{
    public interface IUsingDirectiveParser
    {
        IEnumerable<CSharpUsing> ParseUsingDirectives(string input);
    }
}
