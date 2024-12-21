using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace StructureHelperLogics.NdmCalculations.Analyses
{
    public class ExportFrameWorkElementLogic : IExportResultLogic
    {
        private FrameworkElement visual;
        public string FileName { get; set; }

        public void Export()
        {
            var encoder = new PngBitmapEncoder();
            EncodeVisual(visual, FileName, encoder);
        }

        public ExportFrameWorkElementLogic(FrameworkElement visual)
        {
            this.visual = visual;
        }

        private static void EncodeVisual(FrameworkElement visual, string fileName, BitmapEncoder encoder)
        {
            // Measure and arrange the element to ensure it's fully rendered
            visual.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            Rect previousBounds = VisualTreeHelper.GetDescendantBounds(visual);
            visual.Arrange(new Rect(0, 0, visual.ActualWidth, visual.ActualHeight));

            var bitmap = new RenderTargetBitmap(
                (int)visual.ActualWidth, 
                (int)visual.ActualHeight,
                96,
                96,
                PixelFormats.Pbgra32);

            bitmap.Render(visual);
            var frame = BitmapFrame.Create(bitmap);
            encoder.Frames.Add(frame);
            using (var stream = File.Create(fileName)) encoder.Save(stream);
            visual.Arrange(previousBounds);
        }
    }
}
