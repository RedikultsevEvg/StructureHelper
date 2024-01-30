using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using System.Text;

namespace StructureHelperLogics.NdmCalculations.Analyses
{
    public class ExportForceResultToCSVLogic : IExportResultLogic
    {
        const string separator = ";";
        StringBuilder output;
        IForcesResults results;
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

        public ExportForceResultToCSVLogic(IForcesResults forcesResults)
        {
            this.results = forcesResults;
            output = new StringBuilder();
        }

        private void ExportHeadings()
        {
            string[] headings =
    {
                    "Limit State",
                    "Calc duration",
                    "Mx",
                    "My",
                    "Nz",
                    "kx",
                    "ky",
                    "epsz"
                };
            output.AppendLine(string.Join(separator, headings));
        }
        private void ExportBoby()
        {
            foreach (var item in results.ForcesResultList)
            {
                if (item.IsValid == true)
                {
                    var tuple = item.DesignForceTuple.ForceTuple;
                    var strainMatrix = item.LoaderResults.StrainMatrix;
                    string[] newLine =
                        {
                            item.DesignForceTuple.LimitState.ToString(),
                            item.DesignForceTuple.CalcTerm.ToString(),
                            tuple.Mx.ToString(),
                            tuple.My.ToString(),
                            tuple.Nz.ToString(),
                            strainMatrix.Kx.ToString(),
                            strainMatrix.Ky.ToString(),
                            strainMatrix.EpsZ.ToString()
                        };
                    output.AppendLine(string.Join(separator, newLine));
                }
            }
        }
    }
}
