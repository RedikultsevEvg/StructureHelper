using System;
using System.Collections.Generic;
using System.Text;
using TriangleNet.Meshing;

namespace StructureHelperCommon.Models.Shapes
{
    public interface IMeshShapeLogic
    {
        ICenterShape CenterShape { get; set; }
        double MinimumAngleInDegree { get; set; }
        double MaximumMeshSize { get; set; }
        IMesh Triangulate();
    }
}
