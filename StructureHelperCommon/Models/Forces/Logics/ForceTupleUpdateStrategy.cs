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
            CheckObject.IsNull(targetObject, ": target object ");
            CheckObject.IsNull(sourceObject, ": source object ");

            targetObject.Mx = sourceObject.Mx;
            targetObject.My = sourceObject.My;
            targetObject.Nz = targetObject.Nz;
            targetObject.Qx = targetObject.Qx;
            targetObject.Qy = targetObject.Qy;
            targetObject.Mz = sourceObject.Mz;
        }
    }
}
