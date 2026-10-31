using FieldVisualizer.Entities.Values.Primitives;
using FieldVisualizer.WindowsOperation;
using System.Collections.Generic;

namespace StructureHelper.Services.Reports.CalculationReports
{
    public class IsoFieldReport : IContourReport
    {
        private IEnumerable<IPrimitiveSet> primitiveSets;

        public void Prepare()
        {
            //
        }

        public void ShowPrepared()
        {
            FieldViewerOperation.ShowViewer(primitiveSets);
        }

        public void Show()
        {
            Prepare();
            ShowPrepared();
        }

        public IsoFieldReport(IEnumerable<IPrimitiveSet> primitiveSets)
        {
            this.primitiveSets = primitiveSets;
        }
    }
}
