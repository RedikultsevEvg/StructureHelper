using StructureHelper.Models.Materials;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Shapes;
using StructureHelperCommon.Services.Forces;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Triangulations
{
    public class CircleTriangulationLogicOptions : IShapeTriangulationLogicOptions
    {
        public ICircleShape Circle { get; }

        public IPoint2D Center { get; }

        public double NdmMaxSize { get; }

        public int NdmMinDivision { get; }

        public StrainTuple Prestrain { get; set; }
        public ITriangulationOptions triangulationOptions { get; set; }
        public IHeadMaterial HeadMaterial { get; set; }

        public CircleTriangulationLogicOptions(ICirclePrimitive primitive)
        {
            Center = primitive.Center.Clone() as Point2D;
            Circle = primitive;
            NdmMaxSize = primitive.NdmMaxSize;
            NdmMinDivision = primitive.NdmMinDivision;
            HeadMaterial = primitive.HeadMaterial;
            Prestrain = ForceTupleService.SumTuples(primitive.UsersPrestrain, primitive.AutoPrestrain) as StrainTuple;
        }
    }
}
