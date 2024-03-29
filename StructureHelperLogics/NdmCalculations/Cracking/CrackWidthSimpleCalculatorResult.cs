﻿using StructureHelperCommon.Models.Calculators;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    public class CrackWidthSimpleCalculatorResult : IResult
    {
        public bool IsValid { get; set; }
        public string Description { get; set; }
        public RebarPrimitive RebarPrimitive { get; set; }
        public double CrackWidth { get; set; }
        public double RebarStrain { get; set; }
        public double ConcreteStrain { get; set; }
    }
}
