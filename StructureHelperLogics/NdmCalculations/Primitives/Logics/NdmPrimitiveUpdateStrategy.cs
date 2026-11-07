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
            if (targetObject is IPointNdmPrimitive point)
            {
                new PointNdmPrimitiveUpdateStrategy().Update(point, (IPointNdmPrimitive)sourceObject);
            }
            else if (targetObject is IRebarNdmPrimitive rebar)
            {
                new RebarNdmPrimitiveUpdateStrategy().Update(rebar, (IRebarNdmPrimitive)sourceObject);
            }
            else if (targetObject is IRectangleNdmPrimitive rectangle)
            {
                new RectanglePrimitiveUpdateStrategy().Update(rectangle, (IRectangleNdmPrimitive)sourceObject);
            }
            else if (targetObject is IEllipseNdmPrimitive circle)
            {
                new EllipsePrimitiveUpdateStrategy().Update(circle, (IEllipseNdmPrimitive)sourceObject);
            }
            else if (targetObject is IShapeNdmPrimitive shapePrimitive)
            {
                new ShapeNdmPrimitiveUpdateStrategy().Update(shapePrimitive, (IShapeNdmPrimitive)sourceObject);
            }
            else
            {
                ErrorCommonProcessor.ObjectTypeIsUnknown(typeof(INdmPrimitive), sourceObject.GetType());
            }
        }
    }
}
