using StructureHelperCommon.Models.Shapes;
using System.Windows;

namespace StructureHelper.Windows.Shapes
{
    /// <summary>
    /// Interaction logic for RingShapeView.xaml
    /// </summary>
    public partial class RingShapeView : Window
    {
        RingShapeViewModel viewModel;
        public RingShapeView(IRingShape ringShape) : this(new RingShapeViewModel(ringShape))
        {
            
        }
        public RingShapeView(RingShapeViewModel viewModel)
        {
            InitializeComponent();
            viewModel.ParentWindow = this;
            this.viewModel = viewModel;
            this.DataContext = viewModel;
        }
    }
}
