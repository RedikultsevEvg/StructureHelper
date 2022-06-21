using StructureHelperLogics.Infrastructures.CommonEnums;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperLogics.NdmCalculations.Triangulations
{
    public class TriangulationOptions : ITriangulationOptions
    {
        public LimitStates LimiteState { get; set; }
        public CalcTerms CalcTerm { get; set; }
    }
}
