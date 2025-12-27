using StructureHelper.Infrastructure.UI.DataContexts;
using StructureHelper.Windows.CalculationWindows.CalculatorsViews.ForceCalculatorViews;

namespace StructureHelper.Services.Reports.CalculationReports
{
    public class IsoField2DReport : IIsoFieldReport
    {
        private SelectedPrimitiveSet primitiveSet;
        private IsoField2DViewerViewModel viewModel;
        private IsoField2DViewerView view;

        public IsoField2DReport(SelectedPrimitiveSet primitiveSet)
        {
            this.primitiveSet = primitiveSet;
        }

        public void Prepare()
        {
            viewModel = new(primitiveSet);
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
