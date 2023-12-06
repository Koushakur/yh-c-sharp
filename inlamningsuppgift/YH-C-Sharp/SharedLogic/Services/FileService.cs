using System.Diagnostics;

namespace SharedLogic.Services
{
    public class FileService
    {
        /// <summary>
        /// Saves content to a given file path
        /// </summary>
        /// <param name="filePath">Path to file to save content to</param>
        /// <param name="content">Content to put into the file</param>
        public static void SaveToFile(string filePath, string content) {
            try {
                var directory = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(directory)) {
                    Directory.CreateDirectory(directory!);
                }

                using (var f = new StreamWriter(filePath)) {
                    f.Write(content);
                }
            }
            catch (Exception e) { Debug.WriteLine(e); }
        }

        /// <summary>
        /// Reads and returns the content of a text file
        /// </summary>
        /// <param name="filePath">Path to file to read</param>
        /// <returns>File contents if file exists and is successfully read, otherwise null</returns>
        public static string? ReadFromFile(string filePath) {
            try {
                if (File.Exists(filePath)) {
                    using var f = new StreamReader(filePath);
                    return f.ReadToEnd();
                }
            }
            catch (Exception e) { Debug.WriteLine(e); }

            return null;
        }

        /// <summary>
        /// Cross-platform opening of a folder in respective file browsers
        /// </summary>
        /// <param name="folderPath">Path to folder to open</param>
        public static void OpenFolder(string folderPath) {
            try {

                //Windows
                if (OperatingSystem.IsWindows()) {
                    Process.Start("explorer.exe", folderPath);

                    //Mac
                } else if (OperatingSystem.IsMacOS()) {
                    Process.Start("open", folderPath);

                    //Linux
                } else if (OperatingSystem.IsLinux()) {
                    Process.Start("xdg-open", folderPath);

                } else {
                    throw new NotSupportedException("Operating system not supported.");
                }
            }
            catch (Exception e) { Debug.WriteLine(e); }
        }
    }
}
