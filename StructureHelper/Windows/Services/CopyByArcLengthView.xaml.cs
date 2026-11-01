using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace StructureHelper.Windows.Services
{
    /// <summary>
    /// Dialog for entering the parameters of a copy-along-arc operation.
    /// </summary>
    public partial class CopyByArcLengthView : Window
    {
        public CopyByArcLengthView(CopyByArcLengthViewModel viewModel)
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
