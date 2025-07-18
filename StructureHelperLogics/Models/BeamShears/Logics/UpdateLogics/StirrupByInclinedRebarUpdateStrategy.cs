using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;
using StructureHelperLogics.Models.Materials;
using StructureHelperLogics.NdmCalculations.Primitives;

namespace StructureHelperLogics.Models.BeamShears
{
    public class StirrupByInclinedRebarUpdateStrategy : IParentUpdateStrategy<IStirrupByInclinedRebar>
    {
        private IUpdateStrategy<IStirrup>? baseUpdateStrategy;

        public bool UpdateChildren { get; set; } = true;

        public void Update(IStirrupByInclinedRebar targetObject, IStirrupByInclinedRebar sourceObject)
        {
            CheckObject.IsNull(sourceObject, ErrorStrings.SourceObject);
            CheckObject.IsNull(targetObject, ErrorStrings.TargetObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            baseUpdateStrategy ??= new StirrupBaseUpdateStrategy();
            baseUpdateStrategy.Update(targetObject, sourceObject);
            targetObject.StartCoordinate = sourceObject.StartCoordinate;
            targetObject.TransferLength = sourceObject.TransferLength;
            targetObject.AngleOfInclination = sourceObject.AngleOfInclination;
            targetObject.LegCount = sourceObject.LegCount;
            if (UpdateChildren)
            {
                CheckObject.IsNull(sourceObject.RebarSection, "Rebar section");
                targetObject.RebarSection = sourceObject.RebarSection.Clone() as IRebarSection;
            }
        }
    }
}
