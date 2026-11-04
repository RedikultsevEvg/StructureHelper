using StructureHelper.Windows.ViewModels.Materials;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace StructureHelper.Windows.Materials
{
    /// <summary>
    /// Modal editor for one custom concrete model.
    /// </summary>
    public partial class CustomConcreteModelView : Window
    {
        private readonly CustomConcreteModelEditViewModel viewModel;

        public CustomConcreteModelView(CustomConcreteModelEditViewModel viewModel)
        {
            InitializeComponent();
            this.viewModel = viewModel;
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
