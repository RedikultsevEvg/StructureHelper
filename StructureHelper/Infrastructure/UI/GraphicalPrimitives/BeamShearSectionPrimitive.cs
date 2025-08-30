using StructureHelper.Windows.UserControls;
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

        public double FullDepth => inclinedSection.FullDepth;
        public double WebWidth => inclinedSection.WebWidth;
        public double ReinforcementArea => inclinedSection.BeamShearSection.ReinforcementArea;
        public double EffectiveDepth => Math.Round(inclinedSection.EffectiveDepth, 3);
        public double BottomCover => Math.Round(FullDepth - EffectiveDepth, 3);
        public double PositiveLength => 100;
        public double NegativeLength { get; set; } = -0.1;
        public double SupportHeight { get; set; } = 0.1;
        public double SupportWidth { get; set; } = 0.2;
        public double SupportStartX => -SupportWidth / 2;
        public double SupportStartY => -SupportHeight;
        public string SupportPathData => $"M 0 0 L {SupportWidth / 2} {-SupportHeight} L {-SupportWidth / 2} {-SupportHeight} Z";


        public IBeamShearSection BeamShearSection => beamShearSection;

        public PrimitiveVisualPropertyViewModel VisualProperty {get;}

        public string Name => beamShearSection.Name;

        public BeamShearSectionPrimitive(IBeamShearSection beamShearSection, IInclinedSection inclinedSection)
        {
            this.beamShearSection = beamShearSection;
            this.inclinedSection = inclinedSection;
            VisualProperty = new(beamShearSection.VisualProperty);
        }
    }
}
