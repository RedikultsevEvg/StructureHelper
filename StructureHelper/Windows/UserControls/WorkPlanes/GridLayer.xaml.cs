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
    /// Логика взаимодействия для Gridlayer.xaml
    /// </summary>
    public partial class GridLayer : UserControl
    {
        public GridLayer()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty GridSizeProperty =
            DependencyProperty.Register(nameof(GridSize), typeof(double), typeof(GridLayer), new PropertyMetadata(10.0));

        public double GridSize
        {
            get => (double)GetValue(GridSizeProperty);
            set => SetValue(GridSizeProperty, value);
        }

        public static readonly DependencyProperty GridColorBrushProperty =
            DependencyProperty.Register(nameof(GridColorBrush), typeof(Brush), typeof(GridLayer), new PropertyMetadata(Brushes.LightGray));

        public Brush GridColorBrush
        {
            get => (Brush)GetValue(GridColorBrushProperty);
            set => SetValue(GridColorBrushProperty, value);
        }

        public static readonly DependencyProperty GridLineThicknessProperty =
            DependencyProperty.Register(nameof(GridLineThickness), typeof(double), typeof(GridLayer), new PropertyMetadata(0.5));

        public double GridLineThickness
        {
            get => (double)GetValue(GridLineThicknessProperty);
            set => SetValue(GridLineThicknessProperty, value);
        }

        public static readonly DependencyProperty CanvasViewportSizeProperty =
            DependencyProperty.Register(nameof(CanvasViewportSize), typeof(Rect), typeof(GridLayer), new PropertyMetadata(new Rect(0, 0, 20, 20)));

        public Rect CanvasViewportSize
        {
            get => (Rect)GetValue(CanvasViewportSizeProperty);
            set => SetValue(CanvasViewportSizeProperty, value);
        }
    }

}
