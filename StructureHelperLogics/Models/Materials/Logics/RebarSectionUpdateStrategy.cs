using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;

namespace StructureHelperLogics.Models.Materials
{
    public class RebarSectionUpdateStrategy : IParentUpdateStrategy<IRebarSection>
    {
        public bool UpdateChildren { get; set; } = true;

        public void Update(IRebarSection targetObject, IRebarSection sourceObject)
        {
            CheckObject.ThrowIfNull(sourceObject);
            CheckObject.ThrowIfNull(targetObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            targetObject.Diameter = sourceObject.Diameter;
            if (UpdateChildren == true)
            {
                CheckObject.ThrowIfNull(sourceObject.Material);
                targetObject.Material = sourceObject.Material.GetClone() as IReinforcementLibMaterial;
            }
        }
    }
}
