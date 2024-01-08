using System;
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

            File.WriteAllText(GetCacheLocation(), JsonSerializer.Serialize(knownRepositories, options));
        }

        public static IEnumerable<KnownRepository> Load()
        {
            if (!File.Exists(GetCacheLocation()))
                return Enumerable.Empty<KnownRepository>();

            var result = JsonSerializer.Deserialize<IEnumerable<KnownRepository>>(File.ReadAllText(GetCacheLocation()));
            return result ?? Enumerable.Empty<KnownRepository>();
        }

        public static string GetCacheLocation()
        {
            var folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Hephaestus");
            return Path.Combine(folder, $"known-repositories.json");
        }

        public static bool Exists(KnownRepository kr)
        {
            return Path.Exists(kr.Path);
        }
    }
}
