﻿using System;
using System.Collections.Generic;
using StructureHelperCommon.Infrastructures.Enums;

namespace StructureHelperCommon.Models.Materials.Libraries
{
    public interface IMaterialSafetyFactor : ICloneable
    {
        string Name { get; set; }
        bool Take { get; set; }
        string Description { get; set; }
        List<IMaterialPartialFactor> PartialFactors { get; }
        double GetFactor(StressStates stressState, CalcTerms calcTerm, LimitStates limitStates);
    }
}
