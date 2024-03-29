﻿using StructureHelperLogics.NdmCalculations.Analyses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.Templates.CrossSections
{
    internal interface ICalculatorLogic
    {
        IEnumerable<INdmCalculator> GetNdmCalculators();
    }
}
