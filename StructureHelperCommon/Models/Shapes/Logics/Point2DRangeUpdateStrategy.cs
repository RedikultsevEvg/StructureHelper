using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;

namespace StructureHelperCommon.Models.Shapes
{
    public class Point2DRangeUpdateStrategy : IParentUpdateStrategy<IPoint2DRange>
    {
        private IUpdateStrategy<IPoint2D> pointUpdateStrategy;
        private IUpdateStrategy<IPoint2D> PointUpdateStrategy => pointUpdateStrategy ??= new Point2DUpdateStrategy();

        public bool UpdateChildren { get; set; } = true;

        public void Update(IPoint2DRange targetObject, IPoint2DRange sourceObject)
        {
            CheckObject.ThrowIfNull(targetObject, sourceObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            if (UpdateChildren == true)
            {
                CheckObject.ThrowIfNull(targetObject.StartPoint, ": range start point");
                CheckObject.ThrowIfNull(targetObject.EndPoint, ": range end point");
                PointUpdateStrategy.Update(targetObject.StartPoint, sourceObject.StartPoint);
                PointUpdateStrategy.Update(targetObject.EndPoint, sourceObject.EndPoint);
            }
        }
    }
}
