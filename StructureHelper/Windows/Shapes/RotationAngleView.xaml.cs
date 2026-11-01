using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace StructureHelper.Windows.Shapes
{
    /// <summary>
    /// Dialog for entering a rotation angle, its direction and the rotation centre.
    /// </summary>
    public partial class RotationAngleView : Window
    {
        public RotationAngleView(RotationAngleViewModel viewModel)
        {
            InitializeComponent();
            viewModel.ParentWindow = this;
            DataContext = viewModel;
        }

        /// <summary>
        /// Commits the numeric input before the default OK button handles Enter.
        /// </summary>
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
