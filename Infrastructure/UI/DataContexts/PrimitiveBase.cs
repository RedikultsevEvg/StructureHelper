using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using StructureHelper.Infrastructure.Enums;
using StructureHelper.Infrastructure.UI.Converters.Units;
using StructureHelper.Models.Materials;
using StructureHelper.Services.Primitives;
using StructureHelper.UnitSystem.Systems;
using StructureHelper.Windows.MainWindow;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Strings;
using StructureHelperLogics.Models.Materials;
using StructureHelperCommon.Services.ColorServices;
using StructureHelperLogics.Models.Primitives;

namespace StructureHelper.Infrastructure.UI.DataContexts
{
    public abstract class PrimitiveBase : ViewModelBase
    {
        #region Поля
        private IPrimitiveRepository primitiveRepository;
        private readonly PrimitiveType type;
        private string name;
        private double centerX, centerY;
        private int minElementDivision;
        private double maxElementSize;
        private bool captured, parameterCaptured, elementLock, paramsPanelVisibilty, popupCanBeClosed = true, borderCaptured;
        private Brush brush;
        private bool setMaterialColor;
        private Color color;
        private IHeadMaterial headMaterial;
        private MaterialDefinitionBase material;
        private double prestrainKx, prestrainKy, prestrainEpsZ;
        private double opacity = 1, showedOpacity = 0, x, y, xY1, yX1, primitiveWidth, primitiveHeight, showedX, showedY;
        protected double delta = 0.5;
        private double stressValue;
        private double strainValue;
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

        public IPrimitiveRepository PrimitiveRepository => primitiveRepository;

        public string Name
        {
            get => name;
            set
            {
                OnPropertyChanged(value, ref name);
            }
        }
        public double CenterX
        {
            get => centerX;
            set
            {
                if (this is Rectangle)
                {
                    ShowedX = value - primitiveWidth / 2d;
                }
                else if (this is Point)
                {
                    Point point = this as Point;
                    ShowedX = value - point.Diameter / 2;
                }
                else { throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknown); }
                OnPropertyChanged(value, ref centerX);
            }
        }
        public double CenterY
        {
            get => centerY;
            set
            {           
                if (this is Rectangle)
                {
                    ShowedY = value - primitiveHeight / 2d;
                }
                else if (this is Point)
                {
                    Point point = this as Point;
                    ShowedY = value - point.Diameter / 2;
                }
                else { throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknown); }
                OnPropertyChanged(value, ref centerY);            
            }
        }
        public double PrestrainKx { get; set; }
        public double PrestrainKy { get; set; }
        public double PrestrainEpsZ { get; set; }

        public IHeadMaterial HeadMaterial
        {
            get => headMaterial;
            set
            {
                OnPropertyChanged(value, ref headMaterial);
                OnPropertyChanged(nameof(Color));
            }
        }

        public bool SetMaterialColor
        {
            get => setMaterialColor;
            set { OnPropertyChanged(value, ref setMaterialColor);}
        }
        public Color Color
        {
            get => ((setMaterialColor == true) & (headMaterial !=null))?  headMaterial.Color :color;
            set
            {
                SetMaterialColor = false;
                OnPropertyChanged(value, ref color);
            }
        }
        public int MinElementDivision
        {
            get => minElementDivision;
            set { OnPropertyChanged(value, ref minElementDivision); }
        }
        public double MaxElementSize
        {
            get => maxElementSize;
            set { OnPropertyChanged(value, ref maxElementSize); }
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
            get => new SolidColorBrush(Color);
            set { }
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
        public double StressValue
        {
            get { return stressValue; }
            set
            {
                OnPropertyChanged(value, ref stressValue);
            }
        }
        public double StrainValue
        {
            get { return strainValue; }
            set
            {
                OnPropertyChanged(value, ref strainValue);
            }
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
            color = ColorProcessor.GetRandomColor();
            PrimitiveLeftButtonUp = new RelayCommand(o => Captured = false);
            PrimitiveLeftButtonDown = new RelayCommand(o => Captured = true);
            
            PrimitiveDoubleClick = new RelayCommand(o =>
            {
                //PopupCanBeClosed = false;
                //Captured = false;
                //ParamsPanelVisibilty = true;
                //ParameterCaptured = true;

            });
            OwnerVm = ownerVM;
            SetMaterialColor = true;
            PrestrainKx = 0;
            PrestrainKy = 0;
            PrestrainEpsZ = 0;
        }

        protected readonly MainViewModel OwnerVm;

        private void UpdateCoordinatesX(double showedX)
        {
            if (this is Rectangle)
            {
                X = showedX + OwnerVm.YX1 / UnitConstatnts.LengthConstant;
            }
            else if (this is Point)
            {
                Point point = this as Point;
                X = showedX + OwnerVm.YX1 / UnitConstatnts.LengthConstant;
            }
            else { throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknown); }
        }
        private void UpdateCoordinatesY(double showedY)
        {
            if (this is Rectangle)
            {
                Y = -showedY + OwnerVm.XY1 / UnitConstatnts.LengthConstant - PrimitiveHeight;
            }
            else if (this is Point)
            {
                Point point = this as Point;
                Y = -showedY + OwnerVm.XY1 / UnitConstatnts.LengthConstant - point.Diameter;
            }
            else { throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknown); }
        }

        public abstract INdmPrimitive GetNdmPrimitive(IUnitSystem unitSystem);
        public MaterialTypes GetMaterialTypes()
        {
            MaterialTypes materialTypes;
            if (Material is ConcreteDefinition) { materialTypes = MaterialTypes.Concrete; }
            else if (Material is RebarDefinition) { materialTypes = MaterialTypes.Reinforcement; }
            else { throw new StructureHelperException(ErrorStrings.MaterialTypeIsUnknown); }
            return materialTypes;
        }
    }
}
