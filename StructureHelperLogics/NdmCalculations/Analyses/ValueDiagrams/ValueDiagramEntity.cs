namespace StructureHelperLogics.NdmCalculations.Analyses.ValueDiagrams
{
    public class ValueDiagramEntity : IValueDiagramEntity
    {
        public Guid Id { get; }
        public string Name { get; set; } = string.Empty;
        public bool IsTaken { get; set; } = true;

        public IValueDiagram ValueDigram { get; set; } = new ValueDiagram(Guid.NewGuid());

        public ValueDiagramEntity(Guid id)
        {
            Id = id;
        }

        public object Clone()
        {
            ValueDiagramEntity newItem = new ValueDiagramEntity(Guid.NewGuid());
            var updateStrategy = new ValueDiagramEntityUpdateStrategy();
            updateStrategy.Update(newItem, this);
            return newItem;
        }
    }
}
