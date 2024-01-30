using StructureHelperCommon.Models.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.Arrays
{
    public interface IGeometryArrayMember
    {
        IPoint2D Center { get; set; }
        double RotationAngle { get; set; }
    }
}
