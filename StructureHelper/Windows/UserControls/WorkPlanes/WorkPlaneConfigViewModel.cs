using StructureHelper.Infrastructure;
using System;
using System.Windows;
using System.Windows.Media;

namespace StructureHelper.Windows.UserControls.WorkPlanes
{
    public class WorkPlaneConfigViewModel : ViewModelBase
    {
        private double zoomCenterX;
        private double zoomCenterY;
        private double scaleValue = 200;
        private double canvasWidth = 3;
        private double canvasHeight = 3;
        private double axisFontSize = 14;
        private double axisLineThickness = 2;

        public double CanvasWidth
        {
            get => canvasWidth;
            set
            {
                canvasWidth = value;
                Refresh();
            }
        }
        public double CanvasHeight
        {
            get => canvasHeight;
            set
            {
                canvasHeight = value;
                Refresh();
            }
        }
        public double ScaleValue
        {
            get => scaleValue;
            set
            {
                scaleValue = value;
                Refresh();
            }
        }

        public double ZoomCenterX
        {
            get => zoomCenterX;
            set
            {
                zoomCenterX = value;
                Refresh();
            }
        }
        public double ZoomCenterY
        {
            get => zoomCenterY;
            set
            {
                zoomCenterY = value;
                Refresh();
            }
        }

        private void Refresh()
        {
            OnPropertyChanged(nameof(CanvasWidth));
            OnPropertyChanged(nameof(CanvasHeight));
            OnPropertyChanged(nameof(ScaleValue));
            OnPropertyChanged(nameof(ZoomCenterX));
            OnPropertyChanged(nameof(ZoomCenterY));
            OnPropertyChanged(nameof(AxisFontSize));
            OnPropertyChanged(nameof(AxisLineThickness));
            OnPropertyChanged(nameof(CenterOffsetX));
            OnPropertyChanged(nameof(CenterOffsetY));
        }

        public double CenterOffsetX => CanvasWidth / 2;
        public double CenterOffsetY => CanvasHeight / 2 * (-1);

        public double GridSize { get; set; } = 0.05;
        public Brush GridColorBrush { get; set; } = Brushes.LightGray;
        public double GridLineThickness { get; set; } = 0.005;

        public Size CanvasViewportSize => new(CanvasWidth, CanvasHeight);

        public Brush XAxisColorBrush { get; set; } = Brushes.DarkRed;
        public Brush YAxisColorBrush { get; set; } = Brushes.DarkGreen;
        public double AxisLineThickness
        {
            get
            {
                return axisLineThickness / scaleValue;
            }
        }
        public void ZoomAt(double actualHeight, double actualWidth, Point position, double zoomFactor)
        {
            // Optional: zoom to cursor
            //ZoomCenterX *= zoomFactor;// position.X / scaleValue ;// - CenterOffsetX;
            //ZoomCenterY *= zoomFactor;// position.Y / NegativeScaleValue;// - CenterOffsetY;
            ScaleValue = Math.Round(ScaleValue * zoomFactor, 2);

            var dx = (actualWidth / 2 - position.X) / ScaleValue * (zoomFactor - 1.0) * 2.0;
            var dy = (actualHeight / 2 - position.Y) / ScaleValue * (zoomFactor - 1.0) * 2.0;

            ZoomCenterX -= dx;
            ZoomCenterY += dy;

            // Update scale
        }

        public double AxisFontSize
        {
            get
            {
                return axisFontSize / scaleValue;
            }
        }
    }
}
