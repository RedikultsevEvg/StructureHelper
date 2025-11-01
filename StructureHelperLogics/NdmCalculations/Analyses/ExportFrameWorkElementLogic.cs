using StructureHelperCommon.Services.Exports;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace StructureHelperLogics.NdmCalculations.Analyses
{
    public class ExportFrameWorkElementLogic : IExportToFileLogic
    {
        private FrameworkElement element;
        private double scaleFactor;
        public string FileName { get; set; }

        public void Export()
        {
            var encoder = new PngBitmapEncoder();
            EncodeVisual(element, FileName, encoder);
        }

        public ExportFrameWorkElementLogic(FrameworkElement visual, double scaleFactor = 1)
        {
            this.element = visual;
            this.scaleFactor = scaleFactor;
        }

        private void EncodeVisual(FrameworkElement visual, string fileName, BitmapEncoder encoder)
        {
            // Measure and arrange the element to ensure it's fully rendered
            //visual.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            //Rect previousBounds = VisualTreeHelper.GetDescendantBounds(visual);
            //visual.Arrange(new Rect(0, 0, visual.ActualWidth, visual.ActualHeight));
            element.Measure(new Size(element.ActualWidth, element.ActualHeight));
            element.Arrange(new Rect(new Size(element.ActualWidth, element.ActualHeight)));

            var bitmap = new RenderTargetBitmap(
                (int)(visual.ActualWidth * scaleFactor), 
                (int)(visual.ActualHeight * scaleFactor),
                96,
                96,
                PixelFormats.Pbgra32);

            bitmap.Render(visual);
            var frame = BitmapFrame.Create(bitmap);
            encoder.Frames.Add(frame);
            using (var stream = File.Create(fileName)) encoder.Save(stream);
            //visual.Arrange(previousBounds);
        }
    }
}
