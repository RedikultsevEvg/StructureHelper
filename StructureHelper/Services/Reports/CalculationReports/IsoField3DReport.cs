using FieldVisualizer.Entities.Values.Primitives;
using FieldVisualizer.WindowsOperation;
using StructureHelper.Windows.CalculationWindows.CalculatorsViews.ForceCalculatorViews;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelper.Services.Reports.CalculationReports
{
    public class IsoField3DReport : IIsoField3DReport
    {
        private IEnumerable<IPrimitiveSet> primitiveSets;
        private IsoField3DViewerViewModel viewModel;

        public void Prepare()
        {
            viewModel = new IsoField3DViewerViewModel(primitiveSets);

        }

        public void ShowPrepared()
        {
            var wnd = new IsoField3DViewerView(viewModel);
            wnd.Show();
        }

        public void Show()
        {
            Prepare();
            ShowPrepared();
        }
        public IsoField3DReport(IEnumerable<IPrimitiveSet> primitiveSets)
        {
            this.primitiveSets = primitiveSets;
        }
    }
}
