using System.IO;

namespace DirectoryListComparer.Model
{
    public class FileEntry
    {
        public string DirectoryEntry { get; set; }
        public string FileName { get; set; }
        public long Bytes { get; set; }
        public DirectoryEntry ParentDirectory { get; set; }

        public string RelativePath => Path.Combine(ParentDirectory.RelativePath, FileName);

        public override bool Equals(object obj)
        {
            var otherFileEntry = obj as FileEntry;
            if (otherFileEntry == null) return false;

            return RelativePath == otherFileEntry.RelativePath;
        }

        public override int GetHashCode()
        {
            return RelativePath.GetHashCode();
        }

        public override string ToString()
        {
            return Path.Combine(ParentDirectory.Path, FileName);
        }
    }
}
