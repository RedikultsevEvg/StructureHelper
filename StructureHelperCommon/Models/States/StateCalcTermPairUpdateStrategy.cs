using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.States
{
    public class StateCalcTermPairUpdateStrategy : IUpdateStrategy<IStateCalcTermPair>
    {
        public void Update(IStateCalcTermPair targetObject, IStateCalcTermPair sourceObject)
        {
            CheckObject.ThrowIfNull(targetObject);
            CheckObject.ThrowIfNull(sourceObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            targetObject.LimitState = sourceObject.LimitState;
            targetObject.CalcTerm = sourceObject.CalcTerm;
        }
    }
}
