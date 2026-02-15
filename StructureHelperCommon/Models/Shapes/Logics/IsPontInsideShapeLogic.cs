using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperCommon.Models.Shapes
{
    public interface IsPontInsideShapeLogic
    {
        double Gap { get; set; }
        bool IsPontInside(IPoint2D point, ICenterShape centerShape);
    }
}
