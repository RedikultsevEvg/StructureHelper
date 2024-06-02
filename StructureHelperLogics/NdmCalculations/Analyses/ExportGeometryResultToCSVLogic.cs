using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using StructureHelperLogics.NdmCalculations.Analyses.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Analyses
{
    public class ExportGeometryResultToCSVLogic : IExportResultLogic
    {
        const string separator = ";";
        StringBuilder output;
        IGeometryResult result;
        public string FileName { get; set; }
        public ExportGeometryResultToCSVLogic(IGeometryResult geometryResult)
        {
            this.result = geometryResult;
            output = new StringBuilder();
        }
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
        private void ExportHeadings()
        {
            string[] headings =
                {
                    "Name",
                    "Short",
                    "Measerement Unit",
                    "Value",
                    "Description",
                };
            output.AppendLine(string.Join(separator, headings));
        }
        private void ExportBoby()
        {
            foreach (var item in result.TextParameters)
            {
                if (item.IsValid == true)
                {
                    string[] newLine =
                        {
                            item.Name,
                            item.ShortName,
                            item.Text,
                            item.Value.ToString(),
                            item.Description
                        };
                    output.AppendLine(string.Join(separator, newLine));
                }
            }
        }
    }
}
