using System;
using System.IO;

namespace RoadStatus.EndToEndTests.Extensions
{
    internal static class PathExtensions
    {
        public static string GetConsoleAppExePath(this AppDomain appDomain)
        {
            var consoleApplicationName = "RoadStatus.Console";
            var currentDirectory = appDomain.BaseDirectory;
            var testFolder = @"\test";
            return currentDirectory.Contains(testFolder)
                ? Path.Combine(
                    path1: currentDirectory[..currentDirectory.IndexOf(testFolder, StringComparison.Ordinal)],
                    path2: $@"src\{consoleApplicationName}\bin\Debug\net5.0\{consoleApplicationName}.exe")
                : throw new DirectoryNotFoundException("Cannot find the applications base directory.");
        }
    }
}