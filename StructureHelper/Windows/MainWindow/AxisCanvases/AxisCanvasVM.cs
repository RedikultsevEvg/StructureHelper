using StructureHelper.Infrastructure;
using StructureHelper.Windows.ViewModels;
using StructureHelperCommon.Models.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace StructureHelper.Windows.MainWindow
{
    public class AxisCanvasVM : OkCancelViewModelBase, IRectangleShape
    {
        private double axisLineThickness;
        private double gridLineThickness;
        private double gridSize;
        private double width;
        private double height;
        private Color xAxisColor;
        private Color yAxisColor;
        private Color gridColor;

        /// <summary>
        /// Thickness of x-, and y- axis line
        /// </summary>
        public double AxisLineThickness
        {
            get => axisLineThickness;
            set
            {
                axisLineThickness = value;
                OnPropertyChanged(nameof(AxisLineThickness));
            }
        }
        /// <summary>
        /// Thickness of lines of coordinate mesh
        /// </summary>
        public double GridLineThickness
        {
            get => gridLineThickness;
            set
            {
                gridLineThickness = value;
                OnPropertyChanged(nameof(GridLineThickness));
            }
        }
        /// <summary>
        /// Size of coordinate mesh
        /// </summary>
        public double GridSize
        {
            get => gridSize; set
            {
                gridSize = value;
                OnPropertyChanged(nameof(GridSize));
            }
        }
        /// <summary>
        /// Width of work plane
        /// </summary>
        public double Width
        {
            get => width; set
            {
                width = value;
                OnPropertyChanged(nameof(Width));
            }
        }
        /// <summary>
        /// Height of work plane
        /// </summary>
        public double Height
        {
            get => height; set
            {
                height = value;
                OnPropertyChanged(nameof(Height));
            }
        }

        public double Angle { get; set; }

        public Color XAxisColor
        {
            get => xAxisColor; set
            {
                xAxisColor = value;
                OnPropertyChanged(nameof(XAxisColor));
            }
        }
        public Color YAxisColor
        {
            get => yAxisColor; set
            {
                yAxisColor = value;
                OnPropertyChanged(nameof(YAxisColor));
            }
        }

        public Color GridColor
        {
            get => gridColor; set
            {
                gridColor = value;
                OnPropertyChanged(nameof(GridColor));
            }
        }

        public AxisCanvasVM()
        {
            AxisLineThickness = 2d;
            GridLineThickness = 0.25d;
            GridSize = 0.05d;
            Width = 1.2d;
            Height = 1.2d;
            XAxisColor = Colors.Red;
            YAxisColor = Colors.ForestGreen;
            GridColor = Colors.DarkGray;
        }
    }
}
