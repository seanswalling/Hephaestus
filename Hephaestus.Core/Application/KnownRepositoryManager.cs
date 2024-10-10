using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Hephaestus.Core.Application
{
    public class KnownRepositoryManager(string _applicationRoot)
    {
        public void Save(IEnumerable<KnownRepository> knownRepositories)
        {
            var options = new JsonSerializerOptions()
            {
                WriteIndented = true,
            };

            File.WriteAllText(GetCacheLocation(), JsonSerializer.Serialize(knownRepositories, options));
        }

        public IEnumerable<KnownRepository> Load()
        {
            if (!File.Exists(GetCacheLocation()))
                return [];

            var result = JsonSerializer.Deserialize<IEnumerable<KnownRepository>>(File.ReadAllText(GetCacheLocation()));
            return result ?? [];
        }

        public string GetCacheLocation()
        {
            return Path.Combine(_applicationRoot, $"known-repositories.json");
        }

        public static bool Exists(KnownRepository kr)
        {
            return Path.Exists(kr.Path);
        }
    }
}
