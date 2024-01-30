using StructureHelperCommon.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Shapes
{
    public class RectangleShapeUpdateStrategy : IUpdateStrategy<IRectangleShape>
    {
        public void Update(IRectangleShape targetObject, IRectangleShape sourceObject)
        {
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            targetObject.Width = sourceObject.Width;
            targetObject.Height = sourceObject.Height;
            targetObject.Angle = sourceObject.Angle;
        }
    }
}
