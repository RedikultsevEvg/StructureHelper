using StructureHelper.Windows.ViewModels.Calculations.Calculators;
using StructureHelper.Windows.ViewModels.Materials;
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

namespace StructureHelper.Windows.CalculationWindows.CalculatorsViews
{
    /// <summary>
    /// Логика взаимодействия для CrackCalculatorInputDataView.xaml
    /// </summary>
    public partial class CrackCalculatorInputDataView : Window
    {
        private CrackCalculatorInputDataViewModel viewModel;

        public CrackCalculatorInputDataView(CrackCalculatorInputDataViewModel viewModel)
        {
            this.viewModel = viewModel;
            viewModel.ParentWindow = this;
            DataContext = viewModel;
            InitializeComponent();
        }
    }
}
