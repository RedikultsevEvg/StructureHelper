using StructureHelper.Windows.ViewModels.Calculations.CalculationResult;
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

namespace StructureHelper.Windows.CalculationWindows.CalculationResultWindow
{
    /// <summary>
    /// Логика взаимодействия для CalculationResultView.xaml
    /// </summary>
    public partial class CalculationResultView : Window
    {
        private CalculationResultViewModel viewModel;
        public CalculationResultView(CalculationResultViewModel resultViewModel)
        {
            InitializeComponent();
            viewModel = resultViewModel;
            this.DataContext = viewModel;
        }
    }
}
