using FieldVisualizer.Entities.ColorMaps;
using FieldVisualizer.Entities.ColorMaps.Factories;
using FieldVisualizer.Entities.Values;
using FieldVisualizer.Entities.Values.Primitives;
using FieldVisualizer.Infrastructure.Commands;
using FieldVisualizer.InfraStructures.Exceptions;
using FieldVisualizer.InfraStructures.Strings;
using FieldVisualizer.Services.ColorServices;
using FieldVisualizer.Services.PrimitiveServices;
using FieldVisualizer.Services.ValueRanges;
using FieldVisualizer.Windows.UserControls;
using StructureHelperCommon.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace FieldVisualizer.ViewModels.FieldViewerViewModels
{
    public class FieldViewerViewModel : ViewModelBase, IDataErrorInfo
    {
        private IMathRoundLogic roundLogic = new SmartRoundLogic() { DigitQuant = 3 };
        public ICommand RebuildCommand { get; }
        public ICommand ZoomInCommand { get; }
        public ICommand ZoomOutCommand { get; }
        public ICommand ChangeColorMapCommand { get; }
        public ICommand SetUserColorsCommand { get; }
        public ICommand SetCrossLineCommand
        {
            get
            {
                if (setCrossLineCommand == null)
                {
                    setCrossLineCommand = new RelayCommand(SetCrossLine);
                }

                return setCrossLineCommand;
            }
        }

        public IPrimitiveSet PrimitiveSet
        { get
            {
                return primitiveSet;
            }
            set
            {
                primitiveSet = value;
                OnPropertyChanged(nameof(PrimitiveSet));
                AreaTotal = primitiveSet is null ? 0 : primitiveSet.ValuePrimitives.Sum(x => x.Area);
                OnPropertyChanged(nameof(AreaTotal));
                AreaNeg = primitiveSet is null ? 0 : primitiveSet.ValuePrimitives.Where(x => x.Value < 0d).Sum(x => x.Area);
                OnPropertyChanged(nameof(AreaNeg));
                AreaZero = primitiveSet is null ? 0 : primitiveSet.ValuePrimitives.Where(x => x.Value == 0d).Sum(x => x.Area);
                OnPropertyChanged(nameof(AreaZero));
                AreaPos = primitiveSet is null ? 0 : primitiveSet.ValuePrimitives.Where(x => x.Value > 0d).Sum(x => x.Area);
                OnPropertyChanged(nameof(AreaPos));
                SumTotal = primitiveSet is null ? 0 : primitiveSet.ValuePrimitives.Sum(x => x.Value);
                OnPropertyChanged(nameof(SumTotal));
                SumNeg = primitiveSet is null ? 0 : primitiveSet.ValuePrimitives.Where(x => x.Value < 0d).Sum(x => x.Value);
                OnPropertyChanged(nameof(SumNeg));
                SumPos = primitiveSet is null ? 0 : primitiveSet.ValuePrimitives.Where(x => x.Value > 0d).Sum(x => x.Value);
                OnPropertyChanged(nameof(SumPos));

            }
        }
        public IValueRange UserValueRange { get; set; }
        public bool SetMinValue
        {
            get
            {
                return setMinValue;
            }
            set
            {
                setMinValue = value;
                OnPropertyChanged(nameof(SetMinValue));
                //OnPropertyChanged(nameof(UserMinValue));
            }
        }
        public bool SetMaxValue
        {
            get
            {
                return setMaxValue;
            }
            set
            {
                setMaxValue = value;
                OnPropertyChanged(nameof(SetMaxValue));
                //OnPropertyChanged(nameof(UserMaxValue));
            }
        }
        public double UserMinValue
        {
            get
            {
                return UserValueRange.BottomValue;
            }
            set
            {
                UserValueRange.BottomValue = value;
                OnPropertyChanged(nameof(UserMinValue));
            }
        }
        public double UserMaxValue
        {
            get
            {
                return UserValueRange.TopValue;
            }
            set
            {
                double tmpVal = UserValueRange.TopValue;
                try
                {
                    UserValueRange.TopValue = value;
                    OnPropertyChanged(nameof(UserMaxValue));
                }
                catch (Exception ex)
                {
                    UserValueRange.TopValue = tmpVal;
                }
            }
        }
        public double AreaTotal { get; private set; }
        public double AreaNeg { get; private set; }
        public double AreaZero { get; private set; }
        public double AreaPos { get; private set; }
        public double SumTotal { get; private set;}
        public double SumNeg { get; private set; }
        public double SumPos { get; private set; }
        public Viewbox WorkPlaneBox { get; set; }
        public VerticalLegend Legend { get; set; }
        public Canvas WorkPlaneCanvas { get; set; }
        public double ScrolWidth { get; set; }
        public double ScrolHeight { get; set; }
        public double CrossLineX { get => crossLineX; set => SetProperty(ref crossLineX, value); }
        public double CrossLineY { get => crossLineY; set => SetProperty(ref crossLineY, value); }

        public double SumAboveLine { get => sumAboveLine; set => SetProperty(ref sumAboveLine, value); }
        public double SumUnderLine { get => sumUnderLine; set => SetProperty(ref sumUnderLine, value); }

        public string Error { get; }
        public string this[string columnName]
        {
            get
            {
                string error = String.Empty;
                return error;
            }
        }

        private ICommand setCrossLineCommand;
        private IPrimitiveSet primitiveSet;
        private double dX, dY;
        private ColorMapsTypes _ColorMapType;
        private IColorMap _ColorMap;
        private IValueRange valueRange;
        private IEnumerable<IValueRange> valueRanges;
        private IEnumerable<IValueColorRange> valueColorRanges;
        private bool setMinValue;
        private bool setMaxValue;
        private double crossLineX;
        private double crossLineY;
        private double sumAboveLine;
        private double sumUnderLine;
        private Line previosLine;
        const int RangeNumber = 16;

        public FieldViewerViewModel()
        {
            _ColorMapType = ColorMapsTypes.LiraSpectrum;
            RebuildCommand = new RelayCommand(o => ProcessPrimitives(), o => PrimitiveValidation());
            ZoomInCommand = new RelayCommand(o => Zoom(1.2), o => PrimitiveValidation());
            ZoomOutCommand = new RelayCommand(o => Zoom(0.8), o => PrimitiveValidation());
            ChangeColorMapCommand = new RelayCommand(o => ChangeColorMap(), o => PrimitiveValidation());
            SetUserColorsCommand = new RelayCommand(o => ColorRefresh(), o => (SetMinValue || SetMaxValue));
            UserValueRange = new ValueRange() { BottomValue = 0, TopValue = 0 };
            SetMinValue = false;
            SetMaxValue = false;
        }
        public void ColorRefresh()
        {
            if (PrimitiveValidation() == false) { return; }
            _ColorMap = ColorMapFactory.GetColorMap(_ColorMapType);
            SetColor();
            if ((PrimitiveSet is null) == false)
            {
                ProcessPrimitives();
                Legend.ValueColorRanges = valueColorRanges;
                Legend.Refresh();
            }
        }
        public void Refresh()
        {
            SetMinValue = false;
            SetMaxValue = false;
            ColorRefresh();
        }
        private void ProcessPrimitives()
        {
            WorkPlaneCanvas.Children.Clear();
            double sizeX = PrimitiveOperations.GetSizeX(PrimitiveSet.ValuePrimitives);
            double sizeY = PrimitiveOperations.GetSizeY(PrimitiveSet.ValuePrimitives);
            dX = PrimitiveOperations.GetMinMaxX(PrimitiveSet.ValuePrimitives)[0];
            dY = PrimitiveOperations.GetMinMaxY(PrimitiveSet.ValuePrimitives)[1];
            WorkPlaneCanvas.Width = Math.Abs(sizeX);
            WorkPlaneCanvas.Height = Math.Abs(sizeY);
            WorkPlaneBox.Width = ScrolWidth - 50;
            WorkPlaneBox.Height = ScrolHeight - 50;
            foreach (var primitive in PrimitiveSet.ValuePrimitives)
            {
                if (primitive is IRectanglePrimitive)
                {
                    IRectanglePrimitive rectanglePrimitive = primitive as IRectanglePrimitive;
                    Rectangle rectangle = ProcessRectanglePrimitive(rectanglePrimitive);
                    WorkPlaneCanvas.Children.Add(rectangle);
                }
                else if (primitive is ICirclePrimitive)
                {
                    ICirclePrimitive circlePrimitive = primitive as ICirclePrimitive;
                    Ellipse ellipse = ProcessCirclePrimitive(circlePrimitive);
                    WorkPlaneCanvas.Children.Add(ellipse);
                }
                else { throw new FieldVisulizerException(ErrorStrings.PrimitiveTypeIsUnknown); }
            }
        }
        private Rectangle ProcessRectanglePrimitive(IRectanglePrimitive rectanglePrimitive)
        {
            Rectangle rectangle = new Rectangle
            {
                Height = rectanglePrimitive.Height,
                Width = rectanglePrimitive.Width
            };
            double addX = rectanglePrimitive.Width / 2;
            double addY = rectanglePrimitive.Height / 2;
            ProcessShape(rectangle, rectanglePrimitive, addX, addY);
            return rectangle;
        }
        private Ellipse ProcessCirclePrimitive(ICirclePrimitive circlePrimitive)
        {
            Ellipse ellipse = new Ellipse
            {
                Height = circlePrimitive.Diameter,
                Width = circlePrimitive.Diameter
            };
            double addX = circlePrimitive.Diameter / 2;
            double addY = circlePrimitive.Diameter / 2;

            ProcessShape(ellipse, circlePrimitive, addX, addY);
            return ellipse;
        }
        private void ProcessShape(Shape shape, IValuePrimitive valuePrimitive, double addX, double addY)
        {
            SolidColorBrush brush = new SolidColorBrush();
            brush.Color = ColorOperations.GetColorByValue(valueRange, _ColorMap, valuePrimitive.Value);
            foreach (var valueRange in valueColorRanges)
            {
                if (valuePrimitive.Value >= valueRange.ExactValues.BottomValue & valuePrimitive.Value <= valueRange.ExactValues.TopValue & (!valueRange.IsActive))
                {
                    brush.Color = Colors.Gray;
                }
            }
            shape.ToolTip = roundLogic.RoundValue(valuePrimitive.Value);
            shape.Tag = valuePrimitive;
            shape.Fill = brush;
            Canvas.SetLeft(shape, valuePrimitive.CenterX - addX - dX);
            Canvas.SetTop(shape, -valuePrimitive.CenterY - addY + dY);
        }
        private void Zoom(double coefficient)
        {
            WorkPlaneBox.Width *= coefficient;
            WorkPlaneBox.Height *= coefficient;
        }
        private void ChangeColorMap()
        {
            //Iterate all available color maps one by one
            try
            {
                _ColorMapType++;
                IColorMap colorMap = ColorMapFactory.GetColorMap(_ColorMapType);
            }
            catch (Exception ex) { _ColorMapType = 0; }
            ColorRefresh();
        }
        private bool PrimitiveValidation()
        {
            if (PrimitiveSet == null || PrimitiveSet.ValuePrimitives.Count() == 0) { return false; }
            else return true;
        }
        private void SetColor()
        {
            valueRange = PrimitiveOperations.GetValueRange(PrimitiveSet.ValuePrimitives);
            //if bottom value is greater than top value
            if (SetMinValue
                & SetMaxValue
                & (UserValueRange.BottomValue > UserValueRange.TopValue))
            {
                UserValueRange.TopValue = UserValueRange.BottomValue;
            }
            if (SetMinValue == true)
            {
                valueRange.BottomValue = UserValueRange.BottomValue;
            }
            else
            {
                UserValueRange.BottomValue = valueRange.BottomValue;
            }
            if (SetMaxValue == true)
            {
                valueRange.TopValue = UserValueRange.TopValue;
            }
            else
            {
                UserValueRange.TopValue = valueRange.TopValue;
            }
            valueRanges = ValueRangeOperations.DivideValueRange(valueRange, RangeNumber);
            valueColorRanges = ColorOperations.GetValueColorRanges(valueRange, valueRanges, _ColorMap);
        }
        private void SetCrossLine(object commandParameter)
        {
            AddCrossLine();
            AddSummaryInfoCrossLine();
        }
        private void AddCrossLine()
        {
            double width = WorkPlaneCanvas.ActualWidth;
            double heigth = WorkPlaneCanvas.ActualHeight;
            Line line = new Line();
            if (crossLineX == 0d)
            {
                line.X1 = - width / 2d - dX;
                line.Y1 = - crossLineY + dY;
                line.X2 = width / 2d - dX;
                line.Y2 = - crossLineY + dY;
            }
            else if (crossLineY == 0d)
            {
                line.X1 = crossLineX - dX;
                line.Y1 = heigth / 2 + dY;
                line.X2 = crossLineX - dX;
                line.Y2 = -heigth / 2 + dY;
            }
            else
            {
                line.X1 = - width / 2d - dX;
                line.Y1 = -(crossLineY / crossLineX * width / 2 + crossLineY) - dY;
                line.X2 = width / 2d - dX;
                line.Y2 = -(-crossLineY / crossLineX * width / 2 + crossLineY) - dY ;
            }
            SolidColorBrush brush = new SolidColorBrush();
            brush.Color = Colors.Red;
            line.Fill = brush;
            line.Stroke = brush;
            line.StrokeThickness = (width + heigth) / 100;
            if (previosLine != null)
            {
                try { WorkPlaneCanvas.Children.Remove(previosLine);}
                catch (Exception) {}
            }
            previosLine = line;
            WorkPlaneCanvas.Children.Add(line);
        }
        private double GetPointOfCrossLine(double x)
        {
            double y;
            if (crossLineX == 0d)
            {
                y = crossLineY;
            }
            else
            {
                y = -crossLineY / crossLineX - crossLineY; 
            }
            return y;
        }
        private void AddSummaryInfoCrossLine()
        {
            SumAboveLine = primitiveSet is null ? 0 : primitiveSet.ValuePrimitives.Where(x=>x.CenterY >= GetPointOfCrossLine(x.CenterX)).Sum(x => x.Value);
            SumUnderLine = primitiveSet is null ? 0 : primitiveSet.ValuePrimitives.Where(x => x.CenterY <= GetPointOfCrossLine(x.CenterX)).Sum(x => x.Value);
        }
    }
}
