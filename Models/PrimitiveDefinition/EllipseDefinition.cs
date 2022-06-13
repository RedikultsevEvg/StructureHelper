using System;
using System.Windows.Media;

namespace StructureHelper
{
    public class EllipseDefinition : PrimitiveDefinitionBase
    {
        private double diameter, showedDiameter, ellipseX, ellipseY;
        public double Diameter
        {
            get => diameter;
            set
            {
                diameter = value;
                ShowedDiameter = Math.Round(value, 2);
                OnPropertyChanged();
            }
        }
        public double ShowedDiameter
        {
            get => showedDiameter;
            set
            {
                showedDiameter = value;
                OnPropertyChanged();
            }
        }

        public double EllipseX
        {
            get => ellipseX;
            set
            {
                ellipseX = value;
                OnPropertyChanged();
            }
        }
        public double EllipseY
        {
            get => ellipseY;
            set
            {
                ellipseY = value;
                OnPropertyChanged();
            }
        }
        
        
        private double showedEllipseX, showedEllipseY;
        private double initialEllipseX, initialEllipseY;
        public double ShowedEllipseX
        {
            get => showedEllipseX;
            set
            {
                showedEllipseX = value;
                EllipseX = value + initialEllipseX - Diameter / 2;
                OnPropertyChanged(nameof(EllipseX));
                OnPropertyChanged();
            }
        }
        public double ShowedEllipseY
        {
            get => showedEllipseY;
            set
            {
                showedEllipseY = value;
                EllipseY = -value + initialEllipseY - Diameter / 2;
                OnPropertyChanged(nameof(ShowedEllipseY));
                OnPropertyChanged();
            }
        }
        
        

        private double square;
        public double Square
        {
            get => square;
            set
            {
                square = value;
                Diameter = Math.Sqrt(4 * value / Math.PI);
                OnPropertyChanged(nameof(Diameter));
                OnPropertyChanged();
            }
        }

        public EllipseDefinition(double square, double ellipseX, double ellipseY)
        {
            Square = square;
            initialEllipseX = ellipseX;
            initialEllipseY = ellipseY;
            ShowedEllipseX = 0;
            ShowedEllipseY = 0;
            var randomR = new Random().Next(150, 255);
            var randomG = new Random().Next(0, 255);
            var randomB = new Random().Next(30, 130);
            var color = Color.FromRgb((byte)randomR, (byte)randomG, (byte)randomB);
            Brush = new SolidColorBrush(color);
        }
    }
}
