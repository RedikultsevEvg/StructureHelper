using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Buckling
{
    public interface IConcreteBucklingOptions : IBucklingOptions
    {
        IForceTuple LongTermTuple { get; }
    }
}
