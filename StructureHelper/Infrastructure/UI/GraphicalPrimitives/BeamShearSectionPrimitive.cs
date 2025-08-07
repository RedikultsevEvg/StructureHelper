using StructureHelperCommon.Models.VisualProperties;
using StructureHelperLogics.Models.BeamShears;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelper.Infrastructure.UI.GraphicalPrimitives
{
    public class BeamShearSectionPrimitive : IGraphicalPrimitive
    {
        private IBeamShearSection beamShearSection;
        private IInclinedSection inclinedSection;


        public double CenterX { get; set; } = 0;
        public double CenterY { get; set; } = 0;
        public double FullDepth => inclinedSection.FullDepth;
        public double EffectiveDepth => inclinedSection.EffectiveDepth;
        public double BottomCover => FullDepth - EffectiveDepth;
        public double PositiveLength => inclinedSection.EffectiveDepth * 3.5;
        public double NegativeLength { get; set; } = -0.1;
        public double SupportHeight { get; set; } = 0.1;
        public double SupportWidth { get; set; } = 0.2;
        public double SupportStartX => -SupportWidth / 2;
        public double SupportStartY => -SupportHeight;
        public string SupportPathData => $"M 0 0 L {SupportWidth / 2} {-SupportHeight} L {-SupportWidth / 2} {-SupportHeight} Z";

        public IPrimitiveVisualProperty VisualProperty => beamShearSection.VisualProperty;

        public BeamShearSectionPrimitive(IBeamShearSection beamShearSection, IInclinedSection inclinedSection)
        {
            this.beamShearSection = beamShearSection;
            this.inclinedSection = inclinedSection;
        }
    }
}
