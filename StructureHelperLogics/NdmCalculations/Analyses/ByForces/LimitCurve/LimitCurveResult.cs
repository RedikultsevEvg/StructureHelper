using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces
{
    public class LimitCurveResult : IResult
    {
        public bool IsValid { get; set; }
        public string Description { get; set; }
        public List<IPoint2D> Points { get; set; }
        public LimitCurveResult()
        {
            Points = new List<IPoint2D>();
        }
    }
}
