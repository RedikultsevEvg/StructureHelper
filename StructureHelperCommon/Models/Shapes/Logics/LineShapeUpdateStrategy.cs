using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Shapes.Logics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Shapes
{
    public class LineShapeUpdateStrategy : IUpdateStrategy<ILineShape>
    {
        readonly Point2DUpdateStrategy pointUpdateStrategy = new();
        public void Update(ILineShape targetObject, ILineShape sourceObject)
        {
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            pointUpdateStrategy.Update(targetObject.StartPoint, sourceObject.StartPoint);
            pointUpdateStrategy.Update(targetObject.EndPoint, sourceObject.EndPoint);
        }
    }
}
