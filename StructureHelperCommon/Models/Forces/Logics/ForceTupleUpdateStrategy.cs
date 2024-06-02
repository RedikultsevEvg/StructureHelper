using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Forces
{
    public class ForceTupleUpdateStrategy : IUpdateStrategy<IForceTuple>
    {
        public void Update(IForceTuple targetObject, IForceTuple sourceObject)
        {
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            CheckObject.CompareTypes(targetObject, sourceObject);

            targetObject.Mx = sourceObject.Mx;
            targetObject.My = sourceObject.My;
            targetObject.Nz = sourceObject.Nz;
            targetObject.Qx = sourceObject.Qx;
            targetObject.Qy = sourceObject.Qy;
            targetObject.Mz = sourceObject.Mz;
        }
    }
}
