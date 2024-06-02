using System;
using System.Collections.Generic;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;

namespace StructureHelperCommon.Models.Materials.Libraries
{
    public interface IMaterialSafetyFactor : ISaveable, ICloneable
    {
        string Name { get; set; }
        bool Take { get; set; }
        string Description { get; set; }
        List<IMaterialPartialFactor> PartialFactors { get; }
        double GetFactor(StressStates stressState, CalcTerms calcTerm, LimitStates limitStates);
    }
}
