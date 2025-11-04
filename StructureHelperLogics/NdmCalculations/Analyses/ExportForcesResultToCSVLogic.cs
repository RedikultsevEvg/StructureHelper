using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using System.Text;

namespace StructureHelperLogics.NdmCalculations.Analyses
{
    public class ExportForcesResultToCSVLogic : ExportToCSVLogicBase
    {
        private const string errorString = "-error-";
        IForceCalculatorResult results;

        public ExportForcesResultToCSVLogic(IForceCalculatorResult results)
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
                    var tuple = item.ForcesTupleResult.ForceTuple;
                    LoaderCalculator.Data.Matrix.IStrainMatrix strainMatrix = null;
                    try
                    {
                        strainMatrix = item.ForcesTupleResult.LoaderResults.StrainMatrix;
                    }
                    catch (Exception ex)
                    {

                    }
                    string[] newLine =
                        {
                            item.StateCalcTermPair.LimitState.ToString() ?? errorString,
                            item.StateCalcTermPair.CalcTerm.ToString() ?? errorString,
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
