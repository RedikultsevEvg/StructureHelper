using StructureHelper.Windows.ViewModels;
using StructureHelperCommon.Models.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelper.Windows.Services
{
    public class CopyByParameterViewModel : OkCancelViewModelBase
    {
        private double deltaX;
        private double deltaY;
        private int copyCount;

        public double DeltaX
        {
            get => deltaX; set
            {
                deltaX = value;
                RefreshAngle();
                OnPropertyChanged(nameof(deltaX));
            }
        }


        public double DeltaY
        {
            get => deltaY; set
            {
                deltaY = value;
                RefreshAngle();
                OnPropertyChanged(nameof(deltaY));
            }
        }
        public double Angle { get; set; }
        public double Distance { get; set; }
        public int CopyCount
        {
            get => copyCount; set
            {
                int newValue;
                try
                {
                    newValue = value;
                    copyCount = newValue;
                    OnPropertyChanged(nameof(CopyCount));
                }
                catch (Exception)
                {

                }
            }
        }

        private IPoint2D originalCenter;

        public CopyByParameterViewModel(IPoint2D originalCenter)
        {
            deltaX = 0.2d;
            deltaY = 0d;
            copyCount = 1;
            this.originalCenter = originalCenter;
            RefreshAngle();
        }

        public List<IPoint2D> GetNewItemCenters()
        {
            var result = new List<IPoint2D>();
            for (int i = 1; i <= CopyCount; i++)
            {
                var newPoint = new Point2D()
                {
                    X = originalCenter.X + deltaX * i,
                    Y = originalCenter.Y + deltaY * i,
                };
                result.Add(newPoint);
            }
            return result;
        }
        private void RefreshAngle()
        {
            Angle = Math.Atan2(deltaX, deltaY)  * 180d / Math.PI - 90d;
            Angle = Math.Round(Angle, 1);
            Distance = Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
            Distance = Math.Round(Distance, 3);
            OnPropertyChanged(nameof(Angle));
            OnPropertyChanged(nameof(Distance));
        }
    }
}
