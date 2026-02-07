using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperCommon.Models.Shapes
{
    public class EllipseShapeUpdateStrategy : IUpdateStrategy<IEllipseShape>
    {
        public void Update(IEllipseShape targetObject, IEllipseShape sourceObject)
        {
            CheckObject.ThrowIfNull(sourceObject);
            CheckObject.ThrowIfNull(targetObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            targetObject.Width = sourceObject.Width;
            targetObject.Height = sourceObject.Height;
        }
    }
}
