using System.Windows;
using StructureHelper.Services;
using StructureHelper.Services.Primitives;

namespace StructureHelper.Windows.MainWindow
{
    public partial class MainView : Window
    {
        public IPrimitiveRepository PrimitiveRepository { get; }

        public MainView(IPrimitiveRepository primitiveRepository, MainViewModel viewModel)
        {
            PrimitiveRepository = primitiveRepository;
            DataContext = viewModel;
            InitializeComponent();
        }
    }
}
