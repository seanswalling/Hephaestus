using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace Hephaestus.Core.Application
{
    public static class KnownRepositoryManager
    {
        public static void Save(IEnumerable<KnownRepository> knownRepositories)
        {
            var options = new JsonSerializerOptions()
            {
                WriteIndented = true,
            };

            File.WriteAllText("known-repositories.json", JsonSerializer.Serialize(knownRepositories, options));
        }

        public static IEnumerable<KnownRepository> Load()
        {
            if (!File.Exists("known-repositories.json"))
                return Enumerable.Empty<KnownRepository>();

            var result = JsonSerializer.Deserialize<IEnumerable<KnownRepository>>(File.ReadAllText("known-repositories.json"));
            return result ?? Enumerable.Empty<KnownRepository>();
        }

        public static bool Exists(KnownRepository kr)
        {
            return Path.Exists(kr.Path);
        }
    }
}
