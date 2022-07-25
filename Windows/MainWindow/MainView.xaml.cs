using System.Windows;
using StructureHelper.Services;

namespace StructureHelper.Windows.MainWindow
{
    public partial class MainView : Window
    {
        public IPrimitiveRepository PrimitiveRepository { get; }
        public IPrimitiveService PrimitiveService { get; }

        public MainView(IPrimitiveRepository primitiveRepository, IPrimitiveService primitiveService, MainViewModel viewModel)
        {
            PrimitiveRepository = primitiveRepository;
            PrimitiveService = primitiveService;
            DataContext = viewModel;
            InitializeComponent();
        }
    }
}
