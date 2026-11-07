using StructureHelper.Windows.ViewModels.Materials;
using System.ComponentModel;
using System.Windows;

namespace StructureHelper.Windows.Materials
{
    public partial class SafetyFactorEditView : Window
    {
        private readonly SafetyFactorEditViewModel viewModel;

        public SafetyFactorEditView(SafetyFactorEditViewModel viewModel)
        {
            InitializeComponent();
            this.viewModel = viewModel;
            viewModel.ParentWindow = this;
            DataContext = viewModel;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (DialogResult != true)
            {
                viewModel.RestoreOriginalValues();
            }
            base.OnClosing(e);
        }
    }
}
