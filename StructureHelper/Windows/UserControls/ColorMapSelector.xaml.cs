using FieldVisualizer.Entities.ColorMaps;
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

namespace StructureHelper.Windows.UserControls
{
    /// <summary>
    /// Interaction logic for ColorMapSelector.xaml
    /// </summary>
    public partial class ColorMapSelector : UserControl
    {
        public ColorMapSelector()
        {
            InitializeComponent();
        }

        #region ColorMaps

        public IEnumerable<IColorMap>? ColorMaps
        {
            get => (IEnumerable<IColorMap>?)GetValue(ColorMapsProperty);
            set => SetValue(ColorMapsProperty, value);
        }

        public static readonly DependencyProperty ColorMapsProperty =
            DependencyProperty.Register(
                nameof(ColorMaps),
                typeof(IEnumerable<IColorMap>),
                typeof(ColorMapSelector),
                new PropertyMetadata(null));

        #endregion

        #region SelectedColorMap

        public IColorMap? SelectedColorMap
        {
            get => (IColorMap?)GetValue(SelectedColorMapProperty);
            set => SetValue(SelectedColorMapProperty, value);
        }

        public static readonly DependencyProperty SelectedColorMapProperty =
            DependencyProperty.Register(
                nameof(SelectedColorMap),
                typeof(IColorMap),
                typeof(ColorMapSelector),
                new FrameworkPropertyMetadata(
                    null,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        #endregion

        #region RangeNumbers

        public IEnumerable<int>? RangeNumbers
        {
            get => (IEnumerable<int>?)GetValue(RangeNumbersProperty);
            set => SetValue(RangeNumbersProperty, value);
        }

        public static readonly DependencyProperty RangeNumbersProperty =
            DependencyProperty.Register(
                nameof(RangeNumbers),
                typeof(IEnumerable<int>),
                typeof(ColorMapSelector),
                new PropertyMetadata(null));

        #endregion

        #region RangeNumber

        public int RangeNumber
        {
            get => (int)GetValue(RangeNumberProperty);
            set => SetValue(RangeNumberProperty, value);
        }

        public static readonly DependencyProperty RangeNumberProperty =
            DependencyProperty.Register(
                nameof(RangeNumber),
                typeof(int),
                typeof(ColorMapSelector),
                new FrameworkPropertyMetadata(
                    0,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        #endregion
    }
}
