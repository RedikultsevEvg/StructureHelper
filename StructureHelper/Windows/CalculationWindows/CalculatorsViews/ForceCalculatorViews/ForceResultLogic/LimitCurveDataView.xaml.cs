using StructureHelper.Windows.UserControls;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
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
        public LimitCurveDataView(SurroundData surroundData) : this(new LimitCurveDataViewModel(surroundData)) { }
        public LimitCurveDataView(LimitCurveDataViewModel vm)
        {
            viewModel = vm;
            viewModel.ParentWindow = this;
            DataContext = viewModel;
            InitializeComponent();
            SurData.SurroundData = vm.SurroundData;
        }
        private void PointCountChanged(object sender, EventArgs e)
        {
            viewModel.PointCount = Convert.ToInt32(viewModel.PointCount * ChangeValue(sender));
        }

        private double ChangeValue(object sender)
        {
            var obj = (MultiplyDouble)sender;
            var factor = obj.DoubleFactor;
            return factor;
        }
    }
}
