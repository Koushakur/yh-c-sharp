using System.Diagnostics;

namespace SharedLogic.Services
{
    internal class FileService
    {
        /// <summary>
        /// Saves content to a given file path
        /// </summary>
        /// <param name="filePath">Path to file to save content to</param>
        /// <param name="content">Content to put into the file</param>
        /// <returns>true if file saved successfully, otherwise false</returns>
        public static bool SaveToFile(string filePath, string content) {
            try {
                var directory = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(directory)) {
                    Directory.CreateDirectory(directory!);
                }

                File.WriteAllText(filePath, content);
            }
            catch (Exception e) { Debug.WriteLine(e); }
            return true;
        }

        /// <summary>
        /// Reads and returns the content of a text file
        /// </summary>
        /// <param name="filePath">Path to file to read</param>
        /// <returns>File contents if file exists and is successfully read, otherwise null</returns>
        public static string? ReadFromFile(string filePath) {
            try {
                if (File.Exists(filePath)) {
                    return File.ReadAllText(filePath);
                }
            }
            catch (Exception e) { Debug.WriteLine(e); }

            return null;
        }

        /// <summary>
        /// Cross-platform opening of a folder in respective file browsers
        /// </summary>
        /// <param name="folderPath">Path to folder to open</param>
        /// <returns>true if successfully opened folder, otherwise false</returns>
        public static bool OpenFolder(string folderPath) {
            try {

                if (Directory.Exists(folderPath)) {
                    //Windows
                    if (OperatingSystem.IsWindows()) {
                        Process.Start("explorer.exe", folderPath);
                        return true;

                        //Mac
                    } else if (OperatingSystem.IsMacOS()) {
                        Process.Start("open", folderPath);
                        return true;

                        //Linux
                    } else if (OperatingSystem.IsLinux()) {
                        Process.Start("xdg-open", folderPath);
                        return true;

                    }
                }
            }
            catch (Exception e) { Debug.WriteLine(e); }

            return false;
        }
    }
}
