using System;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows.Media;
using StructureHelper.Infrastructure;
using StructureHelper.Infrastructure.UI.DataContexts;

namespace StructureHelper.Windows.ColorPickerWindow
{
    public class ColorPickerViewModel : ViewModelBase
    {
        private int red, green, blue;

        public int Red
        {
            get => red;
            set => OnColorItemChanged(value, ref red);
        }
        public int Green
        {
            get => green;
            set => OnColorItemChanged(value, ref green);
        }
        public int Blue
        {
            get => blue;
            set => OnColorItemChanged(value, ref blue);
        }

        private Brush selectedColor;
        public Brush SelectedColor
        {
            get => selectedColor;
            set => OnPropertyChanged(value, ref selectedColor);
        }
        public ICommand SetColor { get; }
        public ColorPickerViewModel(PrimitiveBase primitive)
        {
            if (primitive != null)
            {
                var solidBrush = (SolidColorBrush)primitive.Brush;
                Red = solidBrush.Color.R;
                Green = solidBrush.Color.G;
                Blue = solidBrush.Color.B;

                SetColor = new RelayCommand(o => primitive.Brush = SelectedColor);
            }
        }
        private void OnColorItemChanged(int value, ref int colorItem, [CallerMemberName] string propertyName = null)
        {
            if (value >= 0 && value <= 255 && Math.Abs(colorItem - value) > 0.001)
            {
                colorItem = value;
                OnPropertyChanged(propertyName);
                UpdateSelectedColor();
            }
        }
        private void UpdateSelectedColor()
        {
            var color = Color.FromRgb((byte)Red, (byte)Green, (byte)Blue);
            SelectedColor = new SolidColorBrush(color);
            OnPropertyChanged(nameof(SelectedColor));
        }
    }
}
