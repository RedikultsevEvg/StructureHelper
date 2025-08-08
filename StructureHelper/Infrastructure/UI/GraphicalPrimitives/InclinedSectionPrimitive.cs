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
        public double FactorOfUsing => Math.Round(source.FactorOfUsing, 4);
        public double EffectiveDepth => Math.Round(inclinedSection.EffectiveDepth, 3);
        public double SpanRatio => (inclinedSection.EndCoord - inclinedSection.StartCoord) / inclinedSection.EffectiveDepth;
        public double ActualShearForce => Math.Round(source.InputData.ForceTuple.Qy);
        public double UltimateShearForce => Math.Round(source.TotalStrength);

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
