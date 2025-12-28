using StructureHelper.Services.Exports;
using StructureHelperCommon.Services.Exports.Factories;
using StructureHelperLogics.NdmCalculations.Analyses;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace StructureHelper.Windows.Graphs
{
    public class FrameWorkElementServiseLogic : IFrameWorkElementServiseLogic
    {
        private const int defaultDpi = 96;

        public int Dpi { get; set; } = 96;

        public void SaveImageToFile(FrameworkElement element, double scaleFactor = 1)
        {
            var inputData = FileInputDataFactory.GetFileIOInputData(FileInputDataType.Png);
            var logic = new ExportFrameWorkElementLogic(element, scaleFactor);
            var exportService = new ExportToFileService(inputData, logic);
            exportService.Export();
        }

        public void CopyImageToClipboard(FrameworkElement element, double scaleFactor = 1)
        {
            if (element == null) return;

            // Measure and arrange the element to ensure it's fully rendered
            element.Measure(new Size(element.ActualWidth, element.ActualHeight));
            element.Arrange(new Rect(new Size(element.ActualWidth, element.ActualHeight)));

            // Render the element to a RenderTargetBitmap
            var renderTarget = new RenderTargetBitmap(
                (int)(element.ActualWidth * scaleFactor * Dpi / defaultDpi),
                (int)(element.ActualHeight * scaleFactor * Dpi / defaultDpi),
                Dpi, // DPI X
                Dpi, // DPI Y
                PixelFormats.Pbgra32);

            renderTarget.Render(element);

            Clipboard.SetImage(renderTarget);
        }
    }
}
