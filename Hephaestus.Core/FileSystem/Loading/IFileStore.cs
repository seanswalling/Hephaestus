namespace Hephaestus.Core.FileSystem.Loading
{
    public interface IFileStore
    {
        void Save(string path, string content);
        void Rename(string oldPath, string newPath);
        void Remove(string path);
    }
}
