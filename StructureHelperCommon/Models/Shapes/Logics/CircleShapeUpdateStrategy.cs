using StructureHelperCommon.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Shapes
{
    public class CircleShapeUpdateStrategy : IUpdateStrategy<ICircleShape>
    {
        public void Update(ICircleShape targetObject, ICircleShape sourceObject)
        {
            targetObject.Diameter = sourceObject.Diameter;
        }
    }
}
