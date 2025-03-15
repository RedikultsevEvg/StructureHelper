using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelper.Windows.CalculationWindows.ProgressViews
{
    public static class TraceDocumentService
    {
        public static void ShowDocument(IEnumerable<ITraceLoggerEntry> traceLoggerEntries)
        {
            if (traceLoggerEntries is null)
            {
                throw new StructureHelperException(ErrorStrings.ParameterIsNull + ": Document entries");
            }
            var wnd = new TraceDocumentView(traceLoggerEntries);
            wnd.ShowDialog();
        }
    }
}
