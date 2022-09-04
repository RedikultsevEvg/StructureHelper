using FieldVisualizer.Entities.ColorMaps;
using FieldVisualizer.Entities.ColorMaps.Factories;
using FieldVisualizer.Entities.Values;
using FieldVisualizer.Entities.Values.Primitives;
using FieldVisualizer.InfraStructures.Enums;
using FieldVisualizer.Services.ColorServices;
using FieldVisualizer.Services.PrimitiveServices;
using FieldVisualizer.Services.ValueRanges;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
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

namespace FieldVisualizer.Windows.UserControls
{
    /// <summary>
    /// Логика взаимодействия для FieldViewer.xaml
    /// </summary>
    public partial class FieldViewer : UserControl
    {
        public IPrimitiveSet _PrimitiveSet { get; set; }
        private double dX, dY;
        private ColorMapsTypes _ColorMapType;
        private IColorMap _ColorMap;
        private IValueRange _ValueRange;
        private IEnumerable<IValueRange> _ValueRanges;
        private IEnumerable<IValueColorRange> _ValueColorRanges;
        const int RangeNumber = 16;

        public FieldViewer()
        {
            InitializeComponent();
            _ColorMapType = ColorMapsTypes.FullSpectrum;
        }

        public void Refresh()
        {
            _ValueRange = PrimitiveOperations.GetValuRange(_PrimitiveSet.ValuePrimitives);
            _ValueRanges = ValueRangeOperations.DivideValueRange(_ValueRange, RangeNumber);
            _ColorMap = ColorMapFactory.GetColorMap(_ColorMapType);
            _ValueColorRanges = ColorOperations.GetValueColorRanges(_ValueRange, _ValueRanges, _ColorMap);
            if ((_PrimitiveSet is null) == false)
            {
                ProcessPrimitives();
                LegendViewer.ValueColorRanges = _ValueColorRanges;
                LegendViewer.Refresh();
            }
        }

        private void ProcessPrimitives()
        {
            WorkPlaneCanvas.Children.Clear();
            double sizeX = PrimitiveOperations.GetSizeX(_PrimitiveSet.ValuePrimitives);
            double sizeY = PrimitiveOperations.GetSizeY(_PrimitiveSet.ValuePrimitives);
            dX = PrimitiveOperations.GetMinMaxX(_PrimitiveSet.ValuePrimitives)[0];
            dY = PrimitiveOperations.GetMinMaxY(_PrimitiveSet.ValuePrimitives)[0];
            WorkPlaneCanvas.Width = Math.Abs(sizeX);
            WorkPlaneCanvas.Height = Math.Abs(sizeY);
            WorkPlaneBox.Width = WorkPlaneViewer.ActualWidth - 50;
            WorkPlaneBox.Height = WorkPlaneViewer.ActualHeight - 50;
            foreach (var primitive in _PrimitiveSet.ValuePrimitives)
            {
                if (primitive is IRectanglePrimitive)
                {
                    IRectanglePrimitive rectanglePrimitive = primitive as IRectanglePrimitive;
                    Rectangle rectangle = ProcessRectanglePrimitive(rectanglePrimitive);
                    WorkPlaneCanvas.Children.Add(rectangle);
                }
            }
        }

        private void WorkPlaneBox_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            WorkPlaneBox.Height *= WorkPlaneBox.ActualHeight * 0.5;
            WorkPlaneBox.Width *= WorkPlaneBox.ActualWidth * 0.5;
        }

        private Rectangle ProcessRectanglePrimitive(IRectanglePrimitive rectanglePrimitive)
        {
            Rectangle rectangle = new Rectangle();
            rectangle.Height = rectanglePrimitive.Height;
            rectangle.Width = rectanglePrimitive.Width;
            SolidColorBrush brush = new SolidColorBrush();
            brush.Color = ColorOperations.GetColorByValue(_ValueRange, _ColorMap, rectanglePrimitive.Value);
            foreach (var valueRange in _ValueColorRanges)
            {
                if (rectanglePrimitive.Value >= valueRange.BottomValue & rectanglePrimitive.Value <= valueRange.TopValue & (! valueRange.IsActive))
                {
                    brush.Color = Colors.Gray;
                }
            }
            rectangle.ToolTip = rectanglePrimitive.Value;
            rectangle.Fill = brush;
            Canvas.SetLeft(rectangle, rectanglePrimitive.CenterX - dX);
            Canvas.SetTop(rectangle, rectanglePrimitive.CenterY - dY);
            return rectangle;
        }

        private void RebuildButton_Click(object sender, RoutedEventArgs e)
        {
            ProcessPrimitives();
        }

        private void ZoomInButton_Click(object sender, RoutedEventArgs e)
        {
            WorkPlaneBox.Width *= 1.2;
            WorkPlaneBox.Height *= 1.2;
        }

        private void ChangeColorMapButton_Click(object sender, RoutedEventArgs e)
        {
            if (_ColorMapType < ColorMapsTypes.BlueToWhite) { _ColorMapType++;}
            else { _ColorMapType = 0;}
            Refresh();
        }

        private void ZoomOutButton_Click(object sender, RoutedEventArgs e)
        {
            WorkPlaneBox.Width *= 0.8;
            WorkPlaneBox.Height *= 0.8;
        }
    }
}
