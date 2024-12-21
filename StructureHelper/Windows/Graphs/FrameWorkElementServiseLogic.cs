using StructureHelper.Services.Exports;
using StructureHelperLogics.NdmCalculations.Analyses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace StructureHelper.Windows.Graphs
{
    public class FrameWorkElementServiseLogic : IFrameWorkElementServiseLogic
    {
        public void SaveImageToFile(FrameworkElement element)
        {
            var inputData = new ExportToFileInputData
            {
                Filter = "png |*.png",
                Title = "Save in *.png File"
            };
            var logic = new ExportFrameWorkElementLogic(element);
            var exportService = new ExportToFileService(inputData, logic);
            exportService.Export();
        }

        public void CopyImageToClipboard(FrameworkElement element)
        {
            if (element == null) return;

            // Measure and arrange the element to ensure it's fully rendered
            element.Measure(new Size(element.ActualWidth, element.ActualHeight));
            element.Arrange(new Rect(new Size(element.ActualWidth, element.ActualHeight)));

            // Render the element to a RenderTargetBitmap
            var renderTarget = new RenderTargetBitmap(
            (int)element.ActualWidth,
                (int)element.ActualHeight,
                96, // DPI X
                96, // DPI Y
                PixelFormats.Pbgra32);

            renderTarget.Render(element);

            Clipboard.SetImage(renderTarget);
        }
    }
}
