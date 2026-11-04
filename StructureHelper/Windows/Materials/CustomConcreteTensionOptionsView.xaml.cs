using System.Windows.Controls;
using System.Windows.Input;

namespace StructureHelper.Windows.Materials
{
    public partial class CustomConcreteTensionOptionsView : UserControl
    {
        public CustomConcreteTensionOptionsView()
        {
            InitializeComponent();
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
