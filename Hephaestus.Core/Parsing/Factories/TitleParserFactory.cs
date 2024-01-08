using System;
using Hephaestus.Core.Domain;
using Hephaestus.Core.Parsing.Legacy;
using Hephaestus.Core.Parsing.Sdk;

namespace Hephaestus.Core.Parsing.Factories
{
    public class TitleParserFactory : ITitleParserFactory
    {
        public ITitleParser Create(ProjectFormat format)
        {
            return format switch
            {
                ProjectFormat.Sdk => new SdkTitleParser(),
                ProjectFormat.Framework => new LegacyTitleParser(),
                _ => throw new ArgumentException(null, nameof(format)),
            };
        }
    }
}
