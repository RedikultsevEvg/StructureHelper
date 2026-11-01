using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace StructureHelper.Windows.Shapes
{
    /// <summary>
    /// Dialog for entering the polygon offset distance and direction.
    /// </summary>
    public partial class PolygonOffsetView : Window
    {
        public PolygonOffsetView(PolygonOffsetViewModel viewModel)
        {
            InitializeComponent();
            viewModel.ParentWindow = this;
            DataContext = viewModel;
        }

        private void NumericTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter || sender is not TextBox textBox)
            {
                return;
            }
            textBox.GetBindingExpression(TextBox.TextProperty)?.UpdateSource();
        }
    }
}
