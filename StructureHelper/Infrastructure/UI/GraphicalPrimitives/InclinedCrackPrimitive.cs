using StructureHelperLogics.Models.BeamShears;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelper.Infrastructure.UI.GraphicalPrimitives
{
    public class InclinedCrackPrimitive : InclinedSectionPrimitive
    {
        public InclinedCrackPrimitive(IBeamShearSectionLogicResult source) : base(source)
        {
        }
    }
}
