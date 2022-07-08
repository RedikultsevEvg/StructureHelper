using System;
using System.Windows.Media;
using StructureHelper.Infrastructure.Enums;
using StructureHelper.Windows.MainWindow;

namespace StructureHelper.Infrastructure.UI.DataContexts
{
    public class Ellipse : PrimitiveBase
    {
        private double square;
        public double Square
        {
            get => square;
            set
            {
                square = value;
                PrimitiveWidth = Math.Round(Math.Sqrt(4 * value / Math.PI), 2);
                OnPropertyChanged(nameof(PrimitiveWidth));
                OnPropertyChanged();
            }
        }

        public Ellipse(double square, double ellipseX, double ellipseY, MainViewModel mainViewModel) : base(PrimitiveType.Ellipse, ellipseX, ellipseY, mainViewModel)
        {
            Square = square;
            ShowedX = 0;
            ShowedY = 0;
        }
    }
}
