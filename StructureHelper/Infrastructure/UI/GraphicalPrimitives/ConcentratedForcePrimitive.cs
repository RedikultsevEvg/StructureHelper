using StructureHelper.Windows.UserControls;
using StructureHelperCommon.Models.Forces;
using System;
using System.Windows.Media;
using PrimitiveVisualProperty = StructureHelperCommon.Models.VisualProperties.PrimitiveVisualProperty;

namespace StructureHelper.Infrastructure.UI.GraphicalPrimitives
{
    public class ConcentratedForcePrimitive : IGraphicalPrimitive
    {
        private IConcentratedForce concentratedForce;
        private readonly double scaleFactor;

        public double ScaleFactor
        {
            get
            {
                if (concentratedForce.ForceValue.Qy > 0) { return -1; }
                return 1;
            }
        }

        public PrimitiveVisualPropertyViewModel VisualProperty { get; } = new(new PrimitiveVisualProperty(Guid.Empty));
        public IConcentratedForce ConcentratedForce => concentratedForce;

        public ConcentratedForcePrimitive(IConcentratedForce concentratedForce)
        {
            this.concentratedForce = concentratedForce;
            VisualProperty.Color = (Color)ColorConverter.ConvertFromString("Black");
        }
    }
}
