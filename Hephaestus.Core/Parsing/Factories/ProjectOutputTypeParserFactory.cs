using System;
using Hephaestus.Core.Domain;
using Hephaestus.Core.Parsing.Legacy;
using Hephaestus.Core.Parsing.Sdk;

namespace Hephaestus.Core.Parsing.Factories
{
    public class ProjectOutputTypeParserFactory : IProjectOutputTypeParserFactory
    {
        private readonly IOutputTypeTranslator _outputTypeTranslator;

        public ProjectOutputTypeParserFactory(IOutputTypeTranslator outputTypeTranslator)
        {
            _outputTypeTranslator = outputTypeTranslator;
        }

        public IProjectOutputTypeParser Create(ProjectFormat format)
        {
            return format switch
            {
                ProjectFormat.Sdk => new SdkProjectOutputTypeParser(_outputTypeTranslator),
                ProjectFormat.Framework => new LegacyProjectOutputTypeParser(_outputTypeTranslator),
                _ => throw new ArgumentException(null, nameof(format)),
            };
        }
    }
}
