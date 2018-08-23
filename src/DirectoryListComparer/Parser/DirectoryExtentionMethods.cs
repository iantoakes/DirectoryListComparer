namespace DirectoryListComparer.Parser
{
    public static class DirectoryExtentionMethods
    {
        /// <summary>
        /// Contains the string "Volume Serial Number is"
        /// </summary>
        public static bool IsVolumeSerialNumber(this string line)
        {
            return line.Contains("Volume Serial Number is");
        }

        /// <summary>
        /// Contains the string "Volume in drive"
        /// </summary>
        public static bool IsVolumeInDrive(this string line)
        {
            return line.Contains("Volume in drive");
        }

        /// <summary>
        /// Contains a blank line
        /// </summary>
        public static bool IsBlank(this string line)
        {
            return string.IsNullOrWhiteSpace(line);
        }

        /// <summary>
        /// Contains the string "Directory"
        /// </summary>
        public static bool IsDirectoryOf(this string line)
        {
            return line.Contains("Directory of");
        }

        /// <summary>
        /// Contains either the strings "." or ".."
        /// </summary>
        public static bool IsSelfOrParentDirectory(this string subDirectoryName)
        {
            return subDirectoryName != "." && subDirectoryName != "..";
        }

        /// <summary>
        /// Contains the string "<DIR>"
        /// </summary>
        public static bool IsSubDirectory(this string line)
        {
            return line.Contains("<DIR>");
        }

        /// <summary>
        /// Contains the string "Total Files Listed:"
        /// </summary>
        public static bool IsTotalFilesListed(this string line)
        {
            return line.Contains("Total Files Listed:");
        }

        /// <summary>
        /// Contains the string "Dir(s)"
        /// </summary>
        public static bool IsDirs(this string line)
        {
            return line.Contains("Dir(s)");
        }

        /// <summary>
        /// Contains the string "File(s)"
        /// </summary>
        public static bool IsFiles(this string line)
        {
            return line.Contains("File(s)");
        }
    }
}