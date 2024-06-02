using StructureHelper.Services.Reports.CalculationReports;
using StructureHelper.Services.ResultViewers;
using StructureHelper.Windows.Errors;
using StructureHelper.Windows.ViewModels.Errors;
using StructureHelperLogics.NdmCalculations.Cracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelper.Windows.CalculationWindows.CalculatorsViews
{
    public class ShowCrackIsoFieldsLogic : IShowCrackIsoFieldsLogic
    {
        private IsoFieldReport isoFieldReport;

        public void ShowIsoField(IEnumerable<RebarCrackResult> rebarResults)
        {
            try
            {
                var primitiveSets = ShowIsoFieldResult.GetPrimitiveSets(rebarResults, CrackResultFuncFactory.GetResultFuncs());
                isoFieldReport = new IsoFieldReport(primitiveSets);
                isoFieldReport.Show();
            }
            catch (Exception ex)
            {
                var vm = new ErrorProcessor()
                {
                    ShortText = "Errors apearred during showing isofield, see detailed information",
                    DetailText = $"{ex}"
                };
                new ErrorMessage(vm).ShowDialog();
            }
        }
    }
}
