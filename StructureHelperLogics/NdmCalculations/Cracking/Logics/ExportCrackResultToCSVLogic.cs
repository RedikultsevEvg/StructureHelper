using StructureHelperCommon.Models.Calculators;
using StructureHelperLogics.NdmCalculations.Analyses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    public class ExportCrackResultToCSVLogic : ExportToCSVLogicBase
    {
        private const string errorString = "-error-";
        private ICrackResult crackResult;

        public ExportCrackResultToCSVLogic(ICrackResult crackResult)
        {
            this.crackResult = crackResult;
        }

        public override void ExportHeadings()
        {
            string[] headings =
                {
                    "Action name",
                    "Long Nz",
                    "Long Mx",
                    "Long My",
                    "Long CrackWidth",
                    "Long UltimateCrackWidth",
                    "Long IsGood",
                    "Long Uf",
                    "Short Nz",
                    "Short Mx",
                    "Short My",
                    "Short CrackWidth",
                    "Short UltimateCrackWidth",
                    "Short IsGood",
                    "Short Uf",
                };
            output.AppendLine(string.Join(separator, headings));
        }
        public override void ExportBoby()
        {
            foreach (var item in crackResult.TupleResults)
            {
                //if (item.IsValid == true)
                {
                    string[] newLine =
                        {
                            item?.InputData?.TupleName ?? errorString,
                            item?.InputData?.LongTermTuple?.Nz.ToString() ?? errorString,
                            item?.InputData?.LongTermTuple?.Mx.ToString() ?? errorString,
                            item?.InputData?.LongTermTuple?.My.ToString() ?? errorString,
                            item?.LongTermResult?.CrackWidth.ToString() ?? errorString,
                            item?.LongTermResult ?.UltimateCrackWidth.ToString() ?? errorString,
                            item?.LongTermResult ?.IsCrackLessThanUltimate.ToString() ?? errorString,
                            (item?.LongTermResult ?.CrackWidth / item?.LongTermResult?.UltimateCrackWidth).ToString() ?? errorString,
                            item?.InputData?.ShortTermTuple?.Nz.ToString() ?? errorString,
                            item?.InputData?.ShortTermTuple?.Mx.ToString() ?? errorString,
                            item ?.InputData?.ShortTermTuple?.My.ToString() ?? errorString,
                            item?.ShortTermResult?.CrackWidth.ToString() ?? errorString,
                            item?.ShortTermResult?.UltimateCrackWidth.ToString() ?? errorString,
                            item?.ShortTermResult?.IsCrackLessThanUltimate.ToString() ?? errorString,
                            (item?.ShortTermResult?.CrackWidth / item?.ShortTermResult?.UltimateCrackWidth).ToString() ?? errorString,
                        };
                    output.AppendLine(string.Join(separator, newLine));
                }
            }
        }
    }
}
