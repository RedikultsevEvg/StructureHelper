using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using StructureHelper.Infrastructure;
using StructureHelper.Infrastructure.Enums;
using StructureHelper.Infrastructure.Extensions;
using StructureHelper.Infrastructure.UI.DataContexts;
using StructureHelper.MaterialCatalogWindow;
using StructureHelper.Services;
using StructureHelper.Windows.ColorPickerWindow;
using StructureHelper.UnitSystem;

namespace StructureHelper.Windows.MainWindow
{
    public class MainViewModel : ViewModelBase
    {
        private readonly double scaleRate = 1.1;

        private IPrimitiveRepository PrimitiveRepository { get; }
        private readonly UnitSystemService unitSystemService;

        private MainModel Model { get; }
        public ObservableCollection<PrimitiveBase> Primitives { get; set; }

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

        public int PrimitivesCount => Primitives.Count;
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
        public ICommand AddPrimitive { get; }
        public ICommand LeftButtonDown { get; }
        public ICommand LeftButtonUp { get; }
        public ICommand PreviewMouseMove { get; }
        public ICommand ClearSelection { get; }
        public ICommand OpenMaterialCatalog { get; }
        public ICommand OpenMaterialCatalogWithSelection { get; }
        public ICommand OpenUnitsSystemSettings { get; }
        public ICommand SetColor { get; }
        public ICommand SetInFrontOfAll { get; }
        public ICommand SetInBackOfAll { get; }
        public ICommand ScaleCanvasDown { get; }
        public ICommand ScaleCanvasUp { get; }
        public ICommand Calculate { get; }
        public ICommand SetPopupCanBeClosedTrue { get; }
        public ICommand SetPopupCanBeClosedFalse { get; }
        public string UnitsSystemName => unitSystemService.GetCurrentSystem().Name;

        private double delta = 0.5;

        public MainViewModel(MainModel model, IPrimitiveRepository primitiveRepository, UnitSystemService unitSystemService)
        {
            PrimitiveRepository = primitiveRepository;
            Model = model;
            this.unitSystemService = unitSystemService;
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
            ClearSelection = new RelayCommand(o =>
            {
                var primitive = Primitives?.FirstOrDefault(x => x.ParamsPanelVisibilty);
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
            OpenUnitsSystemSettings = new RelayCommand(o =>
            {
                OnPropertyChanged(nameof(UnitsSystemName));
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
                    if (primitiveDefinition != primitive && primitiveDefinition.ShowedZIndex < primitive.ShowedZIndex)
                        primitiveDefinition.ShowedZIndex++;
                primitive.ShowedZIndex = 1;
                OnPropertyChanged(nameof(primitive.ShowedZIndex));
            });
            
            ScaleCanvasDown = new RelayCommand(o =>
            {
                ScrollPanelX = PanelX;
                ScrollPanelY = PanelY;
                ScaleValue *= scaleRate;
            });
            
            ScaleCanvasUp = new RelayCommand(o =>
            {
                ScrollPanelX = PanelX;
                ScrollPanelY = PanelY;
                ScaleValue /= scaleRate;
            });

            Primitives = new ObservableCollection<PrimitiveBase>();
            
            AddPrimitive = new RelayCommand(o =>
            {
                if (!(o is PrimitiveType primitiveType)) return;
                var primitive = primitiveType == PrimitiveType.Point 
                    ? (PrimitiveBase) new Point(50, YX1, XY1, this) 
                    : (PrimitiveBase) new Rectangle(60, 40, YX1, XY1, this);
                Primitives.Add(primitive);
                PrimitiveRepository.Add(primitive);
            });

            Calculate = new RelayCommand(o =>
            {
                model.Calculate(-50e3, 0d, 0d);
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