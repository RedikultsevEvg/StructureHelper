using System.Windows;

namespace StructureHelper.Windows.MainWindow.Analyses
{
    /// <summary>
    /// Interaction logic for OpenRecentFileView.xaml
    /// </summary>
    public partial class OpenRecentFileView : Window
    {
        private OpenRecentFileViewModel vm;

        public OpenRecentFileView(OpenRecentFileViewModel vm)
        {
            this.vm = vm;
            InitializeComponent();
            this.DataContext = vm;
            vm.ParentWindow = this;
        }
    }
}
