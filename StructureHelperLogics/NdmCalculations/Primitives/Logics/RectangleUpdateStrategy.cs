using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Primitives
{
    internal class RectangleUpdateStrategy : IUpdateStrategy<RectanglePrimitive>
    {
        static readonly BaseUpdateStrategy basePrimitiveUpdateStrategy = new();
        static readonly DivisionPropsUpdateStrategy divisionPropsUpdateStrategy = new();
        static readonly RectangleShapeUpdateStrategy shapeUpdateStrategy = new();
        public void Update(RectanglePrimitive targetObject, RectanglePrimitive sourceObject)
        {
            basePrimitiveUpdateStrategy.Update(targetObject, sourceObject);
            divisionPropsUpdateStrategy.Update(targetObject, sourceObject);
            shapeUpdateStrategy.Update(targetObject, sourceObject);
        }
    }
}
