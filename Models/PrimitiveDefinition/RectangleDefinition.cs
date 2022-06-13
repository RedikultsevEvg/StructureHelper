using System;
using System.Windows.Media;

namespace StructureHelper
{
    public class RectangleDefinition : PrimitiveDefinitionBase
    {
        private bool borderCaptured = false;
        private double rectX, rectY, borderWidth, borderHeight;
        
        public bool BorderCaptured
        {
            set
            {
                borderCaptured = value;
                OnPropertyChanged();
            }
            get => borderCaptured;
        }
        
        public double RectX
        {
            get => rectX;
            set
            {
                rectX = value;
                OnPropertyChanged();
            }
        }
        public double RectY
        {
            get => rectY;
            set
            {
                rectY = value;
                OnPropertyChanged();
            }
        }
        private double showedRectX, showedRectY;
        private double initialRectX, initialRectY;
        public double ShowedRectX
        {
            get => showedRectX;
            set
            {
                showedRectX = value;
                RectX = value + initialRectX;
                OnPropertyChanged(nameof(RectX));
                OnPropertyChanged();
            }
        }
        public double ShowedRectY
        {
            get => showedRectY;
            set
            {
                showedRectY = value;
                RectY = -value + initialRectY - BorderHeight;
                OnPropertyChanged(nameof(RectY));
                OnPropertyChanged();
            }
        }
        public double BorderWidth
        {
            get => borderWidth;
            set
            {
                borderWidth = value;
                OnPropertyChanged();
            }
        }
        public double BorderHeight
        {
            get => borderHeight;
            set
            {
                borderHeight = value;
                OnPropertyChanged();
            }
        }

        public RectangleDefinition(double borderWidth, double borderHeight, double rectX, double rectY)
        {
            BorderWidth = borderWidth;
            BorderHeight = borderHeight;
            initialRectX = rectX;
            initialRectY = rectY;
            ShowedRectX = 0;
            ShowedRectY = 0;
            var randomR = new Random().Next(150, 255);
            var randomG = new Random().Next(0, 255);
            var randomB = new Random().Next(30, 130);
            var color = Color.FromRgb((byte)randomR, (byte)randomG, (byte)randomB);
            Brush = new SolidColorBrush(color);
        }
    }
}
