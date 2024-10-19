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
            if (targetObject is PointNdmPrimitive point)
            {
                new PointPrimitiveUpdateStrategy().Update(point, (PointNdmPrimitive)sourceObject);
            }
            else if (targetObject is RebarNdmPrimitive rebar)
            {
                new RebarNdmPrimitiveUpdateStrategy().Update(rebar, (RebarNdmPrimitive)sourceObject);
            }
            else if (targetObject is RectangleNdmPrimitive rectangle)
            {
                new RectanglePrimitiveUpdateStrategy().Update(rectangle, (RectangleNdmPrimitive)sourceObject);
            }
            else if (targetObject is EllipseNdmPrimitive circle)
            {
                new EllipsePrimitiveUpdateStrategy().Update(circle, (EllipseNdmPrimitive)sourceObject);
            }
            else
            {
                ErrorCommonProcessor.ObjectTypeIsUnknown(typeof(INdmPrimitive), sourceObject.GetType());
            }
        }
    }
}
