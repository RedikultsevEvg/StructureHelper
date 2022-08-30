using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using StructureHelper.Infrastructure.Enums;
using StructureHelper.Models.Materials;
using StructureHelper.UnitSystem.Systems;
using StructureHelper.Windows.MainWindow;
using StructureHelperCommon.Models.Entities;
using StructureHelperCommon.Models.Materials;

namespace StructureHelper.Infrastructure.UI.DataContexts
{
    public abstract class PrimitiveBase : ViewModelBase
    {
        #region Поля

        private readonly PrimitiveType type;
        private bool captured, parameterCaptured, elementLock, paramsPanelVisibilty, popupCanBeClosed = true, borderCaptured;
        private Brush brush;
        private MaterialDefinitionBase material;
        private double opacity = 1, showedOpacity = 0, x, y, xY1, yX1, primitiveWidth, primitiveHeight, showedX, showedY;
        protected double delta = 0.5;
        private int showedZIndex = 1, zIndex;

        #endregion

        #region Свойства

        public PrimitiveType Type
        {
            get => type;
            set
            {
                OnPropertyChanged(value, type);
                OnPropertyChanged(nameof(RectangleFieldVisibility));
                OnPropertyChanged(nameof(PrimitiveDimension));
                OnPropertyChanged(nameof(HeightRowHeight));
            }
        }

        public bool Captured
        {
            set => OnPropertyChanged(value, ref captured);
            get => captured;
        }
        public bool ParameterCaptured
        {
            set => OnPropertyChanged(value, ref parameterCaptured);
            get => parameterCaptured;
        }
        public bool ElementLock
        {
            get => elementLock;
            set => OnPropertyChanged(value, ref elementLock);
        }
        public Brush Brush
        {
            get => brush;
            set => OnPropertyChanged(value, ref brush);
        }
        public MaterialDefinitionBase Material
        {
            get => material;
            set
            {
                if (value != null)
                {
                    MaterialName = value.MaterialClass;
                    OnPropertyChanged(value, ref material);
                    OnPropertyChanged(nameof(MaterialName));
                }
            }
        }
        private string materialName = string.Empty;
        public string MaterialName
        {
            get => materialName;
            set => OnPropertyChanged(value, ref materialName);
        }
        public bool ParamsPanelVisibilty
        {
            get => paramsPanelVisibilty;
            set => OnPropertyChanged(value, ref paramsPanelVisibilty);
        }
        public bool PopupCanBeClosed
        {
            get => popupCanBeClosed;
            set => OnPropertyChanged(value, ref popupCanBeClosed);
        }
        public double ShowedOpacity
        {
            get => showedOpacity;
            set
            {
                Opacity = (100 - value) / 100;
                OnPropertyChanged(nameof(Opacity));
                OnPropertyChanged(value, ref showedOpacity);
            }
        }
        public double Opacity
        {
            get => opacity;
            set => OnPropertyChanged(value, ref opacity);
        }
        public int ShowedZIndex
        {
            get => showedZIndex;
            set
            {
                ZIndex = value - 1;
                OnPropertyChanged(nameof(ZIndex));
                OnPropertyChanged(value, ref showedZIndex);
            }
        }
        public int ZIndex
        {
            get => zIndex;
            set => OnPropertyChanged(value, ref zIndex);
        }
        public double X
        {
            get => x;
            set => OnPropertyChanged(value, ref x);
        }
        public double Y
        {
            get => y;
            set => OnPropertyChanged(value, ref y);
        }
        public double Xy1
        {
            get => xY1;
            set => OnPropertyChanged(value, ref xY1);
        }
        public double Yx1
        {
            get => yX1;
            set => OnPropertyChanged(value, ref yX1);
        }
        public double PrimitiveWidth
        {
            get => primitiveWidth;
            set => OnPropertyChanged(value, ref primitiveWidth);
        }
        public double PrimitiveHeight
        {
            get => primitiveHeight;
            set => OnPropertyChanged(value, ref primitiveHeight);
        }
        public double ShowedX
        {
            get => showedX;
            set
            {
                UpdateCoordinatesX(value);
                OnPropertyChanged(value, ref showedX);
                OnPropertyChanged(nameof(X));
            }
        }
        public double ShowedY
        {
            get => showedY;
            set
            {
                UpdateCoordinatesY(value);
                OnPropertyChanged(value, ref showedY);
                OnPropertyChanged(nameof(Y));
            }
        }
        public bool BorderCaptured
        {
            get => borderCaptured;
            set => OnPropertyChanged(value, ref borderCaptured);
        }

        public Visibility RectangleFieldVisibility => Type == PrimitiveType.Rectangle ? Visibility.Visible : Visibility.Hidden;
        public string PrimitiveDimension => Type == PrimitiveType.Rectangle ? "Ширина" : "Диаметр";
        public double HeightRowHeight => Type == PrimitiveType.Rectangle ? 40 : 0;

        #endregion

        #region Команды
        public ICommand PrimitiveLeftButtonDown { get; }
        public ICommand PrimitiveLeftButtonUp { get; }
        public ICommand PreviewMouseMove { get; protected set; }
        public ICommand PrimitiveDoubleClick { get; }

        #endregion

        protected PrimitiveBase(PrimitiveType type, double x, double y, MainViewModel ownerVM)
        {
            this.type = type;
            X = ownerVM.YX1 + x;
            Y = ownerVM.XY1 + y;
            var randomR = new Random(new Random((int)DateTime.Now.Ticks % 1000).Next(50)).Next(0, 255);
            var randomG = new Random(new Random((int)DateTime.Now.Ticks % 200).Next(100, 200)).Next(0, 255);
            var randomB = new Random(new Random((int)DateTime.Now.Ticks % 50).Next(500, 1000)).Next(0, 255);
            var color = Color.FromRgb((byte)randomR, (byte)randomG, (byte)randomB);
            Brush = new SolidColorBrush(color);
            PrimitiveLeftButtonUp = new RelayCommand(o => Captured = false);
            PrimitiveLeftButtonDown = new RelayCommand(o => Captured = true);
            
            PrimitiveDoubleClick = new RelayCommand(o =>
            {
                PopupCanBeClosed = false;
                Captured = false;
                ParamsPanelVisibilty = true;
                ParameterCaptured = true;
            });
            OwnerVm = ownerVM;
        }

        protected readonly MainViewModel OwnerVm;

        private void UpdateCoordinatesX(double showedX)
        {
            if (Type == PrimitiveType.Rectangle) X = showedX + OwnerVm.YX1;
            if (Type == PrimitiveType.Point) X = showedX + OwnerVm.YX1 - PrimitiveWidth / 2;
        }
        private void UpdateCoordinatesY(double showedY)
        {
            if (Type == PrimitiveType.Rectangle) Y = -showedY + OwnerVm.XY1 - PrimitiveHeight;
            if (Type == PrimitiveType.Point) Y = -showedY + OwnerVm.XY1 - PrimitiveWidth / 2;
        }

        public abstract INdmPrimitive GetNdmPrimitive(IUnitSystem unitSystem);
        public MaterialTypes GetMaterialTypes()
        {
            MaterialTypes materialTypes;
            if (Material is ConcreteDefinition) { materialTypes = MaterialTypes.Concrete; }
            else if (Material is RebarDefinition) { materialTypes = MaterialTypes.Reinforcement; }
            else { throw new Exception("MaterialType is unknown"); }
            return materialTypes;
        }
    }
}
