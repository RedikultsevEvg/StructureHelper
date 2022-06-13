using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using StructureHelper.Infrastructure;

namespace StructureHelper
{
    public class MainViewModel : ViewModelBase
    {
        private MainModel rectanglesModel;
        private MainView rectanglesView;
        public ObservableCollection<RectangleDefinition> Rectangles { get; set; }
        public ObservableCollection<EllipseDefinition> Ellipses { get; set; }
        public ObservableCollection<PrimitiveDefinition> Primitives { get; set; }
        public ICommand AddRectangle { get; }

        private double panelX, panelY, scrollPanelX, scrollPanelY;

        public double PanelX
        {
            get => panelX;
            set => OnPropertyChanged(value, ref panelX);
        }
        public double PanelY
        {
            get => panelY;
            set => OnPropertyChanged(value, ref panelY);
        }
        public double ScrollPanelX
        {
            get => scrollPanelX;
            set => OnPropertyChanged(value, ref scrollPanelX);
        }
        public double ScrollPanelY
        {
            get => scrollPanelY;
            set => OnPropertyChanged(value, ref scrollPanelY);
        }

        private double rectParameterX, rectParameterY, rectParameterWidth, rectParameterHeight;

        public double RectParameterX
        {
            get => rectParameterX;
            set => OnPropertyChanged(value, ref rectParameterX);
        }
        public double RectParameterY
        {
            get => rectParameterY;
            set => OnPropertyChanged(value, ref rectParameterY);
        }
        public double RectParameterWidth
        {
            get => rectParameterWidth;
            set => OnPropertyChanged(value, ref rectParameterWidth);
        }
        public double RectParameterHeight
        {
            get => rectParameterHeight;
            set => OnPropertyChanged(value, ref rectParameterHeight);
        }

        private double parameterOpacity = 0;

        public double ParameterOpacity
        {
            get => parameterOpacity;
            set
            {
                if (value >= 0 && value <= 100) 
                    OnPropertyChanged(value, ref parameterOpacity);
            }
        }

        private double ellipseParameterX, ellipseParameterY, ellipseParameterSquare;
        public double EllipseParameterX
        {
            get => ellipseParameterX;
            set => OnPropertyChanged(value, ref ellipseParameterX);
        }

        public double EllipseParameterY
        {
            get => ellipseParameterY;
            set => OnPropertyChanged(value, ref ellipseParameterY);
        }

        public double EllipseParameterSquare
        {
            get => ellipseParameterSquare;
            set => OnPropertyChanged(value, ref ellipseParameterSquare);
        }
        private bool elementLock;
        public bool ElementLock
        {
            get => elementLock;
            set => OnPropertyChanged(value, ref elementLock);
        }

        private int primitivesCount;
        public int PrimitivesCount
        {
            get => primitivesCount;
            set => OnPropertyChanged(value, ref primitivesCount);
        }

        private int primitiveIndex = 1;
        public int PrimitiveIndex
        {
            get => primitiveIndex;
            set
            {
                if (value >= 0 && value <= primitivesCount)
                    OnPropertyChanged(value, ref primitiveIndex);
            }
        }

        private double scaleValue = 1.0;
        public double ScaleValue
        {
            get => scaleValue;
            set => OnPropertyChanged(value, ref scaleValue);
        }

        private double canvasWidth, canvasHeight, xX2, xY1, yX1, yY2;
        public double CanvasWidth
        {
            get => canvasWidth;
            set => OnPropertyChanged(value, ref canvasWidth);
        }

        public double CanvasHeight
        {
            get => canvasHeight;
            set => OnPropertyChanged(value, ref canvasHeight);
        }

        public double XX2
        {
            get => xX2;
            set => OnPropertyChanged(value, ref xX2);
        }
        public double XY1
        {
            get => xY1;
            set => OnPropertyChanged(value, ref xY1);
        }
        public double YX1
        {
            get => yX1;
            set => OnPropertyChanged(value, ref yX1);
        }
        public double YY2
        {
            get => yY2;
            set => OnPropertyChanged(value, ref yY2);
        }

        private double delta = 0.5;

        public ICommand LeftButtonDown { get; }
        public ICommand LeftButtonUp { get; }
        public ICommand PreviewMouseMove { get; }
        public ICommand PrimitiveLeftButtonDown { get; }
        public ICommand PrimitiveLeftButtonUp { get; }
        public ICommand BorderPreviewMouseMove { get; }
        public ICommand PrimitiveDoubleClick { get; }
        public ICommand SetParameters { get; }
        public ICommand ClearSelection { get; }
        public ICommand SetPopupCanBeClosedTrue { get; }
        public ICommand SetPopupCanBeClosedFalse { get; }
        public ICommand OpenMaterialCatalog { get; }
        public ICommand OpenMaterialCatalogWithSelection { get; }
        public ICommand SetColor { get; }
        public ICommand SetInFrontOfAll { get; }
        public ICommand SetInBackOfAll { get; }
        public ICommand ScaleCanvasDown { get; }
        public ICommand ScaleCanvasUp { get; }
        public ICommand AddEllipse { get; }
        public ICommand EllipsePreviewMouseMove { get; }

