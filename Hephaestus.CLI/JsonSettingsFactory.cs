using System.Text.Json;
using Hephaestus.Core.Domain;

namespace Hephaestus.CLI
{
    public static class JsonSettingsFactory
    {
        private static JsonSerializerOptions? _options;

        public static JsonSerializerOptions Build()
        {
            if (_options is null)
            {
                _options = new JsonSerializerOptions
                {
                    IncludeFields = true,
                    WriteIndented = true
                };
                _options.Converters.Add(new GroupingConverter<string, PackageReference>());
                _options.Converters.Add(new GroupingConverter<bool, PackageReferenceAndFramework>());
            }
            return _options;
        }
    }
}