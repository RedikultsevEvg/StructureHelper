using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.FileDialogs
{
    using System;
    using System.IO;
    using System.Threading.Tasks;

    public class FileRepository : IFileRepository
    {
        private readonly string _storageDirectory;

        public FileRepository(string storageDirectory)
        {
            _storageDirectory = storageDirectory;

            // Ensure the storage directory exists
            if (!Directory.Exists(_storageDirectory))
            {
                Directory.CreateDirectory(_storageDirectory);
            }
        }

        // Save a file to the repository
        public async Task SaveFileAsync(Stream fileStream, string fileName)
        {
            string filePath = Path.Combine(_storageDirectory, fileName);

            // Ensure the file does not already exist
            if (File.Exists(filePath))
            {
                throw new InvalidOperationException("File already exists.");
            }

            using (var file = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                await fileStream.CopyToAsync(file);
            }
        }

        // Retrieve a file from the repository
        public async Task<Stream> GetFileAsync(string fileName)
        {
            string filePath = Path.Combine(_storageDirectory, fileName);

            // Ensure the file exists
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("File not found.");
            }

            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            return await Task.FromResult(fileStream);
        }

        // Update an existing file in the repository
        public async Task UpdateFileAsync(Stream fileStream, string fileName)
        {
            string filePath = Path.Combine(_storageDirectory, fileName);

            // Ensure the file exists
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("File not found.");
            }

            using (var file = new FileStream(filePath, FileMode.Truncate, FileAccess.Write))
            {
                await fileStream.CopyToAsync(file);
            }
        }

        // Delete a file from the repository
        public async Task DeleteFileAsync(string fileName)
        {
            string filePath = Path.Combine(_storageDirectory, fileName);

            // Ensure the file exists
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("File not found.");
            }

            File.Delete(filePath);
            await Task.CompletedTask;
        }
    }

}
