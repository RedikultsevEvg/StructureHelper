using System;
using System.Windows.Input;
using System.Windows.Media;
using StructureHelper.Infrastructure.Enums;
using StructureHelper.Models.Materials;
using StructureHelper.Windows.MainWindow;
using StructureHelperCommon.Models.Entities;
using StructureHelperCommon.Models.Materials;

namespace StructureHelper.Infrastructure.UI.DataContexts
{
    public abstract class PrimitiveBase: ViewModelBase
    {
        #region Поля

        private readonly PrimitiveType type;
        private bool captured, parameterCaptured, elementLock, paramsPanelVisibilty, popupCanBeClosed = true, borderCaptured;
        private Brush brush;
        private MaterialDefinitionBase material;
        private double opacity = 1, showedOpacity = 0, x, y, xY1, yX1, primitiveWidth, primitiveHeight, showedX, showedY, delta = 0.5;
        private int showedZIndex = 1, zIndex;

        #endregion

        #region Свойства

        public PrimitiveType Type => type;
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
                MaterialName = material.MaterialClass;
                OnPropertyChanged(value, ref material);
                OnPropertyChanged(nameof(MaterialName));
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

        #endregion

        #region Команды
        public ICommand PrimitiveLeftButtonDown { get; }
        public ICommand PrimitiveLeftButtonUp { get; }
        public ICommand RectanglePreviewMouseMove { get; }
        public ICommand PointPreviewMouseMove { get; }
        public ICommand PrimitiveDoubleClick { get; }

        #endregion
        
        protected PrimitiveBase(PrimitiveType type, double rectX, double rectY, MainViewModel mainViewModel)
        {
            this.type = type;
            Yx1 = rectX;
            Xy1 = rectY;
            var randomR = new Random().Next(150, 255);
            var randomG = new Random().Next(0, 255);
            var randomB = new Random().Next(30, 130);
            var color = Color.FromRgb((byte)randomR, (byte)randomG, (byte)randomB);
            Brush = new SolidColorBrush(color);
            PrimitiveLeftButtonUp = new RelayCommand(o => Captured = false);
            PrimitiveLeftButtonDown = new RelayCommand(o => Captured = true);
            RectanglePreviewMouseMove = new RelayCommand(o =>
            {
                if (!(o is Rectangle rect)) return;
                if (Captured && !rect.BorderCaptured && !ElementLock)
                {
                    var deltaX = PrimitiveWidth / 2;
                    var deltaY = PrimitiveHeight / 2;

                    if (rect.ShowedX % 10 <= delta || rect.ShowedX % 10 >= 10 - delta)
                        rect.ShowedX = Math.Round((mainViewModel.PanelX - deltaX - Yx1) / 10) * 10;
                    else
                        rect.ShowedX = mainViewModel.PanelX - deltaX - Yx1;

                    if (rect.ShowedY % 10 <= delta || rect.ShowedY % 10 >= 10 - delta)
                        rect.ShowedY = -(Math.Round((mainViewModel.PanelY - deltaY - Xy1 + rect.PrimitiveHeight) / 10) * 10);
                    else
                        rect.ShowedY = -(mainViewModel.PanelY - deltaY - Xy1 + rect.PrimitiveHeight);
                }
                if (ParameterCaptured)
                {
                    //RectParameterX = rect.ShowedX;
                    //RectParameterY = rect.ShowedY;
                    //RectParameterWidth = rect.PrimitiveWidth;
                    //RectParameterHeight = rect.PrimitiveHeight;
                    //ParameterOpacity = rect.ShowedOpacity;
                    //ElementLock = rect.ElementLock;
                }
            });
            PointPreviewMouseMove = new RelayCommand(o =>
            {
                if (!(o is Point point)) return;
                if (point.Captured && !point.ElementLock)
                {
                    var pointDelta = point.PrimitiveWidth / 2;

                    if (point.ShowedX % 10 <= pointDelta || point.ShowedX % 10 >= 10 - pointDelta)
                        point.ShowedX = Math.Round((mainViewModel.PanelX - Yx1) / 10) * 10;
                    else
                        point.ShowedX = mainViewModel.PanelX - pointDelta - Yx1;

                    if (point.ShowedY % 10 <= pointDelta || point.ShowedY % 10 >= 10 - pointDelta)
                        point.ShowedY = -(Math.Round((mainViewModel.PanelY - Xy1) / 10) * 10);
                    else
                        point.ShowedY = -(mainViewModel.PanelY - pointDelta - Xy1);
                }
                if (ParameterCaptured)
                {
                    //EllipseParameterX = point.ShowedX;
                    //EllipseParameterY = point.ShowedY;
                    //EllipseParameterSquare = point.Square;
                    //ParameterOpacity = point.ShowedOpacity;
                    //ElementLock = point.ElementLock;
                }
            });
            PrimitiveDoubleClick = new RelayCommand(o =>
            {
                PopupCanBeClosed = false;
                Captured = false;
                ParamsPanelVisibilty = true;

                //if (primitive is Rectangle rect)
                //    rect.BorderCaptured = false;
            });
            
        }

        private void UpdateCoordinatesX(double showedX)
        {
            if (Type == PrimitiveType.Rectangle) X = showedX + Yx1;
            if (Type == PrimitiveType.Point) X = showedX + Yx1 - PrimitiveWidth / 2;
        }
        private void UpdateCoordinatesY(double showedY)
        {
            if (Type == PrimitiveType.Rectangle) Y = -showedY + Xy1 - PrimitiveHeight;
            if (Type == PrimitiveType.Point) Y = -showedY + Xy1 - PrimitiveWidth / 2;
        }

        public abstract INdmPrimitive GetNdmPrimitive();
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
