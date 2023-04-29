using StructureHelper.Windows.ViewModels.Calculations.Calculators.GeometryCalculator;
using StructureHelperCommon.Models.Parameters;
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

namespace StructureHelper.Windows.CalculationWindows.CalculatorsViews.GeometryCalculator
{
    /// <summary>
    /// Логика взаимодействия для GeometryCalculatorResultView.xaml
    /// </summary>
    public partial class GeometryCalculatorResultView : Window
    {
        GeometryCalculatorResultViewModel viewModel;
        public GeometryCalculatorResultView(List<ITextParameter> textParameters)
        {
            viewModel = new GeometryCalculatorResultViewModel(textParameters);
            InitializeComponent();
            this.DataContext = viewModel;
        }
    }
}
