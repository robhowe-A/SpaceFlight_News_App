// ==============================================================================
// Filename: CreateLogFile.cs
// 
// Author: Robert Howell
// Date: 1/18/2025
// Edited: 6/22/2026
// Version: 1.2
//
// Description: This file creates a helper object that assists in creating log entries.
//
// ==============================================================================

namespace SpaceFlight_News_App.Models
{
    sealed class CreateLogFile : IDisposable
    {
        readonly string _directory;
        private readonly string _path;
        private readonly StreamWriter _sw;

        public CreateLogFile(string directory, string path, bool createDirectory)
        {
            this._directory = directory;
            this._path = path;

            var directoryExists = Directory.Exists(directory);
            if (createDirectory && !directoryExists)
            {
                var dir = Directory.CreateDirectory(this._directory);
                dir.EnumerateDirectories().ToList().ForEach(Console.WriteLine);
            }
            if (!directoryExists) throw new DirectoryNotFoundException();

            if (!File.Exists(this._path))
            {
                var fs1 = File.Create(this._path);
                fs1.Close();
            }

            _sw = File.AppendText(this._path);
        }

        public override string ToString()
        {
            var str = $"Directory: {_directory}\nPath: {_path}";
            return str;
        }

        public void WriteLog(string log)
        {
            _sw.Write(log);
            _sw.Close();
        }

        public void Dispose()
        {
            _sw.Close();
        }
    };
}
