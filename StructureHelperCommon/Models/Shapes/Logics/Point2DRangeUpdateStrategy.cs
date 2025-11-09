using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;

namespace StructureHelperCommon.Models.Shapes
{
    public class Point2DRangeUpdateStrategy : IUpdateStrategy<IPoint2DRange>
    {
        private IUpdateStrategy<IPoint2D> pointUpdateStrategy;
        public void Update(IPoint2DRange targetObject, IPoint2DRange sourceObject)
        {
            CheckObject.IsNull(targetObject, sourceObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            CheckObject.IsNull(targetObject.StartPoint, ": range start point");
            pointUpdateStrategy ??= new Point2DUpdateStrategy();
            pointUpdateStrategy.Update(targetObject.StartPoint, sourceObject.StartPoint);
            CheckObject.IsNull(targetObject.EndPoint, ": range end point");
            pointUpdateStrategy.Update(targetObject.EndPoint, sourceObject.EndPoint);
        }
    }
}
