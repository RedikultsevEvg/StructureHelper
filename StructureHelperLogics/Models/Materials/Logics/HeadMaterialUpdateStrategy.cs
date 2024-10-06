using StructureHelper.Models.Materials;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;

namespace StructureHelperLogics.Models.Materials
{
    public class HeadMaterialUpdateStrategy : IUpdateStrategy<IHeadMaterial>
    {
        private IUpdateStrategy<IHelperMaterial> helperMaterialUpdateStrategy;

        public HeadMaterialUpdateStrategy(IUpdateStrategy<IHelperMaterial> helperMaterialUpdateStrategy)
        {
            this.helperMaterialUpdateStrategy = helperMaterialUpdateStrategy;
        }
        public HeadMaterialUpdateStrategy() : this(new HelperMaterialUpdateStrategy())  {         }

        public void Update(IHeadMaterial targetObject, IHeadMaterial sourceObject)
        {
            CheckObject.IsNull(sourceObject);
            CheckObject.IsNull(targetObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            targetObject.Name = sourceObject.Name;
            targetObject.Color = sourceObject.Color;
            targetObject.HelperMaterial = sourceObject.HelperMaterial.Clone() as IHelperMaterial;
            helperMaterialUpdateStrategy.Update(targetObject.HelperMaterial, sourceObject.HelperMaterial);
        }


    }
}
