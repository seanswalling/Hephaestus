using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace Hephaestus.Core.Application
{
    public class OwnershipCache
    {
        public string Name { get; init; }
        public FrozenDictionary<string, IEnumerable<string>> FileOwnership { get; init; }

        private OwnershipCache(string name, FrozenDictionary<string, IEnumerable<string>> fileOwnership)
        {
            Name = name;
            FileOwnership = fileOwnership;
        }

        public bool IsEmpty()
        {
            return FileOwnership.Count == 0;
        }

        public void Save()
        {
            var options = new JsonSerializerOptions()
            {
                WriteIndented = true,
            };

            File.WriteAllText($"hephaestus-{Name}-sdk-ownership-cache.json", JsonSerializer.Serialize(FileOwnership, options));
        }

        public static OwnershipCache Load(string name)
        {
            var cache = $"hephaestus-{name}-sdk-ownership-cache.json";

            if (!File.Exists(cache))
                throw new FileNotFoundException(cache);

            var content = File.ReadAllText(cache);

            if (content == null)
                throw new InvalidDataException("Cache Content Missing");

            var fileOwnership = JsonSerializer.Deserialize<Dictionary<string, IEnumerable<string>>>(content)!.ToFrozenDictionary(StringComparer.OrdinalIgnoreCase);

            return new OwnershipCache(name, fileOwnership);
        }

        public static OwnershipCache Empty()
        {
            return new OwnershipCache(string.Empty, FrozenDictionary<string, IEnumerable<string>>.Empty);
        }

        public static bool Exists(string name)
        {
            return File.Exists($"hephaestus-{name}-sdk-ownership-cache.json");
        }

        public static OwnershipCache Build(string name, IEnumerable<string> fileCollection)
        {
            var result = new Dictionary<string, IEnumerable<string>>();

            foreach (var file in fileCollection.Where(x => x.EndsWith(".csproj", StringComparison.OrdinalIgnoreCase)))
            {
                var parent = Directory.GetParent(file).FullName + Path.DirectorySeparatorChar;
                var csfiles = fileCollection
                    .Where(x => x.EndsWith(".cs"))
                    .Where(x => x.Contains(parent, StringComparison.OrdinalIgnoreCase));
                result.Add(file, csfiles);
            }

            return new OwnershipCache(name, result.ToFrozenDictionary());
        }
    }
}
