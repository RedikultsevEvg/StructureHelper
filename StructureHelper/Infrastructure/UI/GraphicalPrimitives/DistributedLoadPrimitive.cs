using StructureHelper.Windows.UserControls;
using StructureHelperCommon.Models.Forces;
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
        public double TranslateY => BeamShearService.GetAbsoluteLevel(distributedLoad, inclinedSection);
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
    }
}
