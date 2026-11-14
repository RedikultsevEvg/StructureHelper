using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperCommon.Models.Shapes
{
    public class RingShapeCloneStrategy : ICloneStrategy<IRingShape>
    {
        private IUpdateStrategy<IRingShape> updateStrategy;
        private IUpdateStrategy<IRingShape> UpdateStrategy => updateStrategy ??= new RingShapeUpdateStrategy();

        public IRingShape GetClone(IRingShape sourceObject, CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            CheckObject.ThrowIfNull(sourceObject);
            if (cloneMode == CloneMode.Reference) { return sourceObject; }
            Guid id = GuidService.GetGuid(sourceObject, cloneMode);
            RingShape clone = new(id);
            UpdateStrategy.Update(clone, sourceObject);
            return clone;
        }
    }
}
