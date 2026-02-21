using StructureHelperCommon.Models.Shapes;
using System.Windows;

namespace StructureHelper.Windows.Shapes
{
    /// <summary>
    /// Interaction logic for TrapezoidShapeView.xaml
    /// </summary>
    public partial class TrapezoidShapeView : Window
    {
        TrapezoidShapeViewModel viewModel;

        public TrapezoidShapeView(ITrapezoidShape trapezoidShape) : this (new TrapezoidShapeViewModel(trapezoidShape))
        {
            
        }
        public TrapezoidShapeView(TrapezoidShapeViewModel viewModel)
        {
            InitializeComponent();
            this.viewModel = viewModel;
            viewModel.ParentWindow = this;
            this.DataContext = viewModel;
        }
    }
}
