using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;
using StructureHelperLogics.Models.Primitives;

namespace StructureHelperLogics.NdmCalculations.Primitives
{
    public class NdmPrimitiveUpdateStrategy : IUpdateStrategy<INdmPrimitive>
    {
        public void Update(INdmPrimitive targetObject, INdmPrimitive sourceObject)
        {
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            CheckObject.CompareTypes(targetObject, sourceObject);
            if (targetObject is PointPrimitive point)
            {
                new PointUpdateStrategy().Update(point, (PointPrimitive)sourceObject);
            }
            else if (targetObject is RebarPrimitive rebar)
            {
                new RebarUpdateStrategy().Update(rebar, (RebarPrimitive)sourceObject);
            }
            else if (targetObject is RectanglePrimitive rectangle)
            {
                new RectanglePrimitiveUpdateStrategy().Update(rectangle, (RectanglePrimitive)sourceObject);
            }
            else if (targetObject is EllipsePrimitive circle)
            {
                new EllipsePrimitiveUpdateStrategy().Update(circle, (EllipsePrimitive)sourceObject);
            }
            else
            {
                ErrorCommonProcessor.ObjectTypeIsUnknown(typeof(INdmPrimitive), sourceObject.GetType());
            }
        }
    }
}
