using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;

namespace StructureHelperLogics.Models.Materials
{
    public class RebarSectionUpdateStrategy : IParentUpdateStrategy<IRebarSection>
    {
        public bool UpdateChildren { get; set; } = true;

        public void Update(IRebarSection targetObject, IRebarSection sourceObject)
        {
            CheckObject.IsNull(sourceObject);
            CheckObject.IsNull(targetObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            CheckObject.IsNull(sourceObject.Material);
            targetObject.Diameter = sourceObject.Diameter;
            if (UpdateChildren)
            {
                targetObject.Material = sourceObject.Material.Clone() as IReinforcementLibMaterial;
            }
        }
    }
}
