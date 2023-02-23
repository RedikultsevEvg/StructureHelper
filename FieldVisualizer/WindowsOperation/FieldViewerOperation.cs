using FieldVisualizer.Entities.Values.Primitives;
using FieldVisualizer.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FieldVisualizer.WindowsOperation
{
    public static class FieldViewerOperation
    {
        public static void ShowViewer(IEnumerable<IPrimitiveSet> primitiveSets)
        {
            WndFieldViewer Viewer = new WndFieldViewer(primitiveSets);
            Viewer.ShowDialog();
        }
    }
}
