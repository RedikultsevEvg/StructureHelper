using System.Collections.Generic;

namespace StructureHelperLogics.NdmCalculations.Primitives
{
    public interface IHasPrimitives
    {
        /// <summary>
        /// Collection of NdmPrimitives
        /// </summary>
        List<INdmPrimitive> Primitives { get; }
    }
}
