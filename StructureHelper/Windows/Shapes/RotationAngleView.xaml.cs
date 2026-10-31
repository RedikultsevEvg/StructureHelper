using System.Windows;

namespace StructureHelper.Windows.Shapes
{
    /// <summary>
    /// Dialog for entering the polygon rotation angle.
    /// </summary>
    public partial class RotationAngleView : Window
    {
        public RotationAngleView(RotationAngleViewModel viewModel)
        {
            InitializeComponent();
            viewModel.ParentWindow = this;
            DataContext = viewModel;
        }
    }
}
