namespace Hephaestus.Core.Version1.FileSystem.Loading
{
    public interface IFileStore
    {
        void Save(string path, string content);
        void Rename(string oldPath, string newPath);
        void Remove(string path);
    }
}
