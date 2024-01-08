using System.Collections.Generic;

namespace Hephaestus.Core.Parsing
{
    public interface IFileProvider
    {
        bool HasFile(string path);
        string GetFile(string path);
        IEnumerable<KeyValuePair<string, string>> QueryByExtension(string extension);
    }
}