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

        public CircleTriangulationLogicOptions(IEllipsePrimitive primitive)
        {
            Center = primitive.Center.Clone() as Point2D;
            //to do change to ellipse
            Circle = new CircleShape() { Diameter = primitive.DiameterByX };
            NdmMaxSize = primitive.DivisionSize.NdmMaxSize;
            NdmMinDivision = primitive.DivisionSize.NdmMinDivision;
            HeadMaterial = primitive.NdmElement.HeadMaterial;
            Prestrain = ForceTupleService.SumTuples(primitive.NdmElement.UsersPrestrain, primitive.NdmElement.AutoPrestrain) as StrainTuple;
        }
    }
}
