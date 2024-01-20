using StructureHelper.Windows.CalculationWindows.CalculatorsViews.ForceCalculatorViews.ForceResultLogic;
using StructureHelper.Windows.UserControls;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StructureHelper.Windows.CalculationWindows.CalculatorsViews.ForceCalculatorViews
{
    /// <summary>
    /// Логика взаимодействия для LimitCurveControl.xaml
    /// </summary>
    public partial class LimitCurveControl : UserControl
    {
        public static readonly DependencyProperty LimitCurveViewModelProperty =
        DependencyProperty.Register("LimitCurveViewModel", typeof(LimitCurveDataViewModel), typeof(LimitCurveControl));

        public LimitCurveDataViewModel LimitCurveViewModel
        {
            get { return (LimitCurveDataViewModel)GetValue(LimitCurveViewModelProperty); }
            set { SetValue(LimitCurveViewModelProperty, value); }
        }
        public LimitCurveControl()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void PointCountChanged(object sender, EventArgs e)
        {
            LimitCurveViewModel.PointCount = Convert.ToInt32(LimitCurveViewModel.PointCount * ChangeValue(sender));
        }

        private double ChangeValue(object sender)
        {
            var obj = (MultiplyDouble)sender;
            var factor = obj.DoubleFactor;
            return factor;
        }
    }
}
