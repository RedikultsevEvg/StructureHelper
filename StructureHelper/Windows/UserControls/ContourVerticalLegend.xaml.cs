using FieldVisualizer.Entities.ColorMaps;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace StructureHelper.Windows.UserControls
{
    /// <summary>
    /// Interaction logic for ContourVerticalLegend.xaml
    /// </summary>
    public partial class ContourVerticalLegend : UserControl
    {
        public ContourVerticalLegend()
        {
            InitializeComponent();
        }

        #region ValueColorRanges

        public ObservableCollection<IValueColorRange>? ValueColorRanges
        {
            get => (ObservableCollection<IValueColorRange>?)
                GetValue(ValueColorRangesProperty);

            set => SetValue(ValueColorRangesProperty, value);
        }

        public static readonly DependencyProperty ValueColorRangesProperty =
            DependencyProperty.Register(
                nameof(ValueColorRanges),
                typeof(ObservableCollection<IValueColorRange>),
                typeof(ContourVerticalLegend),
                new PropertyMetadata(null));

        #endregion
    }
}
