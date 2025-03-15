using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Forces.BeamShearActions
{
    public class BeamShearLoadBaseUpdateStrategy : IUpdateStrategy<IBeamShearLoad>
    {
        public void Update(IBeamShearLoad targetObject, IBeamShearLoad sourceObject)
        {
            CheckObject.IsNull(targetObject);
            CheckObject.IsNull(sourceObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            targetObject.Name = sourceObject.Name;
            targetObject.LoadRatio = sourceObject.LoadRatio;
            targetObject.RelativeLoadLevel = sourceObject.RelativeLoadLevel;
        }
    }
}
