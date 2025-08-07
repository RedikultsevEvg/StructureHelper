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

namespace StructureHelper.Windows.UserControls.WorkPlanes
{
    /// <summary>
    /// Логика взаимодействия для AxisLayer.xaml
    /// </summary>
    public partial class AxisLayer : UserControl
    {
        public AxisLayer()
        {
            InitializeComponent();
        }

        public double HalfOfWidth => Width / 2;
        public double HalfOfHeight => Height / 2;

        public Brush XAxisColorBrush
        {
            get => (Brush)GetValue(XAxisColorBrushProperty);
            set => SetValue(XAxisColorBrushProperty, value);
        }
        public static readonly DependencyProperty XAxisColorBrushProperty =
            DependencyProperty.Register(nameof(XAxisColorBrush), typeof(Brush), typeof(AxisLayer), new PropertyMetadata(Brushes.Black));

        public Brush YAxisColorBrush
        {
            get => (Brush)GetValue(YAxisColorBrushProperty);
            set => SetValue(YAxisColorBrushProperty, value);
        }
        public static readonly DependencyProperty YAxisColorBrushProperty =
            DependencyProperty.Register(nameof(YAxisColorBrush), typeof(Brush), typeof(AxisLayer), new PropertyMetadata(Brushes.Black));

        public double AxisLineThickness
        {
            get => (double)GetValue(AxisLineThicknessProperty);
            set => SetValue(AxisLineThicknessProperty, value);
        }
        public static readonly DependencyProperty AxisLineThicknessProperty =
            DependencyProperty.Register(nameof(AxisLineThickness), typeof(double), typeof(AxisLayer), new PropertyMetadata(1.0));
    }

}
