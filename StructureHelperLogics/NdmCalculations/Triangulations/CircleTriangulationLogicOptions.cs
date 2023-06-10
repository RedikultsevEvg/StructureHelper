using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Shapes;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Triangulations
{
    public class CircleTriangulationLogicOptions : ICircleTriangulationLogicOptions
    {
        public ICircleShape Circle { get; }

        public IPoint2D Center { get; }

        public double NdmMaxSize { get; }

        public int NdmMinDivision { get; }

        public StrainTuple Prestrain { get; set; }

        public CircleTriangulationLogicOptions(ICirclePrimitive primitive)
        {
            Center = new Point2D() { X = primitive.CenterX, Y = primitive.CenterY };
            Circle = primitive;
            NdmMaxSize = primitive.NdmMaxSize;
            NdmMinDivision = primitive.NdmMinDivision;
            Prestrain = new StrainTuple
            {
                Mx = primitive.UsersPrestrain.Mx + primitive.AutoPrestrain.Mx,
                My = primitive.UsersPrestrain.My + primitive.AutoPrestrain.My,
                Nz = primitive.UsersPrestrain.Nz + primitive.AutoPrestrain.Nz
            };
        }
    }
}
