using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;
using StructureHelperLogics.NdmCalculations.Primitives;

namespace StructureHelperLogics.Models.BeamShears
{
    public class StirrupByInclinedRebarUpdateStrategy : IUpdateStrategy<IStirrupByInclinedRebar>
    {
        private IUpdateStrategy<IStirrup>? baseUpdateStrategy;
        public void Update(IStirrupByInclinedRebar targetObject, IStirrupByInclinedRebar sourceObject)
        {
            CheckObject.IsNull(sourceObject, ErrorStrings.SourceObject);
            CheckObject.IsNull(targetObject, ErrorStrings.TargetObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            baseUpdateStrategy ??= new StirrupBaseUpdateStrategy();
            baseUpdateStrategy.Update(targetObject, sourceObject);
            targetObject.StartCoordinate = sourceObject.StartCoordinate;
            targetObject.OffSet = sourceObject.OffSet;
            targetObject.AngleOfInclination = sourceObject.AngleOfInclination;
        }
    }
}
