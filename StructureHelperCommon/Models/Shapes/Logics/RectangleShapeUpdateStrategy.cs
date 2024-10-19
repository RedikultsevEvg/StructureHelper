using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;
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
            CheckObject.IsNull(sourceObject);
            CheckObject.IsNull(targetObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            targetObject.Width = sourceObject.Width;
            targetObject.Height = sourceObject.Height;
        }
    }
}
