using System.Windows;

namespace StructureHelper.Windows.Graphs
{
    public interface IFrameWorkElementServiseLogic
    {
        void CopyImageToClipboard(FrameworkElement element, double scaleFactor = 1);
        void SaveImageToFile(FrameworkElement element, double scaleFactor = 1);
    }
}