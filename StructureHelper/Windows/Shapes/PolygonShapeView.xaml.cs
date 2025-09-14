using System.Windows;

namespace StructureHelper.Windows.Shapes
{
    /// <summary>
    /// Логика взаимодействия для PolygonView.xaml
    /// </summary>
    public partial class PolygonView : Window
    {
        private PolygonShapeViewModel viewModel;
        public PolygonView(PolygonShapeViewModel viewModel)
        {
            InitializeComponent();
            this.viewModel = viewModel;
            viewModel.ParentWindow = this;
            this.DataContext = viewModel;
        }
    }
}
