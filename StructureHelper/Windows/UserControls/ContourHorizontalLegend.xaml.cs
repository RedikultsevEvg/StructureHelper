using FieldVisualizer.Entities.ColorMaps;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;

namespace StructureHelper.Windows.UserControls
{
    /// <summary>
    /// Interaction logic for ContourHorizontalLegend.xaml
    /// </summary>
    public partial class ContourHorizontalLegend : UserControl
    {
        public ContourHorizontalLegend()
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
                typeof(ContourHorizontalLegend),
                new PropertyMetadata(
                    null,
                    OnValueColorRangesChanged));

        private static void OnValueColorRangesChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            var control = (ContourHorizontalLegend)d;

            if (e.OldValue is INotifyCollectionChanged oldCollection)
            {
                oldCollection.CollectionChanged -=
                    control.OnCollectionChanged;
            }

            if (e.NewValue is INotifyCollectionChanged newCollection)
            {
                newCollection.CollectionChanged +=
                    control.OnCollectionChanged;
            }

            control.Refresh();
        }

        private void OnCollectionChanged(
            object? sender,
            NotifyCollectionChangedEventArgs e)
        {
            Refresh();
        }

        #endregion

        #region MinValue

        private static readonly DependencyPropertyKey MinValuePropertyKey =
            DependencyProperty.RegisterReadOnly(
                nameof(MinValue),
                typeof(double),
                typeof(ContourHorizontalLegend),
                new PropertyMetadata(double.NaN));

        public static readonly DependencyProperty MinValueProperty =
            MinValuePropertyKey.DependencyProperty;

        public double MinValue =>
            (double)GetValue(MinValueProperty);

        #endregion

        #region MaxValue

        private static readonly DependencyPropertyKey MaxValuePropertyKey =
            DependencyProperty.RegisterReadOnly(
                nameof(MaxValue),
                typeof(double),
                typeof(ContourHorizontalLegend),
                new PropertyMetadata(double.NaN));

        public static readonly DependencyProperty MaxValueProperty =
            MaxValuePropertyKey.DependencyProperty;

        public double MaxValue =>
            (double)GetValue(MaxValueProperty);

        #endregion

        #region Refresh

        public void Refresh()
        {
            double minValue = double.NaN;
            double maxValue = double.NaN;

            if (ValueColorRanges is { Count: > 0 } ranges)
            {
                minValue = ranges[0].RoundedValues.BottomValue;
                maxValue = ranges[^1].RoundedValues.TopValue;
            }

            SetValue(MinValuePropertyKey, minValue);
            SetValue(MaxValuePropertyKey, maxValue);
        }

        #endregion
    }
}
