using System.Collections.Generic;
using System.IO;
using System.Linq;
using Hephaestus.Core.Domain;
using Hephaestus.Core.Parsing;

namespace Hephaestus.Core.Version1.Parsing
{
    public class CSharpFileFinder
    {
        private readonly IFileProvider _fileProvider;
        private readonly ICSharpFileParser _fileParser;

        public CSharpFileFinder(IFileProvider fileProvider, ICSharpFileParser fileParser)
        {
            _fileProvider = fileProvider;
            _fileParser = fileParser;
        }

        public IEnumerable<CSharpFile> FindFiles(string root)
        {
            return _fileProvider.QueryByExtension(".cs")
                .Where(x => x.Key.Contains(Path.GetDirectoryName(root)!))
                .Select(x => _fileParser.ParseFile(x.Key, x.Value));
        }
    }
}
