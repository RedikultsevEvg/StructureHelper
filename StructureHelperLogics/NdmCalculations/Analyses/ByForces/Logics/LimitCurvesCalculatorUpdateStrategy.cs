using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces.LimitCurve;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces.Logics
{
    internal class LimitCurvesCalculatorUpdateStrategy : IUpdateStrategy<LimitCurvesCalculator>
    {
        LimitCurveInputDataUpdateStrategy inputDataUpdateStrategy => new();
        public void Update(LimitCurvesCalculator targetObject, LimitCurvesCalculator sourceObject)
        {
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            targetObject.Name = sourceObject.Name;
            targetObject.ActionToOutputResults = sourceObject.ActionToOutputResults;
            inputDataUpdateStrategy.Update(targetObject.InputData, sourceObject.InputData);
        }
    }
}
