using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using StructureHelper.Annotations;

namespace StructureHelper
{
    public class PrimitiveDefinitionBase : INotifyPropertyChanged
    {
        private bool captured, parameterCaptured, elementLock;

        public bool Captured
        {
            set
            {
                captured = value;
                OnPropertyChanged();
            }
            get => captured;
        }
        public bool ParameterCaptured
        {
            set
            {
                parameterCaptured = value;
                OnPropertyChanged();
            }
            get => parameterCaptured;
        }
        public bool ElementLock
        {
            get => elementLock;
            set
            {
                elementLock = value;
                OnPropertyChanged();
            }
        }
        private SolidColorBrush brush = null;
        public SolidColorBrush Brush
        {
            get => brush;
            set
            {
                brush = value;
                OnPropertyChanged();
            }
        }
        private MaterialDefinitionBase material = null;
        public MaterialDefinitionBase Material
        {
            get => material;
            set
            {
                material = value;
                MaterialName = material.MaterialClass;
                OnPropertyChanged();
                OnPropertyChanged(nameof(MaterialName));
            }
        }
        private string materialName = string.Empty;
        public string MaterialName
        {
            get => materialName;
            set
            {
                materialName = value;
                OnPropertyChanged();
            }
        }
        private bool paramsPanelVisibilty;
        public bool ParamsPanelVisibilty
        {
            get => paramsPanelVisibilty;
            set
            {
                paramsPanelVisibilty = value;
                OnPropertyChanged();
            }
        }
        private bool popupCanBeClosed = true;
        public bool PopupCanBeClosed
        {
            get => popupCanBeClosed;
            set
            {
                popupCanBeClosed = value;
                OnPropertyChanged();
            }
        }
        private double opacity = 1;
        private double showedOpacity = 0;
        public double ShowedOpacity
        {
            get => showedOpacity;
            set
            {
                showedOpacity = value;
                Opacity = (100 - value) / 100;
                OnPropertyChanged(nameof(Opacity));
                OnPropertyChanged();
            }
        }
        public double Opacity
        {
            get => opacity;
            set
            {
                opacity = value;
                OnPropertyChanged();
            }
        }
        private int showedZIndex = 1;
        public int ShowedZIndex
        {
            get => showedZIndex;
            set
            {
                showedZIndex = value;
                ZIndex = value - 1;
                OnPropertyChanged(nameof(ZIndex));
                OnPropertyChanged();
            }
        }

        private int zIndex;
        public int ZIndex
        {
            get => zIndex;
            set
            {
                zIndex = value;
                OnPropertyChanged();
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
