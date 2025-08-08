using StructureHelperCommon.Models.VisualProperties;
using StructureHelperLogics.Models.BeamShears;

namespace StructureHelper.Infrastructure.UI.GraphicalPrimitives
{
    internal class StirrupByDensityPrimitive : IGraphicalPrimitive
    {
        public IStirrupByDensity StirrupByDensity { get; }

        public double CenterX => 0;
        public double CenterY => 0;
        public double StartPoinX => StirrupByDensity.StartCoordinate;
        public double BottomPointY => InclinedSection.FullDepth - InclinedSection.EffectiveDepth;
        public double TopPointY => InclinedSection.FullDepth;
        public double Length => StirrupByDensity.EndCoordinate - StirrupByDensity.StartCoordinate;
        public double Depth => InclinedSection.EffectiveDepth;
        public IPrimitiveVisualProperty VisualProperty => StirrupByDensity.VisualProperty;

        public IInclinedSection InclinedSection { get; set; }

        public StirrupByDensityPrimitive(IStirrupByDensity stirrupByDensity, IInclinedSection inclinedSection)
        {
            StirrupByDensity = stirrupByDensity;
            this.InclinedSection = inclinedSection;
        }
    }
}
