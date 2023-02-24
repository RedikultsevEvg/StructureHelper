using System.Windows;
using System.Windows.Controls;
using StructureHelper.Infrastructure.UI.DataContexts;
using StructureHelper.Services;
using StructureHelper.Services.Primitives;

namespace StructureHelper.Windows.MainWindow
{
    public partial class MainView : Window
    {
        private MainViewModel viewModel;
        public IPrimitiveRepository PrimitiveRepository { get; }

        public MainView(IPrimitiveRepository primitiveRepository, MainViewModel viewModel)
        {
            PrimitiveRepository = primitiveRepository;
            this.viewModel = viewModel;
            DataContext = this.viewModel;
            InitializeComponent();
        }

        private void ContentPresenter_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var contentPresenter = sender as ContentPresenter;
            var item = contentPresenter?.Content as PrimitiveBase;
            viewModel.PrimitiveLogic.SelectedItem = item;
        }
    }
}
