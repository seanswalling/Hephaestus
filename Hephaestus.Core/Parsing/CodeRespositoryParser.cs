using System.Linq;
using Hephaestus.Core.Domain;

namespace Hephaestus.Core.Parsing
{
    public class CodeRespositoryParser : ICodeRepositoryParser
    {
        private readonly ISolutionParser _solutionParser;
        private readonly IFileCollection _fileCollection;

        public CodeRespositoryParser(ISolutionParser solutionParser, IFileCollection fileCollection)
        {
            _solutionParser = solutionParser;
            _fileCollection = fileCollection;
        }

        public CodeRepository Parse(string name, string path)
        {
            var slns = _fileCollection.GetFiles(new Glob(".sln", path))
                .Select((kvp) => _solutionParser.Parse(kvp.Key, kvp.Value));

            return new CodeRepository(name, slns);
        }
    }
}
