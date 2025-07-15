using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;

namespace StructureHelperLogics.Models.Materials
{
    public class RebarSectionUpdateStrategy : IUpdateStrategy<IRebarSection>
    {
        public void Update(IRebarSection targetObject, IRebarSection sourceObject)
        {
            CheckObject.IsNull(sourceObject);
            CheckObject.IsNull(targetObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            CheckObject.IsNull(sourceObject.Material);
            targetObject.Material = sourceObject.Material.Clone() as IReinforcementLibMaterial;
            targetObject.Diameter = sourceObject.Diameter;
        }
    }
}
