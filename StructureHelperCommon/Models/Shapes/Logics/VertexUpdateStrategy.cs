using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Shapes
{
    public class VertexUpdateStrategy : IParentUpdateStrategy<IVertex>
    {
        public bool UpdateChildren { get; set; } = true;

        public void Update(IVertex targetObject, IVertex sourceObject)
        {
            CheckObject.ThrowIfNull(targetObject, sourceObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            if (UpdateChildren == true)
            {
                var newPoint = sourceObject.Point.Clone() as IPoint2D;
                targetObject.Point = newPoint;
            }
        }
    }
}
