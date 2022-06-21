using StructureHelperLogics.Infrastructures.CommonEnums;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperLogics.NdmCalculations.Triangulations
{
    public interface ITriangulationOptions
    {
        LimitStates LimiteState { get; }
        CalcTerms CalcTerm { get; }
    }
}
