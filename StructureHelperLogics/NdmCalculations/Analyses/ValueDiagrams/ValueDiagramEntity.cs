using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Services;

namespace StructureHelperLogics.NdmCalculations.Analyses.ValueDiagrams
{
    public class ValueDiagramEntity : IValueDiagramEntity
    {
        public Guid Id { get; }
        public string Name { get; set; } = string.Empty;
        public bool IsTaken { get; set; } = true;

        public IValueDiagram ValueDiagram { get; set; } = new ValueDiagram(Guid.NewGuid());

        public ValueDiagramEntity(Guid id)
        {
            Id = id;
        }

        public IValueDiagramEntity GetClone(CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            if (cloneMode == CloneMode.Reference) { return this; }
            Guid id = GuidService.GetGuid(this, cloneMode);
            ValueDiagramEntity newItem = new ValueDiagramEntity(id);
            var updateStrategy = new ValueDiagramEntityUpdateStrategy();
            updateStrategy.Update(newItem, this);
            return newItem;
        }
    }
}
