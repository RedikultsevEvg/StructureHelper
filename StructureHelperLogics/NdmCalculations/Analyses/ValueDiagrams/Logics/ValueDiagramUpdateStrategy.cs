using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Shapes;
using StructureHelperCommon.Services;

namespace StructureHelperLogics.NdmCalculations.Analyses.ValueDiagrams
{
    public class ValueDiagramUpdateStrategy : IParentUpdateStrategy<IValueDiagram>
    {
        private IUpdateStrategy<IPoint2DRange>? _rangeUpdateStrategy;

        public ValueDiagramUpdateStrategy(IUpdateStrategy<IPoint2DRange> rangeUpdateStrategy)
        {
            _rangeUpdateStrategy = rangeUpdateStrategy ?? throw new StructureHelperException("rangeUpdateStrategy cannot be null");
        }

        public ValueDiagramUpdateStrategy() { }

        public bool UpdateChildren { get; set; } = true;

        private IUpdateStrategy<IPoint2DRange> RangeUpdateStrategy
            => _rangeUpdateStrategy ??= new Point2DRangeUpdateStrategy();

        public void Update(IValueDiagram targetObject, IValueDiagram sourceObject)
        {
            CheckObject.ThrowIfNull(targetObject, nameof(targetObject));
            CheckObject.ThrowIfNull(sourceObject, nameof(sourceObject));

            if (ReferenceEquals(targetObject, sourceObject))
                return;

            targetObject.StepNumber = sourceObject.StepNumber;

            if (UpdateChildren)
            {
                // Use property for lazy initialization
                RangeUpdateStrategy.Update(targetObject.Point2DRange, sourceObject.Point2DRange);
            }
        }
    }

}
