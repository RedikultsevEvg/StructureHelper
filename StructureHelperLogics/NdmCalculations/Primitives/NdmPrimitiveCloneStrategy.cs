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

        public INdmPrimitive GetClone(INdmPrimitive sourceObject, CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            CheckObject.ThrowIfNull(sourceObject);
            if (cloneMode == CloneMode.Reference) { return sourceObject;}

            var clone = sourceObject.GetClone(sourceObject, cloneMode);

            if (sourceObject.NdmElement?.HeadMaterial is not null)
            {
                clone.NdmElement.HeadMaterial = cloningStrategy is null
                    ? materialCloneStrategy.GetClone(sourceObject.NdmElement.HeadMaterial, cloneMode)
                    : cloningStrategy.Clone(
                        sourceObject.NdmElement.HeadMaterial,
                        materialCloneStrategy,
                        cloneMode);
            }

            if (sourceObject is IHasHostPrimitive sourceHost
                && sourceHost.HostPrimitive is not null
                && clone is IHasHostPrimitive cloneHost)
            {
                cloneHost.HostPrimitive = cloningStrategy is null
                    ? GetClone(sourceHost.HostPrimitive, cloneMode)
                    : cloningStrategy.Clone(sourceHost.HostPrimitive, this, cloneMode);
            }
            return clone;
        }
    }
}
