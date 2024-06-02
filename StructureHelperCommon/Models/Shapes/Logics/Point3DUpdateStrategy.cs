using StructureHelperCommon.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Shapes.Logics
{
    /// <inheritdoc />
    public class Point3DUpdateStrategy : IUpdateStrategy<IPoint3D>
    {
        /// <inheritdoc />
        public void Update(IPoint3D targetObject, IPoint3D sourceObject)
        {
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            targetObject.X = sourceObject.X;
            targetObject.Y = sourceObject.Y;
            targetObject.Z = sourceObject.Z;
        }
    }
}
