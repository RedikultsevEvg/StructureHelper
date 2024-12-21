using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using System.Text;

namespace StructureHelperLogics.NdmCalculations.Analyses
{
    public class ExportForcesResultToCSVLogic : ExportToCSVLogicBase
    {
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
