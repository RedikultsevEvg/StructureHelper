using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Shapes;
using StructureHelperCommon.Services;

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

        public IValueDiagram GetClone(CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            if (cloneMode == CloneMode.Reference) { return this; }
            Guid id = GuidService.GetGuid(this, cloneMode);
            ValueDiagram newItem = new(id);
            var updateStrategy = new ValueDiagramUpdateStrategy();
            updateStrategy.Update(newItem, this);
            return newItem;
        }
    }
}
