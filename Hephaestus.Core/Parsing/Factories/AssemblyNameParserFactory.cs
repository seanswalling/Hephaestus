using System;
using Hephaestus.Core.Domain;
using Hephaestus.Core.Parsing.Legacy;
using Hephaestus.Core.Parsing.Sdk;

namespace Hephaestus.Core.Parsing.Factories
{
    public class AssemblyNameParserFactory : IAssemblyNameParserFactory
    {
        public IAssemblyNameParser Create(ProjectFormat format)
        {
            return format switch
            {
                ProjectFormat.Sdk => new SdkAssemblyNameParser(),
                ProjectFormat.Framework => new LegacyAssemblyNameParser(),
                _ => throw new ArgumentException(null, nameof(format)),
            };
        }
    }
}
