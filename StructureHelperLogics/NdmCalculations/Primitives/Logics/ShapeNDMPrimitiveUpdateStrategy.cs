using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Shapes;
using StructureHelperCommon.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Primitives
{
    public class ShapeNDMPrimitiveUpdateStrategy : IUpdateStrategy<IShapeNDMPrimitive>
    {
        private IUpdateStrategy<INdmPrimitive> basePrimitiveUpdateStrategy;
        private IUpdateStrategy<IDivisionSize> divisionPropsUpdateStrategy;
        private IUpdateStrategy<IShape> shapeUpdateStrategy;

        public ShapeNDMPrimitiveUpdateStrategy(
            IUpdateStrategy<INdmPrimitive> basePrimitiveUpdateStrategy,
            IUpdateStrategy<IShape> shapeUpdateStrategy,
            IUpdateStrategy<IDivisionSize> divisionPropsUpdateStrategy)
        {
            this.basePrimitiveUpdateStrategy = basePrimitiveUpdateStrategy;
            this.shapeUpdateStrategy = shapeUpdateStrategy;
            this.divisionPropsUpdateStrategy = divisionPropsUpdateStrategy;
        }

        public ShapeNDMPrimitiveUpdateStrategy() : this(
            new BaseUpdateStrategy(),
            new ShapeUpdateStrategy(),
            new DivisionSizeUpdateStrategy())
        {

        }

        public void Update(IShapeNDMPrimitive targetObject, IShapeNDMPrimitive sourceObject)
        {
            CheckObject.IsNull(sourceObject, "source object");
            CheckObject.IsNull(targetObject, "target object");
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            InitializeStrategies();
            basePrimitiveUpdateStrategy.Update(targetObject, sourceObject);
            divisionPropsUpdateStrategy.Update(targetObject.DivisionSize, sourceObject.DivisionSize);
            shapeUpdateStrategy.Update(targetObject.Shape, sourceObject.Shape);
        }

        private void InitializeStrategies()
        {
            basePrimitiveUpdateStrategy ??= new BaseUpdateStrategy();
            divisionPropsUpdateStrategy ??= new DivisionSizeUpdateStrategy();
            shapeUpdateStrategy ??= new ShapeUpdateStrategy();
        }
    }
}
