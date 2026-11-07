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
        public ICalculator GetClone(ICalculator sourceObject, CloningMode cloningMode)
        {
            CheckObject.ThrowIfNull(sourceObject);
            if (cloningMode == CloningMode.Reference)
            {
                return sourceObject;
            }

            var clone = (ICalculator)LegacyCloneHelper.Clone(sourceObject, cloningMode);
            CloneIdSynchronizer.Synchronize(sourceObject, clone, cloningMode);
            return clone;
        }
    }
}
