using DataAccess.FileDialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataAccess.FileDialogs
{
    internal class FileStorage
    {
        using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

public class FileStorageManager
    {
        // Dictionary to store files with unique IDs as keys
        private readonly Dictionary<Guid, OpenedFile> _openedFiles = new Dictionary<Guid, OpenedFile>();

        // Method to open a file and add it to the storage
        public Guid OpenFile()
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
                openFileDialog.Multiselect = true; // Allow multiple file selection
                openFileDialog.Title = "Select Files";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    foreach (var filePath in openFileDialog.FileNames)
                    {
                        var fileId = Guid.NewGuid();
                        var openedFile = new OpenedFile(fileId, filePath);

                        // Add to storage
                        _openedFiles[fileId] = openedFile;

                        Console.WriteLine($"File '{openedFile.FilePath}' opened with ID: {fileId}");
                    }
                }
            }

            return Guid.Empty;
        }

        // Method to get an opened file by ID
        public OpenedFile GetFile(Guid fileId)
        {
            if (_openedFiles.TryGetValue(fileId, out var openedFile))
            {
                return openedFile;
            }

            throw new KeyNotFoundException("File not found.");
        }

        // Method to close a file by ID
        public void CloseFile(Guid fileId)
        {
            if (_openedFiles.ContainsKey(fileId))
            {
                _openedFiles.Remove(fileId);
                Console.WriteLine($"File with ID: {fileId} has been closed.");
            }
            else
            {
                throw new KeyNotFoundException("File not found.");
            }
        }

        // Method to read content of an opened file by ID
        public string ReadFileContent(Guid fileId)
        {
            var openedFile = GetFile(fileId);
            return File.ReadAllText(openedFile.FilePath);
        }

        // Method to list all opened files
        public void ListOpenedFiles()
        {
            foreach (var file in _openedFiles.Values)
            {
                Console.WriteLine($"File ID: {file.Id}, Path: {file.FilePath}");
            }
        }
    }

    // Class representing an opened file
    public class OpenedFile
    {
        public Guid Id { get; }
        public string FilePath { get; }

        public OpenedFile(Guid id, string filePath)
        {
            Id = id;
            FilePath = filePath;
        }
    }

}

class Program
{
    [STAThread] // Required for OpenFileDialog
    static void Main()
    {
        var fileStorageManager = new FileStorageManager();

        // Open files and add them to the storage
        fileStorageManager.OpenFile();

        // List all opened files
        Console.WriteLine("\nOpened Files:");
        fileStorageManager.ListOpenedFiles();

        // Example: Read content of the first opened file (if any)
        var openedFiles = new List<Guid>(fileStorageManager._openedFiles.Keys);
        if (openedFiles.Count > 0)
        {
            var firstFileId = openedFiles[0];
            Console.WriteLine($"\nReading content of the first opened file (ID: {firstFileId}):");
            string content = fileStorageManager.ReadFileContent(firstFileId);
            Console.WriteLine(content);
        }

        // Close all files
        foreach (var fileId in openedFiles)
        {
            fileStorageManager.CloseFile(fileId);
        }
    }
}

}
