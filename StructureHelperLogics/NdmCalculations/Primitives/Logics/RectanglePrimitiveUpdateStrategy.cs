using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Shapes;
using StructureHelperCommon.Services;

namespace StructureHelperLogics.NdmCalculations.Primitives
{
    public class RectanglePrimitiveUpdateStrategy : IUpdateStrategy<IRectangleNdmPrimitive>
    {
        private IUpdateStrategy<INdmPrimitive> basePrimitiveUpdateStrategy;
        private IUpdateStrategy<IDivisionSize> divisionPropsUpdateStrategy;
        private IUpdateStrategy<IShape> shapeUpdateStrategy;

        public RectanglePrimitiveUpdateStrategy(IUpdateStrategy<INdmPrimitive> basePrimitiveUpdateStrategy,
            IUpdateStrategy<IShape> shapeUpdateStrategy,
            IUpdateStrategy<IDivisionSize> divisionPropsUpdateStrategy)
        {
            this.basePrimitiveUpdateStrategy = basePrimitiveUpdateStrategy;
            this.shapeUpdateStrategy = shapeUpdateStrategy;
            this.divisionPropsUpdateStrategy = divisionPropsUpdateStrategy;
        }
        public RectanglePrimitiveUpdateStrategy() : this(
            new NdmPrimitiveBaseUpdateStrategy(),
            new ShapeUpdateStrategy(),
            new DivisionSizeUpdateStrategy())
        {

        }

        public void Update(IRectangleNdmPrimitive targetObject, IRectangleNdmPrimitive sourceObject)
        {
            CheckObject.IsNull(sourceObject, "source object");
            CheckObject.IsNull(targetObject, "target object");
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            basePrimitiveUpdateStrategy.Update(targetObject, sourceObject);
            divisionPropsUpdateStrategy.Update(targetObject.DivisionSize, sourceObject.DivisionSize);
            shapeUpdateStrategy.Update(targetObject.Shape, sourceObject.Shape);
        }
    }
}
