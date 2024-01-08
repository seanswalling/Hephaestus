using System;
using Hephaestus.Core.Domain;
using Hephaestus.Core.Parsing.Legacy;
using Hephaestus.Core.Parsing.Sdk;

namespace Hephaestus.Core.Parsing.Factories
{
    public class WarningsParserFactory : IWarningsParserFactory
    {
        public IWarningsParser Create(ProjectFormat format)
        {
            return format switch
            {
                ProjectFormat.Sdk => new SdkWarningParser(),
                ProjectFormat.Framework => new LegacyWarningParser(),
                _ => throw new ArgumentException(null, nameof(format)),
            };
        }
    }
}