        public MainViewModel() { }
        public MainViewModel(MainModel rectanglesModel, MainView rectanglesView)
        {
            this.rectanglesModel = rectanglesModel;
            this.rectanglesView = rectanglesView;

            CanvasWidth = 1500;
            CanvasHeight = 1000;
            XX2 = CanvasWidth;
            XY1 = CanvasHeight / 2;
            YX1 = CanvasWidth / 2;
            YY2 = CanvasHeight;

            BorderPreviewMouseMove = new RelayCommand(o =>
            {
                var rect = o as RectangleDefinition;
                if (rect.Captured && !rect.BorderCaptured && !rect.ElementLock)
                {
                    var deltaX = rect.BorderWidth / 2;
                    var deltaY = rect.BorderHeight / 2;

                    if (rect.ShowedRectX % 10 <= delta || rect.ShowedRectX % 10 >= 10 - delta)
                        rect.ShowedRectX = Math.Round((PanelX - deltaX - YX1) / 10) * 10;
                    else
                        rect.ShowedRectX = PanelX - deltaX - YX1;

                    if (rect.ShowedRectY % 10 <= delta || rect.ShowedRectY % 10 >= 10 - delta)
                        rect.ShowedRectY = -(Math.Round((PanelY - deltaY - XY1 + rect.BorderHeight) / 10) * 10);
                    else
                        rect.ShowedRectY = -(PanelY - deltaY - XY1 + rect.BorderHeight);
                }
                if (rect.ParameterCaptured)
                {
                    RectParameterX = rect.ShowedRectX;
                    RectParameterY = rect.ShowedRectY;
                    RectParameterWidth = rect.BorderWidth;
                    RectParameterHeight = rect.BorderHeight;
                    ParameterOpacity = rect.ShowedOpacity;
                    ElementLock = rect.ElementLock;
                }
            });
            PrimitiveLeftButtonUp = new RelayCommand(o =>
            {
                var primitive = o as PrimitiveDefinition;
                primitive.Captured = false;
            });
            PrimitiveLeftButtonDown = new RelayCommand(o =>
            {
                var primitive = o as PrimitiveDefinition;
                primitive.Captured = true;
                foreach (var primitiveDefinition in Primitives)
                    primitiveDefinition.ParameterCaptured = false;
                primitive.ParameterCaptured = true;
            });
            LeftButtonUp = new RelayCommand(o =>
            {
                var rect = o as RectangleDefinition;
                rect.BorderCaptured = false;
            });
            LeftButtonDown = new RelayCommand(o =>
            {
                var rect = o as RectangleDefinition;
                rect.BorderCaptured = true;
            });
            PreviewMouseMove = new RelayCommand(o =>
            {
                var rect = o as RectangleDefinition;
                if (rect.BorderCaptured && rect.Captured && !rect.ElementLock)
                {
                    if (rect.BorderWidth % 10 < delta || rect.BorderWidth % 10 >= delta)
                        rect.BorderWidth = Math.Round(PanelX / 10) * 10 - rect.RectX + 10;
                    else
                        rect.BorderWidth = PanelX - rect.RectX + 10;

                    if (rect.BorderHeight % 10 < delta || rect.BorderHeight % 10 >= delta)
                        rect.BorderHeight = Math.Round(PanelY / 10) * 10 - rect.RectY + 10;
                    else
                        rect.BorderHeight = PanelY - rect.RectY + 10;
                }
            });
            SetParameters = new RelayCommand(o =>
            {
                var primitive = Primitives.FirstOrDefault(x => x.ParameterCaptured);
                primitive.ElementLock = ElementLock;
                primitive.ShowedOpacity = ParameterOpacity;
                Primitives.MoveElementToSelectedIndex(primitive, PrimitiveIndex);
                foreach (var primitiveDefinition in Primitives)
                    primitiveDefinition.ShowedZIndex = Primitives.IndexOf(primitiveDefinition) + 1;

                if (primitive is RectangleDefinition rectangle)
                {
                    rectangle.ShowedRectX = RectParameterX;
                    rectangle.ShowedRectY = RectParameterY;
                    rectangle.BorderWidth = RectParameterWidth;
                    rectangle.BorderHeight = RectParameterHeight;
                }

                if (primitive is EllipseDefinition ellipse)
                {
                    ellipse.Square = EllipseParameterSquare;
                    ellipse.ShowedEllipseX = EllipseParameterX;
                    ellipse.ShowedEllipseY = EllipseParameterY;
                }
            });
            ClearSelection = new RelayCommand(o =>
            {
                var primitive = Primitives.FirstOrDefault(x => x.ParamsPanelVisibilty);
                if (primitive != null && primitive.PopupCanBeClosed)
                    primitive.ParamsPanelVisibilty = false;
            });
            PrimitiveDoubleClick = new RelayCommand(o =>
            {
                var primitive = o as PrimitiveDefinition;
                primitive.PopupCanBeClosed = false;
                primitive.Captured = false;
                primitive.ParamsPanelVisibilty = true;

                if (primitive is RectangleDefinition rect)
                    rect.BorderCaptured = false;
            });
            SetPopupCanBeClosedTrue = new RelayCommand(o =>
            {
                var primitiveParamsVisible = Primitives.FirstOrDefault(x => x.ParameterCaptured);
                primitiveParamsVisible.PopupCanBeClosed = true;
            });
            SetPopupCanBeClosedFalse = new RelayCommand(o =>
            {
                var primitiveParamsVisible = Primitives.FirstOrDefault(x => x.ParameterCaptured);
                primitiveParamsVisible.PopupCanBeClosed = false;
            });
            OpenMaterialCatalog = new RelayCommand(o =>
            {
                var materialCatalogView = new MaterialCatalogView();
                materialCatalogView.ShowDialog();
            });
            OpenMaterialCatalogWithSelection = new RelayCommand(o =>
            {
                var primitive = o as PrimitiveDefinition;
                var materialCatalogView = new MaterialCatalogView(true, primitive);
                materialCatalogView.ShowDialog();
            });
            SetColor = new RelayCommand(o =>
            {
                var primitive = o as PrimitiveDefinition;
                var colorPickerView = new ColorPickerView(primitive);
                colorPickerView.ShowDialog();
            });
            SetInFrontOfAll = new RelayCommand(o =>
            {
                var primitive = o as PrimitiveDefinition;
                foreach (var primitiveDefinition in Primitives)
                    if (primitiveDefinition.ShowedZIndex > primitive.ShowedZIndex && primitiveDefinition != primitive)
                        primitiveDefinition.ShowedZIndex--;
                primitive.ShowedZIndex = PrimitivesCount;
                OnPropertyChanged(nameof(primitive.ShowedZIndex));
            });
            SetInBackOfAll = new RelayCommand(o =>
            {
                var primitive = o as PrimitiveDefinition;
                foreach (var primitiveDefinition in Primitives)
                    if (primitiveDefinition.ShowedZIndex < primitive.ShowedZIndex && primitiveDefinition != primitive)
                        primitiveDefinition.ShowedZIndex++;
                primitive.ShowedZIndex = 1;
                OnPropertyChanged(nameof(primitive.ShowedZIndex));
            });
            ScaleCanvasDown = new RelayCommand(o =>
            {
                var scaleRate = 1.1;
                ScrollPanelX = PanelX;
                ScrollPanelY = PanelY;
                ScaleValue *= scaleRate;
            });
            ScaleCanvasUp = new RelayCommand(o =>
            {
                var scaleRate = 1.1;
                ScrollPanelX = PanelX;
                ScrollPanelY = PanelY;
                ScaleValue /= scaleRate;
            });

            Primitives = new ObservableCollection<PrimitiveDefinition>();
            Rectangles = new ObservableCollection<RectangleDefinition>();
            Ellipses = new ObservableCollection<EllipseDefinition>();
            AddRectangle = new RelayCommand(o =>
            {
                var rectangle = new RectangleDefinition(60, 40, YX1, XY1);
                Rectangles.Add(rectangle);
                Primitives.Add(rectangle);
                PrimitivesCount = Primitives.Count;
            });
            AddEllipse = new RelayCommand(o =>
            {
                var ellipse = new EllipseDefinition(2000, YX1, XY1);
                Ellipses.Add(ellipse);
                Primitives.Add(ellipse);
                PrimitivesCount = Primitives.Count;
            });
            EllipsePreviewMouseMove = new RelayCommand(o =>
            {
                if (!(o is EllipseDefinition ellipse)) return;
                if (ellipse.Captured && !ellipse.ElementLock)
                {
                    var delta = ellipse.Diameter / 2;

                    if (ellipse.ShowedEllipseX % 10 <= delta || ellipse.ShowedEllipseX % 10 >= 10 - delta)
                        ellipse.ShowedEllipseX = Math.Round((PanelX - YX1) / 10) * 10;
                    else
                        ellipse.ShowedEllipseX = PanelX - delta - YX1;

                    if (ellipse.ShowedEllipseY % 10 <= delta || ellipse.ShowedEllipseY % 10 >= 10 - delta)
                        ellipse.ShowedEllipseY = -(Math.Round((PanelY - XY1) / 10) * 10);
                    else
                        ellipse.ShowedEllipseY = -(PanelY - delta - XY1);
                }
                if (ellipse.ParameterCaptured)
                {
                    EllipseParameterX = ellipse.ShowedEllipseX;
                    EllipseParameterY = ellipse.ShowedEllipseY;
                    EllipseParameterSquare = ellipse.Square;
                    ParameterOpacity = ellipse.ShowedOpacity;
                    ElementLock = ellipse.ElementLock;
                }
            });
        }
    }
}
