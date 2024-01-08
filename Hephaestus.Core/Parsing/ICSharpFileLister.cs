using System.Collections.Generic;

namespace Hephaestus.Core.Parsing
{
    public interface ICSharpFileLister
    {
        IDictionary<string, string> ListFiles();
    }
}
