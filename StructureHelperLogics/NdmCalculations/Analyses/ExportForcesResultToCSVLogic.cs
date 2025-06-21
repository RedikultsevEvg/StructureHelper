using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using System.Text;

namespace StructureHelperLogics.NdmCalculations.Analyses
{
    public class ExportForcesResultToCSVLogic : ExportToCSVLogicBase
    {
        private const string errorString = "-error-";
        IForcesResults results;

        public ExportForcesResultToCSVLogic(IForcesResults results)
        {
            this.results = results;
        }

        public override void ExportHeadings()
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
        public override void ExportBoby()
        {
            foreach (var item in results.ForcesResultList)
            {
                //if (item.IsValid == true)
                {
                    var tuple = item.DesignForceTuple.ForceTuple;
                    LoaderCalculator.Data.Matrix.IStrainMatrix strainMatrix = null;
                    try
                    {
                        strainMatrix = item.LoaderResults.StrainMatrix;
                    }
                    catch (Exception ex)
                    {

                    }
                    string[] newLine =
                        {
                            item.DesignForceTuple.LimitState.ToString() ?? errorString,
                            item.DesignForceTuple.CalcTerm.ToString() ?? errorString,
                            tuple.Mx.ToString() ?? errorString,
                            tuple.My.ToString() ?? errorString,
                            tuple.Nz.ToString() ?? errorString,
                            strainMatrix?.Kx.ToString() ?? errorString,
                            strainMatrix?.Ky.ToString() ?? errorString,
                            strainMatrix?.EpsZ.ToString() ?? errorString
                        };
                    output.AppendLine(string.Join(separator, newLine));
                }
            }
        }
    }
}
