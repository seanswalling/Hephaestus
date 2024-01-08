using System;
using Hephaestus.Core.Domain;
using Hephaestus.Core.Parsing.Legacy;
using Hephaestus.Core.Parsing.Sdk;

namespace Hephaestus.Core.Parsing.Factories
{
    public class EmbeddedResourceParserFactory : IEmbeddedResourceParserFactory
    {
        public IEmbeddedResourceParser Create(ProjectFormat format)
        {
            return format switch
            {
                ProjectFormat.Sdk => new SdkEmbeddedResourceParser(),
                ProjectFormat.Framework => new LegacyEmbeddedResourceParser(),
                _ => throw new ArgumentException(null, nameof(format)),
            };
        }
    }
}
