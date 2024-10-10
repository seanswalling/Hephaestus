using System;
using System.IO;
using Spectre.Console.Cli;

namespace Hephaestus.CLI
{
    public static class FileLocations
    {
        public static readonly string ApplicationRoot = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Hephaestus");
        public static readonly string OutputFolder = Path.Combine(ApplicationRoot, "output");
        public static readonly string ErrorFolder = Path.Combine(ApplicationRoot, "error");
        public static readonly Func<Command, DateTime, string> OutputJsonFile = (ActualCommand, DateTime) => Path.Combine(OutputFolder, $"{(ActualCommand).GetType().Name}-{DateTime:yyyy-MM-dd_HH-mm-ss}.json");
        public static readonly Func<Command, DateTime, string> OutputCsvFile = (ActualCommand, DateTime) => Path.Combine(OutputFolder, $"{(ActualCommand).GetType().Name}-{DateTime:yyyy-MM-dd_HH-mm-ss}.csv");
        public static readonly Func<DateTime, string> ErrorFile = (DateTime) => Path.Combine(ErrorFolder, $"errors-{DateTime:yyyy-MM-dd}.txt");

        public static void EnsureFileStructures()
        {
            if (!Directory.Exists(ApplicationRoot))
            {
                Directory.CreateDirectory(ApplicationRoot);
            }

            if (!Directory.Exists(OutputFolder))
            {
                Directory.CreateDirectory(OutputFolder);
            }

            if (!Directory.Exists(ErrorFolder))
            {
                Directory.CreateDirectory(ErrorFolder);
            }
        }

        public static void EnsureApplicationFileStructures()
        {
            if (!Directory.Exists(ApplicationRoot))
            {
                Directory.CreateDirectory(ApplicationRoot);
            }
        }

        public static void EnsureRepositoryFolder(string repoName)
        {
            var repoFolder = Path.Combine(ApplicationRoot, repoName);

            if (!Directory.Exists(repoFolder))
            {
                Directory.CreateDirectory(repoFolder);
            }
        }
    }
}
