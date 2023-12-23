using StructureHelperCommon.Models.Calculators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces.LimitCurve
{
    public class LimitCurvesResult : IResult, IiterationResult
    {
        public bool IsValid { get; set; }
        public string? Description { get; set; }
        public List<LimitCurveResult> LimitCurveResults {get;set;}
        public int MaxIterationCount { get; set; }
        public int IterationNumber { get; set; }

        public LimitCurvesResult()
        {
            LimitCurveResults = new();
        }
    }
}
