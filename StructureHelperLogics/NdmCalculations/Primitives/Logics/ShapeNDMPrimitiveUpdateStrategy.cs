using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Shapes;
using StructureHelperCommon.Services;

namespace StructureHelperLogics.NdmCalculations.Primitives
{
    public class ShapeNdmPrimitiveUpdateStrategy : IParentUpdateStrategy<IShapeNdmPrimitive>
    {
        private IParentUpdateStrategy<INdmPrimitive> basePrimitiveUpdateStrategy;
        private IUpdateStrategy<IDivisionSize> divisionPropsUpdateStrategy;
        private IUpdateStrategy<IShape> shapeUpdateStrategy;

        public bool UpdateChildren { get; set; } = true;

        public ShapeNdmPrimitiveUpdateStrategy(
            IParentUpdateStrategy<INdmPrimitive> basePrimitiveUpdateStrategy,
            IUpdateStrategy<IShape> shapeUpdateStrategy,
            IUpdateStrategy<IDivisionSize> divisionPropsUpdateStrategy)
        {
            this.basePrimitiveUpdateStrategy = basePrimitiveUpdateStrategy;
            this.shapeUpdateStrategy = shapeUpdateStrategy;
            this.divisionPropsUpdateStrategy = divisionPropsUpdateStrategy;
        }

        public ShapeNdmPrimitiveUpdateStrategy() : this(
            new NdmPrimitiveBaseUpdateStrategy(),
            new ShapeUpdateStrategy(),
            new DivisionSizeUpdateStrategy())
        {

        }

        public void Update(IShapeNdmPrimitive targetObject, IShapeNdmPrimitive sourceObject)
        {
            CheckObject.IsNull(targetObject, sourceObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            InitializeStrategies();
            basePrimitiveUpdateStrategy.Update(targetObject, sourceObject);
            if (UpdateChildren == true)
            {
                divisionPropsUpdateStrategy.Update(targetObject.DivisionSize, sourceObject.DivisionSize);
                shapeUpdateStrategy.Update(targetObject.Shape, sourceObject.Shape);
            }
        }

        private void InitializeStrategies()
        {
            basePrimitiveUpdateStrategy ??= new NdmPrimitiveBaseUpdateStrategy();
            basePrimitiveUpdateStrategy.UpdateChildren = UpdateChildren;
            divisionPropsUpdateStrategy ??= new DivisionSizeUpdateStrategy();
            shapeUpdateStrategy ??= new ShapeUpdateStrategy();
        }
    }
}
