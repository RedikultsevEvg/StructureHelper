using System.Windows;

namespace StructureHelper.Windows.Graphs
{
    public interface IFrameWorkElementServiseLogic
    {
        void CopyImageToClipboard(FrameworkElement element);
        void SaveImageToFile(FrameworkElement element);
    }
}