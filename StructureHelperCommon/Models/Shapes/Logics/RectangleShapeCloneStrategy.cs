using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Shapes
{
    public class RectangleShapeCloneStrategy : ICloneStrategy<IRectangleShape>
    {
        IUpdateStrategy<IRectangleShape> updateStrategy;
        IUpdateStrategy<IRectangleShape> UpdateStrategy => updateStrategy ??= new RectangleShapeUpdateStrategy();

        public IRectangleShape GetClone(IRectangleShape sourceObject, CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            CheckObject.ThrowIfNull(sourceObject);
            if (cloneMode == CloneMode.Reference) { return sourceObject; }
            Guid id = GuidService.GetGuid(sourceObject, cloneMode);
            RectangleShape clone = new(id);
            UpdateStrategy.Update(clone, sourceObject);
            return clone;
        }
    }
}
