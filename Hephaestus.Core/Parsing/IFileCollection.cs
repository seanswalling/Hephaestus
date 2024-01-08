using System.Collections.Generic;
using Hephaestus.Core.Domain;

namespace Hephaestus.Core.Parsing
{
    public interface IFileCollection
    {
        IDictionary<string, string> GetFiles(Glob glob);
        IDictionary<string, string> GetFiles(string projectPath);
        IDictionary<string, string> GetFiles(IEnumerable<string> filePaths);
        string GetContent(string filePath);
        bool Exists(string filePath);
    }
}
