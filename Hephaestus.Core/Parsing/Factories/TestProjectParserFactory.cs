using System;
using Hephaestus.Core.Domain;
using Hephaestus.Core.Parsing.Legacy;
using Hephaestus.Core.Parsing.Sdk;

namespace Hephaestus.Core.Parsing.Factories
{
    public class TestProjectParserFactory : ITestProjectParserFactory
    {
        public ITestProjectParser Create(ProjectFormat format)
        {
            return format switch
            {
                ProjectFormat.Unknown => throw new NotImplementedException(),
                ProjectFormat.Framework => new LegacyTestProjectParser(),
                ProjectFormat.Sdk => new SdkTestProjectParser(),
                _ => throw new ArgumentException(null, nameof(format)),
            };
        }
    }
}
