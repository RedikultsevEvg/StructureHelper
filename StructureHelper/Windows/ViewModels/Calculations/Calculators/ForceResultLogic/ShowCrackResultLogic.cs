using StructureHelper.Windows.Errors;
using StructureHelper.Windows.ViewModels.Errors;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Settings;
using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.NdmCalculations.Cracking;
using StructureHelperLogics.NdmCalculations.Primitives;
using StructureHelperLogics.Services.NdmPrimitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StructureHelper.Windows.ViewModels.Calculations.Calculators.ForceResultLogic
{
    internal class ShowCrackResultLogic
    {
        public static GeometryNames GeometryNames => ProgramSetting.GeometryNames;
        public LimitStates LimitState { get; set; }
        public CalcTerms CalcTerm { get; set; }
        public ForceTuple ForceTuple { get; set; }
        public IEnumerable<INdmPrimitive> ndmPrimitives { get; set; }
        public void Show()
        {
            var calculator = new CrackForceCalculator();
            calculator.EndTuple = ForceTuple;
            calculator.NdmCollection = NdmPrimitivesService.GetNdms(ndmPrimitives, LimitState, CalcTerm);
            calculator.Run();
            var result = (CrackForceResult)calculator.Result;
            if (result.IsValid)
            {
                //var softLogic = new ExponentialSofteningLogic() { ForceRatio = result.ActualFactor };
                string message = string.Empty;
                if (result.IsSectionCracked)
                {
                    message += $"Actual crack factor {result.FactorOfCrackAppearance}\n";
                    message += $"Softening crack factor PsiS={result.PsiS}\n";
                    message += $"{GeometryNames.MomFstName}={result.TupleOfCrackAppearance.Mx},\n {GeometryNames.MomSndName}={result.TupleOfCrackAppearance.My},\n {GeometryNames.LongForceName}={result.TupleOfCrackAppearance.Nz}";
                }
                else
                {
                    message += "Cracks are not apeared";
                }
                MessageBox.Show(
                    message,
                    "Crack results",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            else
            {
                var errorVM = new ErrorProcessor()
                {
                    ShortText = "Error apeared while crack calculate",
                    DetailText = result.Description
                };
                var wnd = new ErrorMessage(errorVM);
                wnd.ShowDialog();
            }
        }

    }
}
