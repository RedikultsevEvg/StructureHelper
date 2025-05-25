using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Forces
{
    public class FactoredForceTupleUpdateStrategy : IUpdateStrategy<IFactoredForceTuple>
    {
        public void Update(IFactoredForceTuple targetObject, IFactoredForceTuple sourceObject)
        {
            CheckObject.IsNull(targetObject);
            CheckObject.IsNull(sourceObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            CheckObject.IsNull(sourceObject.ForceTuple);
            targetObject.ForceTuple = sourceObject.ForceTuple.Clone() as IForceTuple;
            CheckObject.IsNull(sourceObject.CombinationProperty);
            targetObject.CombinationProperty = sourceObject.CombinationProperty.Clone() as IFactoredCombinationProperty;
        }
    }
}
