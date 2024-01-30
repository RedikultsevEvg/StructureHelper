using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces.LimitCurve;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces.Logics
{
    internal class LimitCurveInputDataUpdateStrategy : IUpdateStrategy<LimitCurveInputData>
    {
        SurroundDataUpdateStrategy surroundDataUpdateStrategy => new();
        public void Update(LimitCurveInputData targetObject, LimitCurveInputData sourceObject)
        {
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            targetObject.LimitStates.Clear();
            targetObject.LimitStates.AddRange(sourceObject.LimitStates);
            targetObject.CalcTerms.Clear();
            targetObject.CalcTerms.AddRange(sourceObject.CalcTerms);
            targetObject.PredicateEntries.Clear();
            targetObject.PredicateEntries.AddRange(sourceObject.PredicateEntries);
            targetObject.PointCount = sourceObject.PointCount;
            surroundDataUpdateStrategy.Update(targetObject.SurroundData, sourceObject.SurroundData);
        }
    }
}
