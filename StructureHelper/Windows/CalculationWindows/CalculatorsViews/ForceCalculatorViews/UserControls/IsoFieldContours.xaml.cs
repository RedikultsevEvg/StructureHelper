using System.Windows;
using System.Windows.Controls;

namespace StructureHelper.Windows.CalculationWindows.CalculatorsViews.ForceCalculatorViews
{
    /// <summary>
    /// Interaction logic for IsoFieldContours.xaml
    /// </summary>
    public partial class IsoFieldContours : UserControl
    {

        //public bool IsSelected { get; set; }
        //public ContoursRangeViewModel ContourRange
        //{
        //    get { return (ContoursRangeViewModel)GetValue(ContourRangeProperty); }
        //    set { SetValue(ContourRangeProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for ContourRange.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty ContourRangeProperty =
        //    DependencyProperty.Register(nameof(ContourRange), typeof(ContoursRangeViewModel), typeof(IsoFieldContours), new PropertyMetadata(null));


        public IsoFieldContours()
        {
            InitializeComponent();
            DataContext = this;
        }
    }
}
