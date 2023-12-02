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
    public partial class SurroundDataView : Window
    {
        private SurroundDataViewModel viewModel;
        public SurroundDataView(SurroundData surroundData) : this(new SurroundDataViewModel(surroundData)) { }
        public SurroundDataView(SurroundDataViewModel vm)
        {
            viewModel = vm;
            viewModel.ParentWindow = this;
            DataContext = viewModel;
            InitializeComponent();
        }

        private void XmaxChanged(object sender, EventArgs e)
        {
            viewModel.XMax *= ChangeValue(sender);
        }
        private void XminChanged(object sender, EventArgs e)
        {
            viewModel.XMin *= ChangeValue(sender);
        }

        private void YmaxChanged(object sender, EventArgs e)
        {
            viewModel.YMax *= ChangeValue(sender);
        }
        private void YminChanged(object sender, EventArgs e)
        {
            viewModel.YMin *= ChangeValue(sender);
        }
        private void ConstZChanged(object sender, EventArgs e)
        {
            viewModel.ConstZ *= ChangeValue(sender);
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
