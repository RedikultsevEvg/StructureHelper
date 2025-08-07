using StructureHelper.Infrastructure;
using StructureHelperCommon.Models.VisualProperties;
using StructureHelperCommon.Services.ColorServices;
using System.Windows.Input;
using System.Windows.Media;

namespace StructureHelper.Windows.UserControls
{
    public class PrimitiveVisualPropertyViewModel : ViewModelBase
    {
        private IPrimitiveVisualProperty visualProperty;
        private RelayCommand editColorCommand;

        public bool IsVisible
        {
            get => visualProperty.IsVisible;
            set
            {
                visualProperty.IsVisible = value;
                OnPropertyChanged(nameof(IsVisible));
            }
        }

        public int ZIndex
        {
            get => visualProperty.ZIndex;
            set
            {
                visualProperty.ZIndex = value;
                OnPropertyChanged(nameof(Color));
            }
        }

        public double Opacity
        {
            get => visualProperty.Opacity * 100d;
            set
            {
                if (value < 0d) { value = 0d; }
                if (value > 100d) { value = 100d; }
                visualProperty.Opacity = value / 100d;
                OnPropertyChanged(nameof(Opacity));
            }
        }

        public Color Color
        {
            get => visualProperty.Color;
            set
            {
                visualProperty.Color = value;
                OnPropertyChanged(nameof(Color));
            }
        }

        public ICommand EditColorCommand => editColorCommand ??= new RelayCommand(o => EditColor());
        private void EditColor()
        {
            Color color = Color;
            ColorProcessor.EditColor(ref color);
            Color = color;
        }

        public PrimitiveVisualPropertyViewModel(IPrimitiveVisualProperty visualProperty)
        {
            this.visualProperty = visualProperty;
        }
    }
}
