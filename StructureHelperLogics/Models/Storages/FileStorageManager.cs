using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace StructureHelperLogics.Models.Storages
{


    public class FileStorageManager
    {
        private readonly Dictionary<Guid, OpenedFile> _openedFiles = new Dictionary<Guid, OpenedFile>();

        public void OpenFile()
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
                openFileDialog.Multiselect = true;
                openFileDialog.Title = "Select Files";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    foreach (var filePath in openFileDialog.FileNames)
                    {
                        var fileId = Guid.NewGuid();
                        var openedFile = new OpenedFile(fileId, filePath);

                        _openedFiles[fileId] = openedFile;

                        Console.WriteLine($"File '{openedFile.FilePath}' opened with ID: {fileId}");
                    }
                }
            }
        }

        public OpenedFile GetFile(Guid fileId)
        {
            if (_openedFiles.TryGetValue(fileId, out var openedFile))
            {
                return openedFile;
            }

            throw new KeyNotFoundException("File not found.");
        }

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

        public void AddRelatedObjectToFile(Guid fileId, FileRelatedObject relatedObject)
        {
            var openedFile = GetFile(fileId);
            openedFile.AddRelatedObject(relatedObject);
            Console.WriteLine($"Added related object to file ID: {fileId}, Related Object: {relatedObject}");
        }

        public FileRelatedObject GetRelatedObjectFromFile(Guid fileId, Guid objectId)
        {
            var openedFile = GetFile(fileId);
            return openedFile.GetRelatedObject(objectId);
        }

        public void RemoveRelatedObjectFromFile(Guid fileId, Guid objectId)
        {
            var openedFile = GetFile(fileId);
            openedFile.RemoveRelatedObject(objectId);
            Console.WriteLine($"Removed related object with ID: {objectId} from file ID: {fileId}");
        }

        public void ListOpenedFiles()
        {
            foreach (var file in _openedFiles.Values)
            {
                Console.WriteLine($"File ID: {file.Id}, Path: {file.FilePath}, Related Objects Count: {file.RelatedObjects.Count}");
            }
        }
    }

}
