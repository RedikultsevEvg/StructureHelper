using StructureHelperCommon.Services.Exports;
using System.Text;

namespace StructureHelperLogics.NdmCalculations.Analyses
{
    public abstract class ExportToCSVLogicBase : IExportToFileLogic
    {
        public string separator => ";";
        public StringBuilder output { get; } = new();
        public string FileName { get; set; }

        public void Export()
        {
            ExportHeadings();
            ExportBoby();
            try
            {
                File.AppendAllText(FileName, output.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine("Data could not be written to the CSV file.");
                return;
            }
        }

        public abstract void ExportBoby();
        public abstract void ExportHeadings();
    }
}