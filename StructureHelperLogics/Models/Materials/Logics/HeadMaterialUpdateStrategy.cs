using StructureHelper.Models.Materials;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Materials;
using StructureHelperCommon.Services;

namespace StructureHelperLogics.Models.Materials
{
    public class HeadMaterialUpdateStrategy : IParentUpdateStrategy<IHeadMaterial>
    {
        private IUpdateStrategy<IHeadMaterial> baseUpdateStrategy;
        private IUpdateStrategy<IHelperMaterial> helperMaterialUpdateStrategy;

        public bool UpdateChildren { get; set; } = true;

        public HeadMaterialUpdateStrategy(
            IUpdateStrategy<IHeadMaterial> baseUpdateStrategy,
            IUpdateStrategy<IHelperMaterial> helperMaterialUpdateStrategy)
        {
            this.baseUpdateStrategy = baseUpdateStrategy;
            this.helperMaterialUpdateStrategy = helperMaterialUpdateStrategy;
        }
        public HeadMaterialUpdateStrategy() : this(
            new HeadMaterialBaseUpdateStrategy(),
            new HelperMaterialUpdateStrategy())
        {

        }

        public void Update(IHeadMaterial targetObject, IHeadMaterial sourceObject)
        {
            CheckObject.ThrowIfNull(sourceObject);
            CheckObject.ThrowIfNull(targetObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            baseUpdateStrategy.Update(targetObject, sourceObject);
            if (UpdateChildren == true)
            {
                targetObject.HelperMaterial = sourceObject.HelperMaterial.Clone() as IHelperMaterial;
                helperMaterialUpdateStrategy.Update(targetObject.HelperMaterial, sourceObject.HelperMaterial);
            }
        }


    }
}
