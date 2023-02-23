using StructureHelper.Windows.ViewModels.Calculations.Calculators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace StructureHelper.Windows.CalculationWindows.CalculatorsViews.ForceCalculatorViews
{
    /// <summary>
    /// Логика взаимодействия для ForceCalculatorView.xaml
    /// </summary>
    public partial class ForceCalculatorView : Window
    {
        ForceCalculatorViewModel viewModel;

        public ForceCalculatorView(ForceCalculatorViewModel _forceCalculatorViewModel)
        {
            viewModel = _forceCalculatorViewModel;
            DataContext = viewModel;
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            viewModel.Refresh();
        }
    }
}
