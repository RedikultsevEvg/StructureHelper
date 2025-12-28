using StructureHelper.Infrastructure;
using System.Windows;
using System.Windows.Input;

namespace StructureHelper.Windows.Graphs
{
    public class SaveCopyFWElementViewModel : ViewModelBase
    {
        private RelayCommand saveImageCommand;
        private RelayCommand copyToClipboardCommand;
        
        public IFrameWorkElementServiseLogic FrameWorkElementServiseLogic { get; set; } = new FrameWorkElementServiseLogic();
        public ICommand SaveAsImageCommand
        {
            get => saveImageCommand ??= new RelayCommand(o => FrameWorkElementServiseLogic.SaveImageToFile(FrameWorkElement));
        }

        public ICommand CopyToClipboardCommand
        {
            get => copyToClipboardCommand ??= new RelayCommand(o => FrameWorkElementServiseLogic.CopyImageToClipboard(FrameWorkElement));
        }
        public FrameworkElement FrameWorkElement { get; set; }
    }
}
