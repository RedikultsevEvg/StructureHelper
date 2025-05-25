using StructureHelperCommon.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Forces.BeamShearActions
{
    internal class BeamShearAxisActionCloneStrategy : ICloneStrategy<IBeamShearAxisAction>
    {
        private ICloningStrategy cloningStrategy;
        private IUpdateStrategy<IBeamShearAxisAction> updateStrategy; 

        public BeamShearAxisActionCloneStrategy(ICloningStrategy cloningStrategy)
        {
            this.cloningStrategy = cloningStrategy;
        }

        private BeamShearAxisAction targetObject;

        public IBeamShearAxisAction GetClone(IBeamShearAxisAction sourceObject)
        {
            InitializeStrategies();
            targetObject = new(Guid.NewGuid());
            updateStrategy.Update(targetObject, sourceObject);
            targetObject.ShearLoads.Clear();
            foreach (var shearLoad in sourceObject.ShearLoads)
            {
                targetObject.ShearLoads.Add(cloningStrategy.Clone(shearLoad));
            }
            return targetObject;
        }

        private void InitializeStrategies()
        {
            updateStrategy ??= new BeamShearAxisActionUpdateStrategy();
        }
    }
}
