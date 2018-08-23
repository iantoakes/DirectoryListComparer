using System.Collections.Generic;

namespace DirectoryListComparer.Model
{
    public class DirectoryEntry
    {
        public DirectoryEntry(string path)
        {
            Path = path;
            Files = new List<FileEntry>();
            SubDirectories = new List<DirectoryEntry>();
        }

        public string Path { get; private set; }

        public string RelativePath
        {
            get
            {
                string pathRoot = System.IO.Path.GetPathRoot(Path);
                bool isUncPath = pathRoot.StartsWith(@"\\");

                string path = Path.Replace(pathRoot, isUncPath ? "" : @"\");
                path = path == "" ? @"\" : path;
                return path;
            }
        }

        public List<DirectoryEntry> SubDirectories { get; set; }

        public List<FileEntry> Files { get; set; }

        public override bool Equals(object obj)
        {
            var otherDirectory = obj as DirectoryEntry;
            if (otherDirectory == null) return false;

            return Path == otherDirectory.Path;
        }

        public override int GetHashCode()
        {
            return Path.GetHashCode();
        }

        public override string ToString()
        {
            return Path;
        }
    }
}
