using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Shapes;
using StructureHelperCommon.Services;

namespace StructureHelperLogics.NdmCalculations.Analyses.ValueDiagrams
{
    public class ValueDiagramUpdateStrategy : IParentUpdateStrategy<IValueDiagram>
    {
        private IUpdateStrategy<IPoint2DRange> rangeUpdateStrategy;
        public bool UpdateChildren { get; set; } = true;

        public void Update(IValueDiagram targetObject, IValueDiagram sourceObject)
        {
            CheckObject.IsNull(targetObject, sourceObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            targetObject.StepNumber = sourceObject.StepNumber;
            if (UpdateChildren == true)
            {
                rangeUpdateStrategy ??= new Point2DRangeUpdateStrategy();
                rangeUpdateStrategy.Update(targetObject.Point2DRange, sourceObject.Point2DRange);
            }
        }
    }
}
