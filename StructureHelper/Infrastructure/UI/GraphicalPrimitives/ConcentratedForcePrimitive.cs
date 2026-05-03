using StructureHelper.Windows.Shapes;
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
    public class ConcentratedForcePrimitive : IGraphicalPrimitive
    {
        private IConcentratedForce concentratedForce;
        private IInclinedSection inclinedSection;

        public double ScaleFactor
        {
            get
            {
                double forceValue = concentratedForce.ForceValue.Qy;
                return forceValue / MaxForce;
            }
        }

        public double TranslateX => concentratedForce.ForceCoordinate;
        public double TranslateY => BeamShearService.GetAbsoluteLevel(concentratedForce, inclinedSection);


        public PrimitiveVisualPropertyViewModel VisualProperty { get; } = new(new PrimitiveVisualProperty(Guid.Empty));
        public IConcentratedForce ConcentratedForce => concentratedForce;

        public string Name => concentratedForce.Name;

        public double MaxForce { get; set; } = 1e6;

        public ConcentratedForcePrimitive(IConcentratedForce concentratedForce, IInclinedSection inclinedSection)
        {
            this.concentratedForce = concentratedForce;
            this.inclinedSection = inclinedSection;
            VisualProperty.Color = (Color)ColorConverter.ConvertFromString("Black");
        }


    }
}
