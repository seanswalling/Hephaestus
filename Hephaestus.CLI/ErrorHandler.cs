using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Spectre.Console;

namespace Hephaestus.CLI
{
    public static class ErrorHandler
    {
        public static void Handle(this List<string> errors)
        {
            if (errors.Count == 0) return;

            var errorFile = FileLocations.ErrorFile(DateTime.Today);
            AnsiConsole.WriteLine($"There were some errors, inspect {errorFile}");
            File.AppendAllLines(errorFile, errors.Select(x => $"{DateTimeOffset.Now}:yyyy-MM-dd_HH-mm-ss" + x).ToArray());
        }
    }
}