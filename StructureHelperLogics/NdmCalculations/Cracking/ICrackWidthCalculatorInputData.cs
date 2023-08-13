using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    public interface ICrackWidthCalculatorInputData
    {
        LimitStates LimitState { get; set; }
        CalcTerms CalcTerm { get; set; }
        StrainTuple StrainTuple { get; set; }
        IEnumerable<INdmPrimitive> NdmPrimitives {get;set;}
    }
}
