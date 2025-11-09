using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;

namespace StructureHelperLogics.NdmCalculations.Analyses.ValueDiagrams
{
    public class ValueDiagramCalculatorInputDataUpdateStrategy : IParentUpdateStrategy<IValueDiagramCalculatorInputData>
    {
        private IUpdateStrategy<IValueDiagramEntity> entityUpdateStrategy;

        public ValueDiagramCalculatorInputDataUpdateStrategy(IUpdateStrategy<IValueDiagramEntity> entityUpdateStrategy)
        {
            this.entityUpdateStrategy = entityUpdateStrategy;
        }

        public ValueDiagramCalculatorInputDataUpdateStrategy()
        {
            
        }

        public bool UpdateChildren { get; set; } = true;

        public void Update(IValueDiagramCalculatorInputData targetObject, IValueDiagramCalculatorInputData sourceObject)
        {
            CheckObject.IsNull(targetObject, sourceObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            targetObject.CheckStrainLimit = sourceObject.CheckStrainLimit;
            targetObject.StateTermPair.LimitState = sourceObject.StateTermPair.LimitState;
            targetObject.StateTermPair.CalcTerm = sourceObject.StateTermPair.CalcTerm;
            if (UpdateChildren == true)
            {
                targetObject.Primitives.Clear();
                targetObject.Primitives.AddRange(sourceObject.Primitives);
                targetObject.ForceActions.Clear();
                targetObject.ForceActions.AddRange(sourceObject.ForceActions);
                targetObject.Digrams.Clear();
                entityUpdateStrategy ??= new ValueDiagramEntityUpdateStrategy();
                foreach (var entity in sourceObject.Digrams)
                {
                    var newItem = entity.Clone() as IValueDiagramEntity;
                    targetObject.Digrams.Add(newItem);
                }
            }
        }
    }
}
