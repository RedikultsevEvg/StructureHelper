using System.Windows;

namespace StructureHelper
{
    public partial class MainView : Window
    {
        public MainView()
        {
            var model = new MainModel();
            var viewModel = new MainViewModel(model, this);
            DataContext = viewModel;
            InitializeComponent();
        }
    }
}
