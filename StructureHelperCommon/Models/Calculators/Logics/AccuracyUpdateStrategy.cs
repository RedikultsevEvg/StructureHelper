using StructureHelperCommon.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Calculators
{
    public class AccuracyUpdateStrategy : IUpdateStrategy<IAccuracy>
    {
        public void Update(IAccuracy targetObject, IAccuracy sourceObject)
        {
            targetObject.IterationAccuracy = sourceObject.IterationAccuracy;
            targetObject.MaxIterationCount = sourceObject.MaxIterationCount;
        }
    }
}
