using StructureHelper.Windows.UserControls;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces.LimitCurve;
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

namespace StructureHelper.Windows.CalculationWindows.CalculatorsViews.ForceCalculatorViews.ForceResultLogic
{
    /// <summary>
    /// Логика взаимодействия для SurroundDataView.xaml
    /// </summary>
    public partial class LimitCurveDataView : Window
    {
        private LimitCurveDataViewModel viewModel;
        public LimitCurveDataView(LimitCurveDataViewModel vm)
        {
            viewModel = vm;
            viewModel.ParentWindow = this;
            DataContext = viewModel;
            InitializeComponent();
            CurveData.LimitCurveViewModel = viewModel;
        }
    }
}
