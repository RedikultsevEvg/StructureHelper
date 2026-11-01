using System.Windows;

namespace StructureHelper.Windows.PrimitivePropertiesWindow
{
    /// <summary>
    /// Window for selecting primitives separately for display and for contour plots.
    /// </summary>
    public partial class SelectPrimitivesForShowAndContoursView : Window
    {
        public SelectPrimitivesForShowAndContoursView(SelectPrimitivesForShowAndContoursViewModel viewModel)
        {
            InitializeComponent();
            viewModel.ParentWindow = this;
            DataContext = viewModel;
        }
    }
}
