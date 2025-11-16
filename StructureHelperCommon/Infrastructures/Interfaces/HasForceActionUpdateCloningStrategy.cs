using StructureHelperCommon.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Infrastructures.Interfaces
{
    /// <summary>
    /// Creates deep copy of internal elements of object which has calculators
    /// </summary>
    public class HasForceActionUpdateCloningStrategy : IUpdateStrategy<IHasForceActions>
    {
        private ICloningStrategy cloningStrategy;

        public HasForceActionUpdateCloningStrategy(ICloningStrategy cloningStrategy)
        {
            this.cloningStrategy = cloningStrategy;
        }

        public void Update(IHasForceActions targetObject, IHasForceActions sourceObject)
        {
            CheckObject.ThrowIfNull(cloningStrategy);
            CheckObject.ThrowIfNull(sourceObject);
            CheckObject.ThrowIfNull(targetObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            targetObject.ForceActions.Clear();
            foreach (var force in sourceObject.ForceActions)
            {
                var newForce = cloningStrategy.Clone(force);
                targetObject.ForceActions.Add(newForce);
            }
        }
    }
}
