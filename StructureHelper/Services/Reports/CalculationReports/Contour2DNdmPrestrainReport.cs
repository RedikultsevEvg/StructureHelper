using StructureHelper.Infrastructure.UI.DataContexts;
using StructureHelper.Services.ResultViewers;
using StructureHelper.Windows.CalculationWindows.CalculatorsViews.ForceCalculatorViews;

namespace StructureHelper.Services.Reports.CalculationReports
{
    public class Contour2DNdmPrestrainReport : IContourReport
    {
        private SelectedPrimitiveSet primitiveSet;
        private IsoField2DViewerViewModel viewModel;
        private IsoField2DViewerView view;
        public Contour2DNdmPrestrainReport(SelectedPrimitiveSet primitiveSet)
        {
            this.primitiveSet = primitiveSet;
        }
        public void Prepare()
        {
            viewModel = new()
            {
                ShadedPrimitives = GetPrimitivesByNdmPrimitivesLogic.GetShadedPrimitives(primitiveSet.AllPrimitives),
                PrimitiveSets = ShowIsoFieldResult.GetPrimitiveSets(primitiveSet.Ndms, NdmPrestrainFuncFactory.GetResultFuncs())
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
