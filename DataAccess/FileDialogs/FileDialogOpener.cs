using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataAccess.FileDialogs
{
    public class FileDialogOpener
    {
        public void OpenFileAndRead()
        {
            // Create an instance of OpenFileDialog
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                // Set filter options and filter index
                openFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.Multiselect = false; // Set to true if you want to allow multiple file selection
                openFileDialog.Title = "Select a File";

                // Show the dialog and get result
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Get the path of the selected file
                    string selectedFilePath = openFileDialog.FileName;

                    // Read the content of the file
                    try
                    {
                        string fileContent = File.ReadAllText(selectedFilePath);
                        Console.WriteLine($"File Content of '{selectedFilePath}':");
                        Console.WriteLine(fileContent);
                    }
                    catch (IOException ex)
                    {
                        Console.WriteLine($"An error occurred while reading the file: {ex.Message}");
                    }
                }
                else
                {
                    Console.WriteLine("File selection was cancelled.");
                }
            }
        }
    }

}
