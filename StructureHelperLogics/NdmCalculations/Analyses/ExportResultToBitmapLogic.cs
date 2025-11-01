using StructureHelperCommon.Services.Exports;
using System.Windows.Media.Imaging;

namespace StructureHelperLogics.NdmCalculations.Analyses
{
    public class ExportResultToBitmapLogic : IExportToFileLogic
    {
        private BitmapImage bitmapImage;

        public ExportResultToBitmapLogic(BitmapImage bitmapImage)
        {
            this.bitmapImage = bitmapImage;
        }

        public string FileName { get; set; }

        public void Export()
        {
            using (var fileStream = new FileStream(FileName, FileMode.Create))
            {
                BitmapEncoder encoder = new BmpBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmapImage));
                encoder.Save(fileStream);
            }
        }
    }
}
