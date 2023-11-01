using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Hephaestus.Core.FileSystem.Loading
{
    public class FileSystemLoader
    {
        private readonly IFileStore _fileStore;

        public FileSystemLoader(IFileStore fileStore)
        {
            _fileStore = fileStore;
        }

        public void LoadAllFiles(string path)
        {
            Parallel.ForEach(Directory.EnumerateFiles(path, "*.*", SearchOption.AllDirectories), LoadFile);
        }

        private static Func<string, bool> IsValidFile => (x) =>
        {
            return new[] { ".sln", ".csproj", ".config", ".cs" }.Contains(Path.GetExtension(x)) &&
                   !IsNonPackagesConfigFile(x) && //exclude non-packages.config configs
                   !IsFolderWeIgnore(x) &&
                   !IsAssemblyFile(x);
        };

        private static Func<string, bool> IsAssemblyFile => (x) =>
            Path.GetFileName(x).Equals("AssemblyInfo.cs", StringComparison.OrdinalIgnoreCase);

        private static Func<string, bool> IsNonPackagesConfigFile =>
            (x) => Path.GetExtension(x) == ".config" && !x.Contains("packages.config");

        private static Func<string, bool> IsFolderWeIgnore => (x) =>
            x.Contains(@"\bin\") || x.Contains(@"\obj\") || x.Contains(@"\.git\") || x.Contains(@"\Templates\");

        internal void LoadFile(string path)
        {
            if (!IsValidFile(path))
                return;
            //, FileMode.Open, FileAccess.Read, FileShare.Read
            using var stream = File.OpenText(path);
            var content = stream.ReadToEnd();
            //using var sr = new StreamReader(stream);
            //var content = File.ReadAllText(path);
            _fileStore.Save(path, content);
        }

        internal void DeleteFile(string path)
        {
            _fileStore.Remove(path);
        }

        internal void RenameFile(string oldPath, string newPath)
        {
            _fileStore.Rename(oldPath, newPath);
        }
    }
}