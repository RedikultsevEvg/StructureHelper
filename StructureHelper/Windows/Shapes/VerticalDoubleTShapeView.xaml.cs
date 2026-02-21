using StructureHelperCommon.Models.Shapes;
using System.Windows;

namespace StructureHelper.Windows.Shapes
{
    /// <summary>
    /// Interaction logic for VerticalDoubleTShapeView.xaml
    /// </summary>
    public partial class VerticalDoubleTShapeView : Window
    {
        VerticalDoubleTShapeViewModel viewModel;
        public VerticalDoubleTShapeView(IVerticalDoubleTShape doubleTShape) : this(new VerticalDoubleTShapeViewModel(doubleTShape))
        {
            
        }
        public VerticalDoubleTShapeView(VerticalDoubleTShapeViewModel viewModel)
        {
            InitializeComponent();
            this.viewModel = viewModel;
            this.viewModel.ParentWindow = this;
            DataContext = this.viewModel;
        }
    }
}
