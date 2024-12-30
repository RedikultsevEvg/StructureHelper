using System.Collections.Generic;


//Copyright (c) 2025 Redikultsev Evgeny, Ekaterinburg, Russia
//All rights reserved.

namespace StructureHelperLogics.NdmCalculations.Primitives
{
    /// <summary>
    /// Interface for entities which has collection of ndm-primitives
    /// </summary>
    public interface IHasPrimitives
    {
        /// <summary>
        /// Collection of NdmPrimitives
        /// </summary>
        List<INdmPrimitive> Primitives { get; }
    }
}
