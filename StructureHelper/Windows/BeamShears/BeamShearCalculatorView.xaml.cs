using System.Windows;

namespace StructureHelper.Windows.BeamShears
{
    /// <summary>
    /// Interaction logic for BeamShearCalculatorView.xaml
    /// </summary>
    public partial class BeamShearCalculatorView : Window
    {
        private BeamShearCalculatorViewModel viewModel;
        public BeamShearCalculatorView(BeamShearCalculatorViewModel viewModel)
        {
            InitializeComponent();
            this.viewModel = viewModel;
            this.viewModel.ParentWindow = this;
            this.DataContext = this.viewModel;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            viewModel.Refresh();
        }
    }
}
