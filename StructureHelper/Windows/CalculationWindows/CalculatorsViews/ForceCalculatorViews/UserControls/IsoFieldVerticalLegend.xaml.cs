using System;
using System.Collections.Generic;
using System.Text;
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
    /// Interaction logic for IsoFieldVerticalLegend.xaml
    /// </summary>
    public partial class IsoFieldVerticalLegend : UserControl
    {


        public ContourLegendViewModel ColorRanges
        {
            get { return (ContourLegendViewModel)GetValue(ColorRangesProperty); }
            set { SetValue(ColorRangesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ColorRanges.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ColorRangesProperty =
            DependencyProperty.Register(nameof(ColorRanges), typeof(ContourLegendViewModel), typeof(IsoFieldVerticalLegend), new PropertyMetadata(null));


        public IsoFieldVerticalLegend()
        {
            InitializeComponent();
        }
    }
}
