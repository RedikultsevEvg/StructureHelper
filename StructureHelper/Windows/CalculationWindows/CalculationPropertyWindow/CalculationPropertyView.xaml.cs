using StructureHelper.Windows.ViewModels.Calculations.CalculationProperies;
using StructureHelperCommon.Infrastructures.Enums;
using System.Windows;
using System.Windows.Controls;

namespace StructureHelper.Windows.CalculationWindows.CalculationPropertyWindow
{
    /// <summary>
    /// Логика взаимодействия для CalculationPropertyView.xaml
    /// </summary>
    public partial class CalculationPropertyView : Window
    {
        private CalculationPropertyViewModel viewModel;
        public CalculationPropertyView(CalculationPropertyViewModel calculationProperty)
        {
            InitializeComponent();
            viewModel = calculationProperty;
            this.DataContext = viewModel;
            if (viewModel.LimitState == LimitStates.ULS) { LsCollapse.IsChecked = true; }
            else { LsServiceability.IsChecked = true; }
            if (viewModel.CalcTerm == CalcTerms.ShortTerm) { ShortLoads.IsChecked = true; }
            else { LongLoads.IsChecked = true; }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            viewModel.SaveProperties();
        }

        private void LsCollapse_Checked(object sender, RoutedEventArgs e)
        {
            var chBox = sender as RadioButton;
            if (chBox.IsChecked == true & viewModel != null)
            { viewModel.LimitState = LimitStates.ULS; }
        }

        private void LsServiceability_Checked(object sender, RoutedEventArgs e)
        {
            var chBox = sender as RadioButton;
            if (chBox.IsChecked == true & viewModel != null)
            { viewModel.LimitState = LimitStates.SLS; }
        }

        private void ShortLoads_Checked(object sender, RoutedEventArgs e)
        {
            var chBox = sender as RadioButton;
            if (chBox.IsChecked == true & viewModel != null) { viewModel.CalcTerm = CalcTerms.ShortTerm; }
        }

        private void LongLoads_Checked(object sender, RoutedEventArgs e)
        {
            var chBox = sender as RadioButton;
            if (chBox.IsChecked == true & viewModel != null) { viewModel.CalcTerm = CalcTerms.LongTerm; }
        }

        private void ForceGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var dg = sender as DataGrid;
        }
    }
}
