using FieldVisualizer.Entities.Values.Primitives;
using FieldVisualizer.Services.PrimitiveServices;
using FieldVisualizer.WindowsOperation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelper.Services.Reports.CalculationReports
{
    public class IsoFieldReport : IIsoFieldReport
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
