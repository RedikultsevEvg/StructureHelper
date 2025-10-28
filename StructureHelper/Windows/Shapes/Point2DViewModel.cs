using StructureHelper.Infrastructure;
using StructureHelperCommon.Models.Shapes;
using System;

namespace StructureHelper.Windows.Shapes
{
    public class Point2DViewModel : ViewModelBase
    {
        private readonly IPoint2D center;
        private readonly IPoint2D point;


        public double X
        {
            get => point.X + center.X;
            set
            {
                try
                {
                    double val = value;
                    point.X = val - center.X;
                    OnPropertyChanged(nameof(X));
                }
                catch (Exception ex)
                {
                    //Nothing to do
                }
            }
        }

        public double Y
        {
            get => point.Y + center.Y;
            set
            {
                try
                {
                    double val = value;
                    point.Y = val - center.Y;
                    OnPropertyChanged(nameof(Y));
                }
                catch (Exception ex)
                {
                    //Nothing to do
                }
            }
        }

        public void Refresh()
        {
            OnPropertyChanged(nameof(X));
            OnPropertyChanged(nameof(Y));

        }

        public Point2DViewModel(IPoint2D point, IPoint2D center)
        {
            this.point = point;
            this.center = center;
        }
        public Point2DViewModel(IPoint2D point) : this (point, new Point2D() { X = 0, Y = 0})
        {    }
    }
}
