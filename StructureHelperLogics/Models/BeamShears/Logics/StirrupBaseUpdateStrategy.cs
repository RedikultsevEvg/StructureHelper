using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;

namespace StructureHelperLogics.Models.BeamShears
{
    public class StirrupBaseUpdateStrategy : IUpdateStrategy<IStirrup>
    {
        public void Update(IStirrup targetObject, IStirrup sourceObject)
        {
            CheckObject.IsNull(targetObject);
            CheckObject.IsNull(sourceObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            targetObject.Name = sourceObject.Name;
            targetObject.CompressedGap = sourceObject.CompressedGap;
        }
    }
}
