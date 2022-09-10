﻿using FieldVisualizer.Entities.ColorMaps;
using FieldVisualizer.Entities.ColorMaps.Factories;
using FieldVisualizer.Entities.Values;
using FieldVisualizer.Entities.Values.Primitives;
using FieldVisualizer.Infrastructure.Commands;
using FieldVisualizer.InfraStructures.Enums;
using FieldVisualizer.InfraStructures.Exceptions;
using FieldVisualizer.InfraStructures.Strings;
using FieldVisualizer.Services.ColorServices;
using FieldVisualizer.Services.PrimitiveServices;
using FieldVisualizer.Services.ValueRanges;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace FieldVisualizer.Windows.UserControls
{
    /// <summary>
    /// Логика взаимодействия для FieldViewer.xaml
    /// </summary>
    public partial class FieldViewer : UserControl
    {
        public ICommand RebuildCommand { get; }
        public ICommand ZoomInCommand { get; }
        public ICommand ZoomOutCommand { get; }
        public ICommand ChangeColorMapCommand { get; }

        public IPrimitiveSet PrimitiveSet { get; set; }
        private double dX, dY;
        private ColorMapsTypes _ColorMapType;
        private IColorMap _ColorMap;
        private IValueRange _ValueRange;
        private IEnumerable<IValueRange> _ValueRanges;
        private IEnumerable<IValueColorRange> _ValueColorRanges;
        const int RangeNumber = 16;

        public FieldViewer()
        {
            InitializeComponent();
            _ColorMapType = ColorMapsTypes.FullSpectrum;
            DataContext = this;
            RebuildCommand = new RelayCommand(o => ProcessPrimitives(), o => PrimitiveValidation());
            ZoomInCommand = new RelayCommand(o => Zoom(1.2), o => PrimitiveValidation());
            ZoomOutCommand = new RelayCommand(o => Zoom(0.8), o => PrimitiveValidation());
            ChangeColorMapCommand = new RelayCommand(o => ChangeColorMap(), o => PrimitiveValidation());
        }
        public void Refresh()
        {
            if (PrimitiveValidation() == false) { return; }
            _ValueRange = PrimitiveOperations.GetValueRange(PrimitiveSet.ValuePrimitives);
            _ValueRanges = ValueRangeOperations.DivideValueRange(_ValueRange, RangeNumber);
            _ColorMap = ColorMapFactory.GetColorMap(_ColorMapType);
            _ValueColorRanges = ColorOperations.GetValueColorRanges(_ValueRange, _ValueRanges, _ColorMap);
            if ((PrimitiveSet is null) == false)
            {
                ProcessPrimitives();
                LegendViewer.ValueColorRanges = _ValueColorRanges;
                LegendViewer.Refresh();
            }
        }
        private void ProcessPrimitives()
        {
            WorkPlaneCanvas.Children.Clear();
            double sizeX = PrimitiveOperations.GetSizeX(PrimitiveSet.ValuePrimitives);
            double sizeY = PrimitiveOperations.GetSizeY(PrimitiveSet.ValuePrimitives);
            dX = PrimitiveOperations.GetMinMaxX(PrimitiveSet.ValuePrimitives)[0];
            dY = PrimitiveOperations.GetMinMaxY(PrimitiveSet.ValuePrimitives)[0];
            WorkPlaneCanvas.Width = Math.Abs(sizeX);
            WorkPlaneCanvas.Height = Math.Abs(sizeY);
            WorkPlaneBox.Width = WorkPlaneViewer.ActualWidth - 50;
            WorkPlaneBox.Height = WorkPlaneViewer.ActualHeight - 50;
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
                else { throw new FieldVisulizerException(ErrorStrings.PrimitiveTypeIsUnknown);}
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
            brush.Color = ColorOperations.GetColorByValue(_ValueRange, _ColorMap, valuePrimitive.Value);
            foreach (var valueRange in _ValueColorRanges)
            {
                if (valuePrimitive.Value >= valueRange.BottomValue & valuePrimitive.Value <= valueRange.TopValue & (!valueRange.IsActive))
                {
                    brush.Color = Colors.Gray;
                }
            }
            shape.ToolTip = valuePrimitive.Value;
            shape.Tag = valuePrimitive;
            shape.Fill = brush;
            Canvas.SetLeft(shape, valuePrimitive.CenterX - addX - dX);
            Canvas.SetTop(shape, valuePrimitive.CenterY - addY - dY);
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
            Refresh();
        }
        private bool PrimitiveValidation()
        {
            if (PrimitiveSet == null || PrimitiveSet.ValuePrimitives.Count() == 0) { return false; }
            else return true;
        }
    }
}