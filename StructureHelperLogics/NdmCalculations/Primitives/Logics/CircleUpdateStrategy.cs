using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Primitives
{
    internal class CircleUpdateStrategy : IUpdateStrategy<CirclePrimitive>
    {
        static readonly BaseUpdateStrategy basePrimitiveUpdateStrategy = new();
        static readonly DivisionPropsUpdateStrategy divisionPropsUpdateStrategy = new();
        static readonly CircleShapeUpdateStrategy shapeUpdateStrategy = new();

        public void Update(CirclePrimitive targetObject, CirclePrimitive sourceObject)
        {
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            basePrimitiveUpdateStrategy.Update(targetObject, sourceObject);
            divisionPropsUpdateStrategy.Update(targetObject, sourceObject);
            shapeUpdateStrategy.Update(targetObject, sourceObject);
        }
    }
}
