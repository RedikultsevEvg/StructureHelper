using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.FileDialogs
{
    using System;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;

    class ProgramExample
    {
        static async Task Main(string[] args)
        {
            string storagePath = Path.Combine(Environment.CurrentDirectory, "UserFiles");
            IFileRepository fileRepository = new FileRepository(storagePath);

            // Save a file
            string fileName = "example.txt";
            using (var fileStream = new MemoryStream(Encoding.UTF8.GetBytes("Hello, World!")))
            {
                await fileRepository.SaveFileAsync(fileStream, fileName);
                Console.WriteLine($"File '{fileName}' saved.");
            }

            // Retrieve a file
            using (Stream retrievedFile = await fileRepository.GetFileAsync(fileName))
            {
                using (var reader = new StreamReader(retrievedFile))
                {
                    string content = await reader.ReadToEndAsync();
                    Console.WriteLine($"Retrieved file content: {content}");
                }
            }

            // Update a file
            using (var updateStream = new MemoryStream(Encoding.UTF8.GetBytes("Updated content!")))
            {
                await fileRepository.UpdateFileAsync(updateStream, fileName);
                Console.WriteLine($"File '{fileName}' updated.");
            }

            // Retrieve updated file
            using (Stream updatedFile = await fileRepository.GetFileAsync(fileName))
            {
                using (var reader = new StreamReader(updatedFile))
                {
                    string updatedContent = await reader.ReadToEndAsync();
                    Console.WriteLine($"Updated file content: {updatedContent}");
                }
            }

            // Delete a file
            await fileRepository.DeleteFileAsync(fileName);
            Console.WriteLine($"File '{fileName}' deleted.");
        }
    }

}
