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
        public IRectangleShape GetClone(IRectangleShape sourceObject)
        {
            RectangleShape clone = new RectangleShape(Guid.NewGuid());
            updateStrategy ??= new RectangleShapeUpdateStrategy();
            updateStrategy.Update(clone, sourceObject);
            return clone;
        }
    }
}
