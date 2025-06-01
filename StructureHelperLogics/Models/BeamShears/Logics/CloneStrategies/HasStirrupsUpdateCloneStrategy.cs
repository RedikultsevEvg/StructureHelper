using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.BeamShears
{
    public class HasStirrupsUpdateCloneStrategy : IUpdateStrategy<IHasStirrups>
    {
        private readonly ICloningStrategy cloningStrategy;

        public HasStirrupsUpdateCloneStrategy(ICloningStrategy cloningStrategy)
        {
            this.cloningStrategy = cloningStrategy;
        }

        public void Update(IHasStirrups targetObject, IHasStirrups sourceObject)
        {
            CheckObject.IsNull(cloningStrategy);
            CheckObject.IsNull(sourceObject);
            CheckObject.IsNull(targetObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            targetObject.Stirrups.Clear();
            foreach (var item in sourceObject.Stirrups)
            {
                IStirrup newStirrup = cloningStrategy.Clone(item);
                targetObject.Stirrups.Add(newStirrup);
            }
        }
    }
}
