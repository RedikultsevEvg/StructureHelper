using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Forces.BeamShearActions
{
    internal class BeamShearAxisActionCloneStrategy : ICloneStrategy<IBeamShearAxisAction>
    {
        private readonly ICloningStrategy cloningStrategy;
        private IUpdateStrategy<IBeamShearAxisAction> updateStrategy;

        public BeamShearAxisActionCloneStrategy(ICloningStrategy cloningStrategy)
        {
            this.cloningStrategy = cloningStrategy;
        }

        public IBeamShearAxisAction GetClone(IBeamShearAxisAction sourceObject, CloningMode cloningMode)
        {
            CheckObject.ThrowIfNull(sourceObject);
            if (cloningMode == CloningMode.Reference) { return sourceObject; }
            Guid id = GuidService.GetGuid(sourceObject, cloningMode);
            updateStrategy ??= new BeamShearAxisActionUpdateStrategy();
            var targetObject = new BeamShearAxisAction(id);
            updateStrategy.Update(targetObject, sourceObject);
            targetObject.ShearLoads.Clear();
            foreach (var shearLoad in sourceObject.ShearLoads)
            {
                targetObject.ShearLoads.Add(cloningStrategy.Clone(shearLoad, null, cloningMode));
            }
            return targetObject;
        }
    }
}
