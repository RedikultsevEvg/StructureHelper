using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace StructureHelper.Windows.Services
{
    /// <summary>
    /// Dialog for entering the parameters of a polar copy operation.
    /// </summary>
    public partial class CopyByAngleView : Window
    {
        public CopyByAngleView(CopyByAngleViewModel viewModel)
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
