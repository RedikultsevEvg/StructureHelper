using StructureHelper.Models.Materials;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;
using StructureHelperLogics.Models.Materials;
using System;

namespace StructureHelperLogics.NdmCalculations.Primitives
{
    /// <summary>
    /// Item clone strategy for NDM primitives, including their material and host references.
    /// </summary>
    public class NdmPrimitiveCloneStrategy : ICloneStrategy<INdmPrimitive>
    {
        private readonly ICloningStrategy? cloningStrategy;
        private readonly ICloneStrategy<IHeadMaterial> materialCloneStrategy;

        public NdmPrimitiveCloneStrategy()
        {
            materialCloneStrategy = new HeadMaterialCloneStrategy();
        }

        public NdmPrimitiveCloneStrategy(ICloningStrategy cloningStrategy)
        {
            this.cloningStrategy = cloningStrategy;
            materialCloneStrategy = new HeadMaterialCloneStrategy();
        }

        public INdmPrimitive GetClone(INdmPrimitive sourceObject, CloningMode cloningMode)
        {
            CheckObject.ThrowIfNull(sourceObject);
            if (cloningMode == CloningMode.Reference)
            {
                return sourceObject;
            }

            var clone = (INdmPrimitive)LegacyCloneHelper.Clone(sourceObject, cloningMode);

            if (sourceObject.NdmElement?.HeadMaterial is not null)
            {
                clone.NdmElement.HeadMaterial = cloningStrategy is null
                    ? materialCloneStrategy.GetClone(sourceObject.NdmElement.HeadMaterial, cloningMode)
                    : (IHeadMaterial)cloningStrategy.Clone(
                        sourceObject.NdmElement.HeadMaterial,
                        materialCloneStrategy,
                        cloningMode);
            }

            if (sourceObject is IHasHostPrimitive sourceHost
                && sourceHost.HostPrimitive is not null
                && clone is IHasHostPrimitive cloneHost)
            {
                cloneHost.HostPrimitive = cloningStrategy is null
                    ? GetClone(sourceHost.HostPrimitive, cloningMode)
                    : (INdmPrimitive)cloningStrategy.Clone(sourceHost.HostPrimitive, this, cloningMode);
            }

            CloneIdSynchronizer.Synchronize(sourceObject, clone, cloningMode);
            return clone;
        }
    }
}
