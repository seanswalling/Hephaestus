using System.Collections.Generic;
using Hephaestus.Core.Domain;

namespace Hephaestus.Core.Parsing
{
    public class ReferenceParser : IReferenceParser
    {
        private IPackageReferenceParser _packageReferenceParser;
        private IProjectReferenceParser _projectReferenceParser;
        private IGacReferenceParser _gacReferenceParser;

        public ReferenceParser(IPackageReferenceParser packageReferenceParser, IProjectReferenceParser projectReferenceParser, IGacReferenceParser gacReferenceParser)
        {
            _packageReferenceParser = packageReferenceParser;
            _projectReferenceParser = projectReferenceParser;
            _gacReferenceParser = gacReferenceParser;
        }

        public IEnumerable<IReference> Parse()
        {
            foreach (var reference in _projectReferenceParser.Parse())
            {
                yield return reference;
            }

            foreach (var reference in _packageReferenceParser.Parse())
            {
                yield return reference;
            }

            foreach (var reference in _gacReferenceParser.Parse())
            {
                yield return reference;
            }
        }
    }
}
