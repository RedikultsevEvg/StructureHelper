using StructureHelper.Infrastructure;
using StructureHelper.Windows.ViewModels;
using StructureHelperCommon.Models.Shapes;
using StructureHelperCommon.Models.WorkPlanes;
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
        private IWorkPlaneProperty workPlaneProperty;

        public AxisCanvasVM(IWorkPlaneProperty workPlaneProperty)
        {
            this.workPlaneProperty = workPlaneProperty;
        }
        private Color xAxisColor = Colors.Red;
        private Color yAxisColor = Colors.ForestGreen;
        private Color gridColor = Colors.DarkGray;

        /// <summary>
        /// Thickness of x-, and y- axis line
        /// </summary>
        public double AxisLineThickness
        {
            get => workPlaneProperty.AxisLineThickness;
            set
            {
                workPlaneProperty.AxisLineThickness = value;
                OnPropertyChanged(nameof(AxisLineThickness));
            }
        }
        /// <summary>
        /// Thickness of lines of coordinate mesh
        /// </summary>
        public double GridLineThickness
        {
            get => workPlaneProperty.GridLineThickness;
            set
            {
                workPlaneProperty.GridLineThickness = value;
                OnPropertyChanged(nameof(GridLineThickness));
            }
        }
        /// <summary>
        /// Size of coordinate mesh
        /// </summary>
        public double GridSize
        {
            get => workPlaneProperty.GridSize; set
            {
                workPlaneProperty.GridSize = value;
                OnPropertyChanged(nameof(GridSize));
            }
        }
        /// <summary>
        /// Width of work plane
        /// </summary>
        public double Width
        {
            get => workPlaneProperty.Width; set
            {
                workPlaneProperty.Width = value;
                OnPropertyChanged(nameof(Width));
            }
        }
        /// <summary>
        /// Height of work plane
        /// </summary>
        public double Height
        {
            get => workPlaneProperty.Height; set
            {
                workPlaneProperty.Height = value;
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
            get => gridColor;
            set
            {
                gridColor = value;
                OnPropertyChanged(nameof(GridColor));
            }
        }

        public Guid Id => throw new NotImplementedException();
    }
}
