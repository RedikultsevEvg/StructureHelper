using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;

namespace StructureHelperLogics.NdmCalculations.Analyses.ValueDiagrams
{
    public class ValueDiagramEntityUpdateStrategy : IParentUpdateStrategy<IValueDiagramEntity>
    {
        private IUpdateStrategy<IValueDiagram> valueDiagramUpdateStrategy;

        public ValueDiagramEntityUpdateStrategy(IUpdateStrategy<IValueDiagram> valueDiagramUpdateStrategy)
        {
            this.valueDiagramUpdateStrategy = valueDiagramUpdateStrategy;
        }

        public ValueDiagramEntityUpdateStrategy()
        {
            
        }

        public bool UpdateChildren { get; set; } = true;

        public void Update(IValueDiagramEntity targetObject, IValueDiagramEntity sourceObject)
        {
            CheckObject.ThrowIfNull(targetObject, sourceObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            targetObject.IsTaken = sourceObject.IsTaken;
            targetObject.Name = sourceObject.Name;
            if (UpdateChildren == true)
            {
                valueDiagramUpdateStrategy ??= new ValueDiagramUpdateStrategy();
                valueDiagramUpdateStrategy.Update(targetObject.ValueDiagram, sourceObject.ValueDiagram);
            }
        }
    }
}
