using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Shapes.Logics;
using StructureHelperCommon.Services.Forces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Primitives
{
    internal class BaseUpdateStrategy : IUpdateStrategy<INdmPrimitive>
    {
        static readonly PointShapeUpdateStrategy pointShapeUpdateStrategy = new();
        readonly ForceTupleUpdateStrategy tupleUpdateStrategy = new();
        readonly VisualPropsUpdateStrategy visualPropsUpdateStrategy = new();

        public void Update(INdmPrimitive target, INdmPrimitive source)
        {
            target.Name = source.Name;
            if (source.HeadMaterial != null) target.HeadMaterial = source.HeadMaterial;
            target.Triangulate = source.Triangulate;
            pointShapeUpdateStrategy.Update(target.Center, source.Center);
            visualPropsUpdateStrategy.Update(target.VisualProperty, source.VisualProperty);
            tupleUpdateStrategy.Update(target.UsersPrestrain, source.UsersPrestrain);
        }

    }
}
