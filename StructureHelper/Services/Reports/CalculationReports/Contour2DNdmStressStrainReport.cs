using StructureHelper.Infrastructure.UI.DataContexts;
using StructureHelper.Services.ResultViewers;
using StructureHelper.Windows.CalculationWindows.CalculatorsViews.ForceCalculatorViews;

namespace StructureHelper.Services.Reports.CalculationReports
{
    public class Contour2DNdmStressStrainReport : IContourReport
    {
        private SelectedPrimitiveSet primitiveSet;
        private IsoField2DViewerViewModel viewModel;
        private IsoField2DViewerView view;

        public Contour2DNdmStressStrainReport(SelectedPrimitiveSet primitiveSet)
        {
            this.primitiveSet = primitiveSet;
        }

        public void Prepare()
        {
            viewModel = new()
            {
                PrimitiveSets = ShowIsoFieldResult.GetPrimitiveSets(primitiveSet.StrainMatrix, primitiveSet.Ndms, NdmStressStrainResultFuncFactory.GetResultFuncs()),
                ShadedPrimitives = GetPrimitivesByNdmPrimitivesLogic.GetShadedPrimitives(primitiveSet.ContourPrimitives),
            };
            view = new(viewModel);
        }

        public void Show()
        {
            Prepare();
            ShowPrepared();
        }

        public void ShowPrepared()
        {
            view.ShowDialog();
        }
    }
}
