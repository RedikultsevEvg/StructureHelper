using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Services.Exports;
using System.IO;

namespace StructureHelperCommon.Models.FeaMaterials.ExportLogics
{
    public class ExportTextToFileLogic : IExportToFileLogic
    {
        public string FileName { get; set; }
        public string Text { get; set; } = string.Empty;

        public void Export()
        {
            if (FileName == string.Empty)
            {
                throw new StructureHelperException("File name is empty");
            }
            File.WriteAllText(FileName, Text);
        }
    }
}
