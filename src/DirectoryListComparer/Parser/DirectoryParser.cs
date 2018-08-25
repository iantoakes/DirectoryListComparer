using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using DirectoryListComparer.Model;

namespace DirectoryListComparer.Parser
{
    public class DirectoryParser
    {
        public static DirectoryEntry Parse(string fileName)
        {
            var fileStream = File.OpenText(fileName);
            string line = fileStream.ReadLine();
            if (line == null) throw new InvalidOperationException();

            var currentDirectory = GetRootDirectoryFromFile(fileStream);
            var directoryDictionary = new Dictionary<string, DirectoryEntry> {{currentDirectory.Path, currentDirectory}};

            while (line != null)
            {
                if (line.IsInsignificantLine())
                {
                    line = fileStream.ReadLine();
                    continue;
                }

                if (line.IsDirectoryOf())
                {
                    currentDirectory = directoryDictionary[ParseDirectoryOf(line)];
                }
                else if (line.IsSubDirectory())
                {
                    string subDirectoryName = ParseSubDirectoryName(line);
                    if (subDirectoryName.IsSelfOrParentDirectory())
                    {
                        var subDirectory = new DirectoryEntry(Path.Combine(currentDirectory.Path, subDirectoryName));
                        currentDirectory.SubDirectories.Add(subDirectory);
                        directoryDictionary[subDirectory.Path] = subDirectory;
                    }
                }
                else
                {
                    var file = PasrseFileEntry(line);
                    file.ParentDirectory = currentDirectory;
                    currentDirectory.Files.Add(file);
                }

                line = fileStream.ReadLine();
            }

            return directoryDictionary.First().Value;
        }

        private static DirectoryEntry GetRootDirectoryFromFile(TextReader fileStream)
        {
            string line = fileStream.ReadLine();
            while (line.IsDirectoryOf() == false)
            {
                line = fileStream.ReadLine();
                if(line == null) throw new InvalidOperationException();
            }

            return new DirectoryEntry(ParseDirectoryOf(line));
        }


        public static FileEntry PasrseFileEntry(string line)
        {
            string bytes = ParseBytes(line);
            var file = new FileEntry
            {
                DirectoryEntry = line,
                Bytes = long.Parse(bytes, NumberStyles.AllowThousands),
                FileName = ParseFileName(line)
            };
            return file;
        }

        private static string ParseFileName(string line)
        {
            return line.Substring(39);
        }

        private static string ParseBytes(string line)
        {
            return line.Substring(20, 18).Trim();
        }

        public static string ParseSubDirectoryName(string line)
        {
            return line.Substring(39);
        }

        public static string ParseDirectoryOf(string line)
        {
            return line.Substring(14);
        }
    }
}