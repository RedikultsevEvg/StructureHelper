using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Shapes;
using StructureHelperCommon.Models.Shapes.Logics;
using StructureHelperCommon.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Primitives
{
    internal class RectanglePrimitiveUpdateStrategy : IUpdateStrategy<IRectanglePrimitive>
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
            new BaseUpdateStrategy(),
            new ShapeUpdateStrategy(),
            new DivisionPropsUpdateStrategy())
        {

        }

        public void Update(IRectanglePrimitive targetObject, IRectanglePrimitive sourceObject)
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
