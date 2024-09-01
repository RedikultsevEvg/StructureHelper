using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Primitives.Logics
{
    public class HasPrimitivesUpdateStrategy : IUpdateStrategy<IHasPrimitives>
    {
        public void Update(IHasPrimitives targetObject, IHasPrimitives sourceObject)
        {
            CheckObject.IsNull(targetObject, sourceObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            targetObject.Primitives.Clear();
            targetObject.Primitives.AddRange(sourceObject.Primitives);
        }
    }
}
