using Autofac.Features.Metadata;
using StructureHelper.Infrastructure;
using StructureHelper.Infrastructure.UI.DataContexts;
using StructureHelper.Windows.ViewModels.NdmCrossSections;
using StructureHelperCommon.Models.Shapes;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Windows.Input;
using System.Windows.Media;

namespace StructureHelper.Windows.MainWindow
{
    public class CrossSectionVisualPropertyVM : ViewModelBase
    {
        private double scaleValue;
        private ICommand previewMouseMove;
        private double delta = 0.0005;
        private readonly double scaleRate = 1.1d;

        public AxisCanvasVM AxisCanvasVM { get; set; }

        /// <summary>
        /// Thickness of x-, and y- axis line
        /// </summary>
        public double AxisLineThickness => AxisCanvasVM.AxisLineThickness / scaleValue;

        /// <summary>
        /// Thickness of lines of coordinate mesh
        /// </summary>
        public double GridLineThickness => AxisCanvasVM.GridLineThickness / scaleValue;
        
        /// <summary>
        /// Size of coordinate mesh
        /// </summary>
        public double GridSize => AxisCanvasVM.GridSize;
        /// <summary>
        /// Width of work plane
        /// </summary>
        public double Width => AxisCanvasVM.Width;
        /// <summary>
        /// Height of work plane
        /// </summary>
        public double Height => AxisCanvasVM.Height;
        public double HalfOfWidth => AxisCanvasVM.Width / 2d;
        public double HalfOfHeight => AxisCanvasVM.Height / 2d;

        public string CanvasViewportSize
        {
            get
            {
                string s = GridSize.ToString();
                s = s.Replace(',', '.');
                return $"0,0,{s},{s}";
            }

        }

        public double ScaleValue
        {
            get => Math.Round(scaleValue);
            set
            {
                OnPropertyChanged(value, ref scaleValue);
                OnPropertyChanged(nameof(AxisLineThickness));
                OnPropertyChanged(nameof(GridLineThickness));
            }
        }

        public ICommand PreviewMouseMove
        {
            get => previewMouseMove ??= new RelayCommand(o => PreviewMouseMoveMethod(o));
        }
        private void PreviewMouseMoveMethod(object o)
        {
            if (o is RectangleViewPrimitive rect && rect.BorderCaptured && !rect.ElementLock)
            {
                if (rect.PrimitiveWidth % 10d < delta || rect.PrimitiveWidth % 10d >= delta)
                    rect.PrimitiveWidth = Math.Round(PanelX / 10d) * 10d - rect.PrimitiveLeft + 10d;
                else
                    rect.PrimitiveWidth = PanelX - rect.PrimitiveLeft + 10d;

                if (rect.PrimitiveHeight % 10d < delta || rect.PrimitiveHeight % 10d >= delta)
                    rect.PrimitiveHeight = Math.Round(PanelY / 10d) * 10d - rect.PrimitiveTop + 10d;
                else
                    rect.PrimitiveHeight = PanelY - rect.PrimitiveTop + 10d;
            }
        }

        public Brush XAxisColorBrush => new SolidColorBrush(AxisCanvasVM.XAxisColor);
        public Brush YAxisColorBrush => new SolidColorBrush(AxisCanvasVM.YAxisColor);
        public Brush GridColorBrush => new SolidColorBrush(AxisCanvasVM.GridColor);

        internal void Refresh()
        {
            OnPropertyChanged(nameof(Width));
            OnPropertyChanged(nameof(Height));
            OnPropertyChanged(nameof(HalfOfWidth));
            OnPropertyChanged(nameof(HalfOfHeight));
            OnPropertyChanged(nameof(GridSize));
            OnPropertyChanged(nameof(AxisLineThickness));
            OnPropertyChanged(nameof(GridLineThickness));
            OnPropertyChanged(nameof(CanvasViewportSize));
            OnPropertyChanged(nameof(XAxisColorBrush));
            OnPropertyChanged(nameof(YAxisColorBrush));
            OnPropertyChanged(nameof(GridColorBrush));
        }

        private double panelX, panelY, scrollPanelX, scrollPanelY;
        private ICommand scaleCanvasDown;
        private ICommand scaleCanvasUp;

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

        public ICommand ScaleCanvasDown => scaleCanvasDown ??= new RelayCommand(o =>
        {
            ScrollPanelX = PanelX;
            ScrollPanelY = PanelY;
            ScaleValue *= scaleRate;
        });
        public ICommand ScaleCanvasUp => scaleCanvasUp ??= new RelayCommand(o =>
        {
            ScrollPanelX = PanelX;
            ScrollPanelY = PanelY;
            ScaleValue /= scaleRate;
        });

        public CrossSectionViewModel ParentViewModel { get; set; }

        public CrossSectionVisualPropertyVM()
        {
            AxisCanvasVM = new();
        }
    }
}
