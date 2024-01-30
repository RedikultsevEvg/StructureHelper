using StructureHelper.Windows.CalculationWindows.CalculatorsViews.ForceCalculatorViews.ForceResultLogic;
using StructureHelper.Windows.UserControls;
using System;
using System.Windows;
using System.Windows.Controls;

namespace StructureHelper.Windows.CalculationWindows.CalculatorsViews.ForceCalculatorViews
{
    /// <summary>
    /// Логика взаимодействия для LimitCurveControl.xaml
    /// </summary>
    public partial class LimitCurveControl : UserControl
    {
        public static readonly DependencyProperty LimitCurveViewModelProperty =
        DependencyProperty.Register(
            "LimitCurveViewModel",
            typeof(LimitCurveDataViewModel),
            typeof(LimitCurveControl));

        public LimitCurveDataViewModel LimitCurveViewModel
        {
            get { return (LimitCurveDataViewModel)GetValue(LimitCurveViewModelProperty); }
            set
            {
                this.DataContext = value;
                SurData.ViewModel = new (value.SurroundData);
                //SurData.SurroundData = value.SurroundData;
                SurData.DataContext = SurData.ViewModel;
                SetValue(LimitCurveViewModelProperty, value);
            }
        }
        public LimitCurveControl()
        {
            InitializeComponent();
            //DataContext = this;
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
