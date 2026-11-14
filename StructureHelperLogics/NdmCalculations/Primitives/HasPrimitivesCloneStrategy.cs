using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Services;

namespace StructureHelperLogics.NdmCalculations.Primitives
{
    /// <summary>
    /// Collection clone strategy for objects which have NDM primitives.
    /// </summary>
    public class HasPrimitivesCloneStrategy : CollectionDeepCloningStrategy<INdmPrimitive>
    {
        public HasPrimitivesCloneStrategy(
            ICloningStrategy cloningStrategy,
            ICloneStrategy<INdmPrimitive> primitiveCloneStrategy)
            : base(cloningStrategy, primitiveCloneStrategy)
        {
        }

        public HasPrimitivesCloneStrategy(ICloningStrategy cloningStrategy)
            : this(cloningStrategy, new NdmPrimitiveCloneStrategy(cloningStrategy))
        {
        }

        public void GetClone(
            IHasPrimitives target,
            IHasPrimitives source,
            CloneMode mode = CloneMode.NewEntity)
        {
            CheckObject.ThrowIfNull(target);
            CheckObject.ThrowIfNull(source);
            GetClone(target.Primitives, source.Primitives, mode);
        }
    }
}
