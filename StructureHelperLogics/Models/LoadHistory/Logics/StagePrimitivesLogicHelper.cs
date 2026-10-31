using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.NdmCalculations.Primitives;
using System.Collections.Generic;

namespace StructureHelperLogics.Models.LoadHistory.Logics
{
    /// <summary>
    /// Shared operations on the primitive dictionaries of load history stages.
    /// </summary>
    internal static class StagePrimitivesLogicHelper
    {
        /// <summary>
        /// Adds the clone only if the source primitive is not present in the dictionary yet.
        /// </summary>
        public static void AddPrimitiveToDictionary(
            Dictionary<INdmPrimitive, INdmPrimitive> primitives,
            INdmPrimitive primitive,
            INdmPrimitive primitiveClone)
        {
            if (primitives.ContainsKey(primitive) == false)
            {
                primitives.Add(primitive, primitiveClone);
            }
        }

        /// <summary>
        /// Adds the prestrain of the force tuple to the users prestrain of the primitive.
        /// </summary>
        public static void AddPrestrain(INdmPrimitive primitive, IForceTuple strainTuple, double factor = 1d)
        {
            primitive.NdmElement.HistoryPrestrain.Nz += strainTuple.Nz * factor;
            primitive.NdmElement.HistoryPrestrain.Mx += strainTuple.Mx * factor;
            primitive.NdmElement.HistoryPrestrain.My += strainTuple.My * factor;
        }
    }
}
