using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;
using StructureHelperLogics.NdmCalculations.Primitives;

namespace StructureHelperLogics.Models.BeamShears
{
    public class StirrupGroupUpdateStrategy : IParentUpdateStrategy<IStirrupGroup>
    {
        private StirrupBaseUpdateStrategy baseUpdateStrategy;

        public bool UpdateChildren { get; set; } = true;

        public void Update(IStirrupGroup targetObject, IStirrupGroup sourceObject)
        {
            CheckObject.IsNull(sourceObject, ErrorStrings.SourceObject);
            CheckObject.IsNull(targetObject, ErrorStrings.TargetObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            baseUpdateStrategy ??= new StirrupBaseUpdateStrategy();
            baseUpdateStrategy.Update(targetObject, sourceObject);
            if (UpdateChildren == true)
            {
                UpdateTargetChildren(targetObject, sourceObject);
            }
        }

        private static void UpdateTargetChildren(IStirrupGroup targetObject, IStirrupGroup sourceObject)
        {
            CheckObject.IsNull(sourceObject.Stirrups);
            CheckObject.IsNull(targetObject.Stirrups);
            targetObject.Stirrups.Clear();
            foreach (var item in sourceObject.Stirrups)
            {
                IStirrup? newItem = item.Clone() as IStirrup;
                targetObject.Stirrups.Add(newItem);
            }
        }
    }
}
