using StructureHelperCommon.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.Storages
{
    //public class OpenedFile
    //{
    //    public Guid Id { get; }
    //    public string FilePath { get; }
    //    public List<ISaveable> RelatedObjects { get; } // List of related objects

    //    public OpenedFile(Guid id, string filePath)
    //    {
    //        Id = id;
    //        FilePath = filePath;
    //        RelatedObjects = new List<ISaveable>();
    //    }

    //    public void AddRelatedObject(ISaveable relatedObject)
    //    {
    //        RelatedObjects.Add(relatedObject);
    //    }

    //    public void RemoveRelatedObject(Guid objectId)
    //    {
    //        RelatedObjects.RemoveAll(o => o.Id == objectId);
    //    }

    //    public ISaveable GetRelatedObject(Guid objectId)
    //    {
    //        return RelatedObjects.Find(o => o.Id == objectId);
    //    }
    //}

    //class Program
    //{
    //    [STAThread] // Required for OpenFileDialog
    //    static void Main()
    //    {
    //        var fileStorageManager = new FileStorageManager();

    //        // Open files and add them to the storage
    //        fileStorageManager.OpenFile();

    //        // List all opened files
    //        Console.WriteLine("\nOpened Files:");
    //        fileStorageManager.ListOpenedFiles();

    //        // Example: Adding related objects to the first opened file (if any)
    //        var openedFiles = new List<Guid>(fileStorageManager._openedFiles.Keys);
    //        if (openedFiles.Count > 0)
    //        {
    //            var firstFileId = openedFiles[0];
    //            var relatedObject = new FileRelatedObject("Sample Object", "This is a sample description");
    //            fileStorageManager.AddRelatedObjectToFile(firstFileId, relatedObject);

    //            Console.WriteLine("\nAfter Adding Related Object:");
    //            fileStorageManager.ListOpenedFiles();

    //            // Retrieve related object
    //            var retrievedObject = fileStorageManager.GetRelatedObjectFromFile(firstFileId, relatedObject.Id);
    //            Console.WriteLine($"\nRetrieved Related Object: {retrievedObject}");

    //            // Remove related object
    //            fileStorageManager.RemoveRelatedObjectFromFile(firstFileId, relatedObject.Id);

    //            Console.WriteLine("\nAfter Removing Related Object:");
    //            fileStorageManager.ListOpenedFiles();
    //        }

    //        // Close all files
    //        foreach (var fileId in openedFiles)
    //        {
    //            fileStorageManager.CloseFile(fileId);
    //        }
    //    }
    //}


}
