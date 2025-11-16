using StructureHelperCommon.Models.Shapes;

namespace StructureHelperLogics.NdmCalculations.Analyses.ValueDiagrams
{
    public class ValueDiagram : IValueDiagram
    {
        private int stepNumber = 50;

        public Guid Id { get; }
        public int StepNumber
        {
            get => stepNumber;
            set
            {
                stepNumber = Math.Max(value, 10);
            }
        }
        public IPoint2DRange Point2DRange { get; set; } = new Point2DRange(Guid.NewGuid());


        public ValueDiagram(Guid id)
        {
            Id = id;
        }

        public object Clone()
        {
            ValueDiagram newItem = new(Guid.NewGuid());
            var updateStrategy = new ValueDiagramUpdateStrategy();
            updateStrategy.Update(newItem, this);
            return newItem;
        }
    }
}
