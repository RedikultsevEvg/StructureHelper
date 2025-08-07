using StructureHelperCommon.Models.VisualProperties;
using StructureHelperLogics.Models.BeamShears;
using System;
using System.Windows.Media;

namespace StructureHelper.Infrastructure.UI.GraphicalPrimitives
{
    public class InclinedSectionPrimitive : IGraphicalPrimitive
    {
        private IBeamShearSectionLogicResult source;
        private IInclinedSection inclinedSection => source.InputData.InclinedSection;

        public double SectionStartX => inclinedSection.StartCoord;
        public double SectionEndX => inclinedSection.EndCoord;
        public double SectionStartY => inclinedSection.FullDepth - inclinedSection.EffectiveDepth;
        public double SectionEndY => inclinedSection.FullDepth;
        public double FactorOfUsing => source.FactorOfUsing;
        public double EffectiveDepth => inclinedSection.EffectiveDepth;
        public double SpanRatio => (inclinedSection.EndCoord - inclinedSection.StartCoord) / inclinedSection.EffectiveDepth;

        public double CenterX => 0;
        public double CenterY => 0;
        public IPrimitiveVisualProperty VisualProperty { get; private set; } = new PrimitiveVisualProperty(Guid.Empty);

        public InclinedSectionPrimitive(IBeamShearSectionLogicResult source)
        {
            this.source = source;
            if (source.FactorOfUsing >= 1)
            {
                VisualProperty.Color = (Color)ColorConverter.ConvertFromString("Red");
            }
            else
            {
                VisualProperty.Color = (Color)ColorConverter.ConvertFromString("Green");
            }
        }

    }
}
