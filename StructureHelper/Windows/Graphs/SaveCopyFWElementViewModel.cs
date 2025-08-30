using StructureHelper.Infrastructure;
using System.Windows;
using System.Windows.Input;

namespace StructureHelper.Windows.Graphs
{
    public class SaveCopyFWElementViewModel : ViewModelBase
    {
        private IFrameWorkElementServiseLogic frameWorkElementServiseLogic = new FrameWorkElementServiseLogic();
        private RelayCommand saveImageCommand;
        private RelayCommand copyToClipboardCommand;
        
        public ICommand SaveAsImageCommand
        {
            get => saveImageCommand ??= new RelayCommand(o => frameWorkElementServiseLogic.SaveImageToFile(FrameWorkElement));
        }

        public ICommand CopyToClipboardCommand
        {
            get => copyToClipboardCommand ??= new RelayCommand(o => frameWorkElementServiseLogic.CopyImageToClipboard(FrameWorkElement));
        }
        public FrameworkElement FrameWorkElement { get; set; }
    }
}
