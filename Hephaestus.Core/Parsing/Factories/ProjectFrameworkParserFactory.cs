using System;
using Hephaestus.Core.Domain;
using Hephaestus.Core.Parsing.Legacy;
using Hephaestus.Core.Parsing.Sdk;

namespace Hephaestus.Core.Parsing.Factories
{
    public class ProjectFrameworkParserFactory : IProjectFrameworkParserFactory
    {
        private readonly ITfmTranslator _translator;

        public ProjectFrameworkParserFactory(ITfmTranslator translator)
        {
            _translator = translator;
        }

        public IProjectFrameworkParser Create(ProjectFormat format)
        {
            return format switch
            {
                ProjectFormat.Sdk => new SdkProjectFrameworkParser(_translator),
                ProjectFormat.Framework => new LegacyProjectFrameworkParser(_translator),
                _ => throw new ArgumentException(null, nameof(format)),
            };
        }
    }
}
