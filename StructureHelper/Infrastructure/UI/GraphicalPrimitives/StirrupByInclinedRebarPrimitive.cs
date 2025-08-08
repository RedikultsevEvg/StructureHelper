using StructureHelperCommon.Models.VisualProperties;
using StructureHelperLogics.Models.BeamShears;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelper.Infrastructure.UI.GraphicalPrimitives
{
    public class StirrupByInclinedRebarPrimitive : IGraphicalPrimitive
    {
        private IInclinedSection inclinedSection;
        public IStirrupByInclinedRebar StirrupByInclinedRebar { get; }
        public double StartPointX => StirrupByInclinedRebar.StartCoordinate;
        public double StartPointY => inclinedSection.FullDepth - StirrupByInclinedRebar.CompressedGap;
        public double EndPointX => StirrupByInclinedRebar.StartCoordinate + GetRebarLengthX();
        public double EndPointY => inclinedSection.FullDepth - inclinedSection.EffectiveDepth;

        public double CenterX => 0;
        public double CenterY => 0;
        public IPrimitiveVisualProperty VisualProperty => StirrupByInclinedRebar.VisualProperty;
        public StirrupByInclinedRebarPrimitive(IStirrupByInclinedRebar stirrupByInclinedRebar, IInclinedSection inclinedSection)
        {
            StirrupByInclinedRebar = stirrupByInclinedRebar;
            this.inclinedSection = inclinedSection;
        }
        private double GetRebarLengthX()
        {
            double angleInRadian = Math.PI * StirrupByInclinedRebar.AngleOfInclination / 180;
            double rebarLengthY = inclinedSection.EffectiveDepth - StirrupByInclinedRebar.CompressedGap;
            double rebarLengthX = rebarLengthY / Math.Tan(angleInRadian);
            return rebarLengthX;
        }
    }
}
