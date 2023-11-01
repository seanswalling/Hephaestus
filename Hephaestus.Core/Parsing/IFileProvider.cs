using System.Collections.Generic;

namespace Hephaestus.Core.Parsing
{
    internal interface IFileProvider
    {
        bool HasFile(string path);
        string GetFile(string path);
        IEnumerable<KeyValuePair<string, string>> QueryByExtension(string extension);
    }
}