using StructureHelper.Windows.UserControls;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Shapes;
using StructureHelperLogics.Models.BeamShears;
using System;
using System.Windows.Media;
using PrimitiveVisualProperty = StructureHelperCommon.Models.VisualProperties.PrimitiveVisualProperty;

namespace StructureHelper.Infrastructure.UI.GraphicalPrimitives
{
    public class DistributedLoadPrimitive : IGraphicalPrimitive
    {
        private IDistributedLoad distributedLoad;
        private IInclinedSection inclinedSection;

        public double ScaleFactor
        {
            get
            {
                double forceValue = distributedLoad.LoadValue.Qy;
                return -1 * forceValue / MaxForce;
            }
        }

        public double TranslateX => distributedLoad.StartCoordinate;
        public double TranslateY => GetAbsoluteLevel();
        public double Length => distributedLoad.EndCoordinate - distributedLoad.StartCoordinate;


        public PrimitiveVisualPropertyViewModel VisualProperty { get; } = new(new PrimitiveVisualProperty(Guid.Empty));
        public IDistributedLoad DistributedLoad => distributedLoad;

        public string Name => distributedLoad.Name;

        public double MaxForce { get; set; } = 1e6;

        public DistributedLoadPrimitive(IDistributedLoad distributedLoad, IInclinedSection inclinedSection)
        {
            this.distributedLoad = distributedLoad;
            this.inclinedSection = inclinedSection;
            VisualProperty.Color = (Color)ColorConverter.ConvertFromString("LightBlue");
            VisualProperty.FactoredOpacity = 90;
        }

        private double GetAbsoluteLevel()
        {
            double height;
            IShape shape = inclinedSection.BeamShearSection.Shape;
            if (shape is IRectangleShape rectangle) { height = rectangle.Height; }
            else if (shape is ICircleShape circle) { height = circle.Diameter; }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(shape) + $": distributed load {distributedLoad.Name} shape");
            }
            double level = (distributedLoad.RelativeLoadLevel + 0.5) * height;
            return level;
        }
    }
}
