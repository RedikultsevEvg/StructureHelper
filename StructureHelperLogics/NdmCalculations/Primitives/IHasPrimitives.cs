using System.Collections.Generic;

namespace StructureHelperLogics.NdmCalculations.Primitives
{
    public interface IHasPrimitives
    {
        List<INdmPrimitive> Primitives { get; }
    }
}
