﻿using StructureHelperCommon.Models.Calculators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces
{
    public interface IForceTupleCalculator : ICalculator, IHasActionByResult
    {
        IForceTupleInputData InputData {get;set;}
    }
}
