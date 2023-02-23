using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Strings;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Analyses
{
    public class ExportToCSVLogic : IExportResultLogic
    {
        string filename;

        public void Export(INdmResult ndmResult)
        {
            string separator = ";";
            StringBuilder output = new StringBuilder();

            if (ndmResult is ForcesResults)
            {
                var forceResults = ndmResult as ForcesResults;
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
                foreach (var item in forceResults.ForcesResultList)
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
                try
                {
                    File.AppendAllText(filename, output.ToString());
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Data could not be written to the CSV file.");
                    return;
                }
            }
            else throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknown +": "+ nameof(ndmResult));
        }

        public ExportToCSVLogic(string filename)
        {
            this.filename = filename;
        }
    }
}
