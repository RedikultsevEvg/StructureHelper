using StructureHelperCommon.Infrastructures.Interfaces;
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
        public IRectangleShape GetClone(IRectangleShape sourceObject)
        {
            RectangleShape clone = new RectangleShape(Guid.NewGuid());
            UpdateStrategy.Update(clone, sourceObject);
            return clone;
        }
    }
}
