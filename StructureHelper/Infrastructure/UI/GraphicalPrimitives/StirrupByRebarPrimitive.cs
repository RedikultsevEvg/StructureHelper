using StructureHelper.Windows.UserControls;
using StructureHelperCommon.Models.VisualProperties;
using StructureHelperLogics.Models.BeamShears;
using System;

namespace StructureHelper.Infrastructure.UI.GraphicalPrimitives
{
    public class StirrupByRebarPrimitive : IGraphicalPrimitive
    {
        private readonly IInclinedSection inclinedSection;
        private readonly IStirrupByDensity stirrupByDensity;

        public IStirrupByRebar StirrupByRebar { get; }
        public double CenterX => 0;
        public double CenterY => 0;
        public double StartPoinX => StirrupByRebar.StartCoordinate;
        public double BottomPointY => InclinedSection.FullDepth - InclinedSection.EffectiveDepth;
        public double TopPointY => InclinedSection.FullDepth;
        public double Length => StirrupByRebar.EndCoordinate - StirrupByRebar.StartCoordinate;
        public double Depth => InclinedSection.EffectiveDepth;
        public double Density => Math.Round(stirrupByDensity.StirrupDensity);


        public IInclinedSection InclinedSection => inclinedSection;

        public PrimitiveVisualPropertyViewModel VisualProperty { get; }

        public StirrupByRebarPrimitive(IStirrupByRebar stirrupByRebar, IInclinedSection inclinedSection)
        {
            this.StirrupByRebar = stirrupByRebar;
            this.inclinedSection = inclinedSection;
            var logic = new StirrupByRebarToDensityConvertStrategy(null, inclinedSection);
            stirrupByDensity = logic.Convert(stirrupByRebar);
            VisualProperty = new(stirrupByRebar.VisualProperty);
        }
    }
}
