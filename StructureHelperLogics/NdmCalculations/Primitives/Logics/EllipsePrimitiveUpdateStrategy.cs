using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Shapes;
using StructureHelperCommon.Models.Shapes.Logics;
using StructureHelperCommon.Services;

namespace StructureHelperLogics.NdmCalculations.Primitives
{
    public class EllipsePrimitiveUpdateStrategy : IUpdateStrategy<IEllipsePrimitive>
    {
        private IUpdateStrategy<INdmPrimitive> basePrimitiveUpdateStrategy;
        private IUpdateStrategy<IDivisionSize> divisionPropsUpdateStrategy;
        private IUpdateStrategy<IShape> shapeUpdateStrategy;

        public EllipsePrimitiveUpdateStrategy(IUpdateStrategy<INdmPrimitive> basePrimitiveUpdateStrategy,
            IUpdateStrategy<IShape> shapeUpdateStrategy,
            IUpdateStrategy<IDivisionSize> divisionPropsUpdateStrategy)
        {
            this.basePrimitiveUpdateStrategy = basePrimitiveUpdateStrategy;
            this.shapeUpdateStrategy = shapeUpdateStrategy;
            this.divisionPropsUpdateStrategy = divisionPropsUpdateStrategy;
        }
        public EllipsePrimitiveUpdateStrategy() : this(
            new BaseUpdateStrategy(),
            new ShapeUpdateStrategy(),
            new DivisionSizeUpdateStrategy())
        {
            
        }
        public void Update(IEllipsePrimitive targetObject, IEllipsePrimitive sourceObject)
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
