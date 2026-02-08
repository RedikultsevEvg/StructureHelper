using StructureHelperCommon.Models.Shapes;
using System.Windows;

namespace StructureHelper.Windows.Shapes
{
    /// <summary>
    /// Interaction logic for VerticalTShapeView.xaml
    /// </summary>
    public partial class VerticalTShapeView : Window
    {
        private VerticalTShapeViewModel viewModel;
        public VerticalTShapeView(VerticalTShapeViewModel viewModel)
        {
            InitializeComponent();
            this.viewModel = viewModel;
            DataContext = this.viewModel;
        }

        public VerticalTShapeView(IVerticalTShape tshape) : this(new VerticalTShapeViewModel(tshape))
        {
            
        }
    }
}
