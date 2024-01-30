using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces.LimitCurve;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces.Logics
{
    internal class SurroundDataUpdateStrategy : IUpdateStrategy<SurroundData>
    {
        public void Update(SurroundData targetObject, SurroundData sourceObject)
        {
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            targetObject.XMax = sourceObject.XMax;
            targetObject.XMin = sourceObject.XMin;
            targetObject.YMax = sourceObject.YMax;
            targetObject.YMin = sourceObject.YMin;
            targetObject.ConvertLogicEntity = sourceObject.ConvertLogicEntity;
        }
    }
}
