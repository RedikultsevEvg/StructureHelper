using StructureHelper.Infrastructure.UI.Converters.Units;
using StructureHelper.Windows.UserControls;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Shapes;
using StructureHelperLogics.Models.BeamShears;
using System;
using System.Windows.Media;
using PrimitiveVisualProperty = StructureHelperCommon.Models.VisualProperties.PrimitiveVisualProperty;

namespace StructureHelper.Infrastructure.UI.GraphicalPrimitives
{
    public class TrapezoidDistributedLoadPrimitive : IGraphicalPrimitive
    {
        private ITrapezoidDistributedLoad trapezoidLoad;
        private IInclinedSection inclinedSection;

        public TrapezoidDistributedLoadPrimitive(ITrapezoidDistributedLoad trapezoidLoad, IInclinedSection inclinedSection)
        {
            this.trapezoidLoad = trapezoidLoad;
            this.inclinedSection = inclinedSection;
            VisualProperty.Color = (Color)ColorConverter.ConvertFromString("Pink");
            VisualProperty.FactoredOpacity = 90;
            GetPath();
        }

        public string Name => trapezoidLoad.Name;

        public double ScaleFactor
        {
            get
            {
                double forceValue = GetMaxValue();
                return -1 * forceValue / MaxForce;
            }
        }

        private double GetMaxValue()
        {
            double startValue = trapezoidLoad.StartLoadValue.Qy;
            double endValue = trapezoidLoad.EndLoadValue.Qy;
            double forceValue = GetForceValue(startValue, endValue);
            return forceValue;
        }

        private static double GetForceValue(double startValue, double endValue)
        {
            return Math.Max(Math.Abs(startValue), Math.Abs(endValue));
        }

        public double MaxForce { get; set; } = 1e6;

        public double TranslateX => trapezoidLoad.StartCoordinate;
        public double TranslateY => BeamShearService.GetAbsoluteLevel(trapezoidLoad, inclinedSection);
        public double Length => trapezoidLoad.EndCoordinate - trapezoidLoad.StartCoordinate;


        public PrimitiveVisualPropertyViewModel VisualProperty { get; } = new(new PrimitiveVisualProperty(Guid.Empty));
        public PathGeometry PathGeometry { get; private set; }
        public ITrapezoidDistributedLoad TrapezoidLoad { get => trapezoidLoad; set => trapezoidLoad = value; }

        private void GetPath()
        {
            LinePolygonShape polygon = new() { IsClosed = true};
            double maxValue = GetMaxValue();
            polygon.AddVertex(new Vertex(0, 0));
            polygon.AddVertex(new Vertex(0, trapezoidLoad.StartLoadValue.Qy / maxValue * 0.2));
            polygon.AddVertex(new Vertex(Length, trapezoidLoad.EndLoadValue.Qy / maxValue * 0.2));
            polygon.AddVertex(new Vertex(Length, 0.0));
            var logic = new LinePolygonToPathGeometryConvertStrategy();
            try
            {
                PathGeometry = logic.Convert(polygon);
            }
            catch (Exception ex)
            {
                return;
            }
        }
    }
}
