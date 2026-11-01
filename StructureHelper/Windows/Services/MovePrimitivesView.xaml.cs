using System.Windows;

namespace StructureHelper.Windows.Services
{
    /// <summary>
    /// Dialog for entering the offset used to move primitives.
    /// </summary>
    public partial class MovePrimitivesView : Window
    {
        public MovePrimitivesView(MovePrimitivesViewModel viewModel)
        {
            InitializeComponent();
            viewModel.ParentWindow = this;
            DataContext = viewModel;
        }
    }
}
