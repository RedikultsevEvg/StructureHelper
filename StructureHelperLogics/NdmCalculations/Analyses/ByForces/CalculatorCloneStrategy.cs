using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Services;
using System;

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces
{
    /// <summary>
    /// Item clone strategy for calculators used by collection cloning.
    /// </summary>
    public class CalculatorCloneStrategy : ICloneStrategy<ICalculator>
    {
        public ICalculator GetClone(ICalculator sourceObject, CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            CheckObject.ThrowIfNull(sourceObject);
            if (cloneMode == CloneMode.Reference)
            {
                return sourceObject;
            }
            var clone = (ICalculator)sourceObject.GetClone(cloneMode, cloneDepth);
            return clone;
        }
    }
}
