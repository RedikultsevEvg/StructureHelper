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


        public IRingShape GetClone(IRingShape sourceObject)
        {
            CheckObject.ThrowIfNull(sourceObject);
            RingShape clone = new(Guid.NewGuid());
            UpdateStrategy.Update(clone, sourceObject);
            return clone;
        }
    }
}
