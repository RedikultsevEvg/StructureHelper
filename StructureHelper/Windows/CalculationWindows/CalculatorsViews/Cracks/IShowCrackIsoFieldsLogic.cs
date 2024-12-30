using StructureHelperLogics.NdmCalculations.Cracking;
using System.Collections.Generic;

namespace StructureHelper.Windows.CalculationWindows.CalculatorsViews
{
    public interface IShowCrackIsoFieldsLogic
    {
        void ShowIsoField(IEnumerable<IRebarCrackResult> rebarResults);
    }
}