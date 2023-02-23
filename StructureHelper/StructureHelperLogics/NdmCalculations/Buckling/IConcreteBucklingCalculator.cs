using StructureHelperCommon.Models.Calculators;
using StructureHelperLogics.NdmCalculations.Analyses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Buckling
{
    internal interface IConcreteBucklingCalculator : INdmCalculator
    {
        IAccuracy Accuracy { get; set; }
    }
}
