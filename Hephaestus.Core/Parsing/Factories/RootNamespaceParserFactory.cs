using System;
using Hephaestus.Core.Domain;
using Hephaestus.Core.Parsing.Legacy;
using Hephaestus.Core.Parsing.Sdk;

namespace Hephaestus.Core.Parsing
{
    public class RootNamespaceParserFactory : IRootNamespaceParserFactory
    {
        public IRootNamespaceParser Create(ProjectFormat format)
        {
            return format switch
            {
                ProjectFormat.Sdk => new SdkRootNamespaceParser(),
                ProjectFormat.Framework => new LegacyRootNamespaceParser(),
                _ => throw new ArgumentException(null, nameof(format)),
            };
        }
    }
}
