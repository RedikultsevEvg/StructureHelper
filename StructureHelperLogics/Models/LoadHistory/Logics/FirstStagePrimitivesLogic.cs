using StructureHelperLogics.NdmCalculations.Primitives;
using System.Collections.Generic;

namespace StructureHelperLogics.Models.LoadHistory.Logics
{
    /// <inheritdoc/>
    public class FirstStagePrimitivesLogic : IFirstStagePrimitivesLogic
    {
        /// <inheritdoc/>
        public void Process(IConstructionStage stage, IStageResult stageResult)
        {
            Dictionary<INdmPrimitive, INdmPrimitive> primitives = stageResult.Primitives;
            foreach (INdmPrimitive primitive in stage.InputData.Primitives)
            {
                INdmPrimitive primitiveClone = (INdmPrimitive)primitive.Clone();
                primitiveClone.NdmElement.AutoPrestrain.Clear();
                primitiveClone.NdmElement.HistoryPrestrain.Clear();
                if (primitive is IHasHostPrimitive hasHostPrimitive)
                {
                    ProcessHost(primitives, hasHostPrimitive, primitiveClone);
                }
                StagePrimitivesLogicHelper.AddPrimitiveToDictionary(primitives, primitive, primitiveClone);
            }
        }

        /// <summary>
        /// Adds the clone of the host primitive to the dictionary if it is absent and links the clone
        /// of the hosted primitive to it.
        /// </summary>
        private static INdmPrimitive ProcessHost(
            Dictionary<INdmPrimitive, INdmPrimitive> primitives,
            IHasHostPrimitive primitive,
            INdmPrimitive primitiveClone)
        {
            INdmPrimitive hostPrimitive = primitive.HostPrimitive!;
            if (primitives.TryGetValue(hostPrimitive, out INdmPrimitive? existingHostClone))
            {
                ((IHasHostPrimitive)primitiveClone).HostPrimitive = existingHostClone;
                return existingHostClone!;
            }

            INdmPrimitive hostPrimitiveClone = (INdmPrimitive)hostPrimitive.Clone();
            hostPrimitiveClone.NdmElement.AutoPrestrain.Clear();
            primitives.Add(hostPrimitive, hostPrimitiveClone);
            ((IHasHostPrimitive)primitiveClone).HostPrimitive = hostPrimitiveClone;
            return hostPrimitiveClone;
        }
    }
}
