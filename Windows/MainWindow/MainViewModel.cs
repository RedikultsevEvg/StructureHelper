using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using StructureHelper.Infrastructure;
using StructureHelper.Infrastructure.Extensions;
using StructureHelper.Infrastructure.UI.DataContexts;
using StructureHelper.MaterialCatalogWindow;
using StructureHelper.Services;
using StructureHelper.Windows.ColorPickerWindow;

namespace StructureHelper.Windows.MainWindow
{
    public class MainViewModel : ViewModelBase
    {
        private IPrimitiveService PrimitiveService { get; }
        private IPrimitiveRepository PrimitiveRepository { get; }
        private MainModel Model { get; }
        public ObservableCollection<PrimitiveBase> Primitives { get; set; }
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

        private double pointParameterX, pointParameterY, pointParameterSquare;
        public double EllipseParameterX
        {
            get => pointParameterX;
            set => OnPropertyChanged(value, ref pointParameterX);
        }

        public double EllipseParameterY
        {
            get => pointParameterY;
            set => OnPropertyChanged(value, ref pointParameterY);
        }

        public double EllipseParameterSquare
        {
            get => pointParameterSquare;
            set => OnPropertyChanged(value, ref pointParameterSquare);
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

        public ICommand LeftButtonDown { get; }
        public ICommand LeftButtonUp { get; }
        public ICommand PreviewMouseMove { get; }
        
        public ICommand SetParameters { get; }
        public ICommand ClearSelection { get; }
        
        public ICommand OpenMaterialCatalog { get; }
        public ICommand OpenMaterialCatalogWithSelection { get; }
        public ICommand SetColor { get; }
        public ICommand SetInFrontOfAll { get; }
        public ICommand SetInBackOfAll { get; }
        public ICommand ScaleCanvasDown { get; }
        public ICommand ScaleCanvasUp { get; }
        public ICommand AddEllipse { get; }
        public ICommand SetPopupCanBeClosedTrue { get; }
        public ICommand SetPopupCanBeClosedFalse { get; }

        private double delta = 0.5;
        
        public MainViewModel(MainModel model, IPrimitiveService primitiveService, IPrimitiveRepository primitiveRepository)
        {
            PrimitiveService = primitiveService;
            PrimitiveRepository = primitiveRepository;
            Model = model;

            CanvasWidth = 1500;
            CanvasHeight = 1000;
            XX2 = CanvasWidth;
            XY1 = CanvasHeight / 2;
            YX1 = CanvasWidth / 2;
            YY2 = CanvasHeight;

            
            
            LeftButtonUp = new RelayCommand(o =>
            {
                if (o is Rectangle rect) rect.BorderCaptured = false;
            });
            LeftButtonDown = new RelayCommand(o =>
            {
                if (o is Rectangle rect) rect.BorderCaptured = true;
            });
            PreviewMouseMove = new RelayCommand(o =>
            {
                if (o is Rectangle rect && rect.BorderCaptured && !rect.ElementLock)
                {
                    if (rect.PrimitiveWidth % 10 < delta || rect.PrimitiveWidth % 10 >= delta)
                        rect.PrimitiveWidth = Math.Round(PanelX / 10) * 10 - rect.X + 10;
                    else
                        rect.PrimitiveWidth = PanelX - rect.X + 10;

                    if (rect.PrimitiveHeight % 10 < delta || rect.PrimitiveHeight % 10 >= delta)
                        rect.PrimitiveHeight = Math.Round(PanelY / 10) * 10 - rect.Y + 10;
                    else
                        rect.PrimitiveHeight = PanelY - rect.Y + 10;
                }
            });
            SetParameters = new RelayCommand(o =>
            {
                var primitive = Primitives.FirstOrDefault(x => x.ParameterCaptured);
                if (primitive != null)
                {
                    primitive.ElementLock = ElementLock;
                    primitive.ShowedOpacity = ParameterOpacity;
                    Primitives.MoveElementToSelectedIndex(primitive, PrimitiveIndex);
                    foreach (var primitiveDefinition in Primitives)
                        primitiveDefinition.ShowedZIndex = Primitives.IndexOf(primitiveDefinition) + 1;

                    switch (primitive)
                    {
                        case Rectangle rectangle:
                            rectangle.ShowedX = RectParameterX;
                            rectangle.ShowedY = RectParameterY;
                            rectangle.PrimitiveWidth = RectParameterWidth;
                            rectangle.PrimitiveHeight = RectParameterHeight;
                            break;
                        case Point point:
                            point.Square = EllipseParameterSquare;
                            point.ShowedX = EllipseParameterX;
                            point.ShowedY = EllipseParameterY;
                            break;
                    }
                }
            });
            ClearSelection = new RelayCommand(o =>
            {
                var primitive = Primitives.FirstOrDefault(x => x.ParamsPanelVisibilty);
                if (primitive != null && primitive.PopupCanBeClosed)
                {
                    primitive.ParamsPanelVisibilty = false;
                    primitive.ParameterCaptured = false;
                }
            });
            
            
            OpenMaterialCatalog = new RelayCommand(o =>
            {
                var materialCatalogView = new MaterialCatalogView();
                materialCatalogView.ShowDialog();
            });
            OpenMaterialCatalogWithSelection = new RelayCommand(o =>
            {
                var primitive = o as PrimitiveBase;
                var materialCatalogView = new MaterialCatalogView(true, primitive);
                materialCatalogView.ShowDialog();
            });
            SetColor = new RelayCommand(o =>
            {
                var primitive = o as PrimitiveBase;
                var colorPickerView = new ColorPickerView(primitive);
                colorPickerView.ShowDialog();
            });
            SetInFrontOfAll = new RelayCommand(o =>
            {
                if (!(o is PrimitiveBase primitive)) return;
                foreach (var primitiveDefinition in Primitives)
                    if (primitiveDefinition.ShowedZIndex > primitive.ShowedZIndex && primitiveDefinition != primitive)
                        primitiveDefinition.ShowedZIndex--;
                primitive.ShowedZIndex = PrimitivesCount;
                OnPropertyChanged(nameof(primitive.ShowedZIndex));
            });
            SetInBackOfAll = new RelayCommand(o =>
            {
                if (!(o is PrimitiveBase primitive)) return;
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

            Primitives = new ObservableCollection<PrimitiveBase>();
            
            AddRectangle = new RelayCommand(o =>
            {
                var rectangle = new Rectangle(60, 40, YX1, XY1, this);
                Primitives.Add(rectangle);
                PrimitivesCount = Primitives.Count;
                PrimitiveRepository.Add(rectangle);
            });
            
            AddEllipse = new RelayCommand(o =>
            {
                var point = new Point(2000, YX1, XY1, this);
                Primitives.Add(point);
                PrimitivesCount = Primitives.Count;
                PrimitiveRepository.Add(point);
            });

            SetPopupCanBeClosedTrue = new RelayCommand(o =>
            {
                if (!(o is PrimitiveBase primitive)) return;
                primitive.PopupCanBeClosed = true;
            });
            SetPopupCanBeClosedFalse = new RelayCommand(o =>
            {
                if (!(o is PrimitiveBase primitive)) return;
                primitive.PopupCanBeClosed = false;
            });
        }
    }
}