using StructureHelper.Infrastructure;
using StructureHelper.Infrastructure.UI.DataContexts;
using StructureHelper.Windows.Graphs;
using StructureHelper.Windows.MainWindow;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace StructureHelper.Windows.UserControls
{
    /// <summary>
    /// Interaction logic for WorkPlane.xaml
    /// </summary>
    public partial class WorkPlane : UserControl
    {
        private IFrameWorkElementServiseLogic frameWorkElementServiseLogic = new FrameWorkElementServiseLogic();
        private RelayCommand saveImageCommand;
        private RelayCommand copyToClipboardCommand;

        public CrossSectionViewModel ViewModel
        {
            get { return (CrossSectionViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel), typeof(CrossSectionViewModel), typeof(WorkPlane), new PropertyMetadata(null));

        public ICommand SaveAsImageCommand
        {
            get => saveImageCommand ??= new RelayCommand(o => frameWorkElementServiseLogic.SaveImageToFile(WorkPlaneGrid));
        }

        public ICommand CopyToClipboardCommand
        {
            get => copyToClipboardCommand ??= new RelayCommand(o => frameWorkElementServiseLogic.CopyImageToClipboard(WorkPlaneGrid));
        }

        public WorkPlane()
        {
            InitializeComponent();
        }

        private void ContentPresenter_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var contentPresenter = sender as ContentPresenter;
            var item = contentPresenter?.Content as PrimitiveBase;
            ViewModel.PrimitiveLogic.SelectedItem = item;
        }
    }
}
