using StructureHelper.Windows.UserControls;
using StructureHelper.Windows.ViewModels.Materials;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StructureHelper.Windows.CalculationWindows.CalculatorsViews.ForceCalculatorViews
{
    /// <summary>
    /// Логика взаимодействия для SorroundDataControl.xaml
    /// </summary>
    public partial class SurroundDataControl : UserControl
    {
    // Using a DependencyProperty as the backing store for SurroundData.
    // This enables animation, styling, binding, etc...
    public static readonly DependencyProperty SurroundDataProperty =
        DependencyProperty.Register(
            "SurroundData",
            typeof(SurroundData),
            typeof(SurroundDataControl),
            new PropertyMetadata(null, OnSurroundDataChanged));

    private static void OnSurroundDataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        SurroundDataControl surroundDataControl = (SurroundDataControl)d;
        SurroundData newValue = (SurroundData)e.NewValue;

        // Handle any additional logic when the SurroundData property changes

        // Example: Update ViewModel.SurroundData
        surroundDataControl.ViewModel.SurroundData = newValue;
    }

    private SurroundData surroundData;
    public SurroundDataViewModel ViewModel { get; private set; }

    public SurroundData SurroundData
        {
            get => (SurroundData)GetValue(SurroundDataProperty);
            set
            {
                SetValue(SurroundDataProperty, value);
            }
        }

        public SurroundDataControl()
    {
            if (SurroundData is null)
            {
                ViewModel = new SurroundDataViewModel(new());
            }
            else
            {
                ViewModel = new SurroundDataViewModel(SurroundData);
            }
            DataContext = this; // ViewModel;
        InitializeComponent();
    }

        private void XmaxChanged(object sender, EventArgs e)
        {
            ViewModel.XMax *= ChangeValue(sender);
        }
        private void XminChanged(object sender, EventArgs e)
        {
            ViewModel.XMin *= ChangeValue(sender);
        }

        private void YmaxChanged(object sender, EventArgs e)
        {
            ViewModel.YMax *= ChangeValue(sender);
        }
        private void YminChanged(object sender, EventArgs e)
        {
            ViewModel.YMin *= ChangeValue(sender);
        }
        private void ConstZChanged(object sender, EventArgs e)
        {
            ViewModel.ConstZ *= ChangeValue(sender);
        }


        private double ChangeValue(object sender)
        {
            var obj = (MultiplyDouble)sender;
            var factor = obj.DoubleFactor;
            return factor;
        }
    }
}
