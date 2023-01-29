using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Sections;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Buckling
{
    public interface IBucklingOptions
    {
        ICompressedMember CompressedMember { get; }
        LimitStates LimitState { get; }
        CalcTerms CalcTerm { get; }
        IEnumerable<INdmPrimitive> Primitives { get; }
        IForceTuple CalcForceTuple { get; }
    }
}
