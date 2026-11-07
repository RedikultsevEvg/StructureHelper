using StructureHelper.Models.Materials;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;

namespace StructureHelperLogics.Models.Materials
{
    /// <summary>
    /// Collection clone strategy for objects which have head materials.
    /// </summary>
    public class HasHeadMaterialsCloneStrategy : CollectionDeepCloningStrategy<IHeadMaterial>
    {
        public HasHeadMaterialsCloneStrategy(
            ICloningStrategy cloningStrategy,
            ICloneStrategy<IHeadMaterial> materialCloneStrategy)
            : base(cloningStrategy, materialCloneStrategy)
        {
        }

        public HasHeadMaterialsCloneStrategy(ICloningStrategy cloningStrategy)
            : this(cloningStrategy, new HeadMaterialCloneStrategy())
        {
        }

        public void GetClone(
            IHasHeadMaterials target,
            IHasHeadMaterials source,
            CloningMode mode = CloningMode.NewEntity)
        {
            CheckObject.ThrowIfNull(target);
            CheckObject.ThrowIfNull(source);
            GetClone(target.HeadMaterials, source.HeadMaterials, mode);
        }
    }
}
