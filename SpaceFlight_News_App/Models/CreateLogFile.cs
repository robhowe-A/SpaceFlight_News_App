// ==============================================================================
// Filename: CreateLogFile.cs
// 
// Author: Robert Howell
// Date: 1/2/2026
// Edited: 1/18/2025
// Version: 1.2
//
// Description: This file creates a helper object that assists in creating log entries.
//
// ==============================================================================

namespace SpaceFlight_News_App.Models
{
    sealed class CreateLogFile : IDisposable
    {
        string directory;
        string path;
        private FileStream? fs;
        private StreamWriter sw;

        public CreateLogFile(string directory, string path, bool createDirectory)
        {
            this.directory = directory;
            this.path = path;

            bool directoryExists = System.IO.Directory.Exists(directory);
            if (createDirectory && !directoryExists)
            {
                var dir = System.IO.Directory.CreateDirectory(this.directory);
                dir.EnumerateDirectories().ToList().ForEach(directory => { Console.WriteLine(directory); });
            }
            if (!directoryExists) throw new DirectoryNotFoundException();

            if (!System.IO.File.Exists(this.path))
            {
                fs = System.IO.File.Create(this.path);
                fs.Close();
            }

            sw = System.IO.File.AppendText(this.path);
        }

        public override string ToString()
        {
            string str = $"Directory: {directory}\nPath: {path}";
            return str;
        }

        public void WriteLog(string log)
        {
            sw.Write(log);
            sw.Close();
        }

        public void Dispose()
        {
            sw.Close();
        }
    };
}
