using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DirectoryListComparer.Model;
using DirectoryListComparer.Parser;

namespace DirectoryListComparer
{
    class Program
    {
        private const string SourceFileSystem = @"Data\ftp files - AU.txt";
        private const string DestinationFileSystem = @"Data\ftp files - nib.txt";
        private static readonly Dictionary<string, DirectoryEntry> IndexedSourceDirectories = new Dictionary<string, DirectoryEntry>();
        private static readonly Dictionary<string, DirectoryEntry> IndexedDestinationDirectories = new Dictionary<string, DirectoryEntry>();

        static void Main()
        {
            var sourceDirectory = DirectoryParser.Parse(SourceFileSystem);
            var destinationDirectory = DirectoryParser.Parse(DestinationFileSystem);

            IndexDirectories(IndexedSourceDirectories, sourceDirectory);
            IndexDirectories(IndexedDestinationDirectories, destinationDirectory);
            
            var missingDirectories = IndexedSourceDirectories.Keys.Except(IndexedDestinationDirectories.Keys).ToList();

            var missingFiles = FindMissingFiles(sourceDirectory);
            PrintMissingFiles(missingFiles);

            if (Debugger.IsAttached)
            {
                Console.Write("Press any key to exit ... ");
                Console.ReadKey();
            }
        }

        public static void IndexDirectories(Dictionary<string, DirectoryEntry> dictionary, DirectoryEntry directory)
        {
            dictionary[directory.RelativePath] = directory;
            foreach (var subDirectory in directory.SubDirectories)
            {
                IndexDirectories(dictionary, subDirectory);
            }
        }

        public static List<FileEntry> FindMissingFiles(DirectoryEntry sourceDirectory)
        {
            var missingFilesList = new List<FileEntry>();

            var destinationDirectory = IndexedDestinationDirectories[sourceDirectory.RelativePath];
            missingFilesList.AddRange(sourceDirectory.Files.Except(destinationDirectory.Files));

            foreach (var subDirectory in sourceDirectory.SubDirectories)
            {
                missingFilesList.AddRange(FindMissingFiles(subDirectory));
            }

            return missingFilesList;
        }

        private static void PrintMissingFiles(List<FileEntry> missingFIles)
        {
            if (missingFIles.Count <= 0) return;

            Console.WriteLine("The following files are missing from nib's FTP folder\r\n");
            var missingFileDictionary = FindMissingFiles(missingFIles);

            int totalMissingFiles = 0;
            long totalMissingBytes = 0;
            foreach (var kvp in missingFileDictionary)
            {
                Console.WriteLine($"Directory of {kvp.Key.Path}\r\n");
                foreach (var fileEntry in kvp.Value)
                {
                    Console.WriteLine(fileEntry.DirectoryEntry);
                }

                totalMissingFiles += kvp.Value.Count;
                totalMissingBytes += kvp.Value.Sum(f => f.Bytes);
                Console.WriteLine($"{kvp.Value.Count,16:N0} File(s){kvp.Value.Sum(f => f.Bytes),14:N0} bytes\r\n");
            }

            Console.WriteLine("     Total Files Listed:");
            Console.WriteLine($"{totalMissingFiles,16:N0} File(s) {totalMissingBytes,13:N0} bytes");
            Console.WriteLine($"{missingFileDictionary.Count,16:N0} Dir(s)\r\n");
        }

        private static Dictionary<DirectoryEntry, List<FileEntry>> FindMissingFiles(List<FileEntry> missingFIles)
        {
            var missingFileDictionary = new Dictionary<DirectoryEntry, List<FileEntry>>();
            foreach (var file in missingFIles)
            {
                if (missingFileDictionary.ContainsKey(file.ParentDirectory) == false)
                {
                    missingFileDictionary[file.ParentDirectory] = new List<FileEntry>();
                }

                missingFileDictionary[file.ParentDirectory].Add(file);
            }

            return missingFileDictionary;
        }
    }
}
