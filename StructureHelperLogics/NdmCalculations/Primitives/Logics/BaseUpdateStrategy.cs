using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Shapes;
using StructureHelperCommon.Models.Shapes.Logics;
using StructureHelperCommon.Services;
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
        static readonly Point2DUpdateStrategy point2DUpdateStrategy = new();
        readonly ForceTupleUpdateStrategy tupleUpdateStrategy = new();
        readonly VisualPropsUpdateStrategy visualPropsUpdateStrategy = new();

        public void Update(INdmPrimitive targetObject, INdmPrimitive sourceObject)
        {
            CheckObject.IsNull(targetObject, sourceObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            targetObject.Name = sourceObject.Name;
            if (sourceObject.HeadMaterial != null) targetObject.HeadMaterial = sourceObject.HeadMaterial;
            targetObject.Triangulate = sourceObject.Triangulate;
            point2DUpdateStrategy.Update(targetObject.Center, sourceObject.Center);
            visualPropsUpdateStrategy.Update(targetObject.VisualProperty, sourceObject.VisualProperty);
            tupleUpdateStrategy.Update(targetObject.UsersPrestrain, sourceObject.UsersPrestrain);
        }

    }
}
