using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Services;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces.LimitCurve;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces.Logics;
using StructureHelperLogics.NdmCalculations.Cracking;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Analyses.Logics
{
    public class CalculatorUpdateStrategy : IUpdateStrategy<ICalculator>
    {
        public void Update(ICalculator targetObject, ICalculator sourceObject)
        {
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            CheckObject.CompareTypes(targetObject, sourceObject);
            if (targetObject is IForceCalculator target)
            {
                new ForceCalculatorUpdateStrategy().Update(target, (IForceCalculator)sourceObject);
            }
            else if (targetObject is LimitCurvesCalculator limitCurves)
            {
                new LimitCurvesCalculatorUpdateStrategy().Update(limitCurves, (LimitCurvesCalculator)sourceObject);
            }
            else if (targetObject is CrackCalculator crackCalculator)
            {
                new CrackCalculatorUpdateStrategy().Update(crackCalculator, (CrackCalculator)sourceObject);
            }
            else
            {
                ErrorCommonProcessor.ObjectTypeIsUnknown(typeof(INdmPrimitive), sourceObject.GetType());
            }
        }
    }
}
