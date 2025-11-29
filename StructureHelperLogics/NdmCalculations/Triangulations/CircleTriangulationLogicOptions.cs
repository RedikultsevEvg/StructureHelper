using StructureHelper.Models.Materials;
using StructureHelperCommon.Infrastructures.Interfaces;
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
        private IForceTupleServiceLogic forceTupleServiceLogic;
        private IForceTupleServiceLogic ForceTupleServiceLogic => forceTupleServiceLogic ??= new ForceTupleServiceLogic();
        public ICircleShape Circle { get;}

        public IPoint2D Center { get; set; }

        public StrainTuple Prestrain { get; set; }
        public ITriangulationOptions TriangulationOptions { get; set; }
        public IHeadMaterial HeadMaterial { get; set; }
        public double RotationAngle { get; set; }

        public IDivisionSize DivisionSize { get; }

        public CircleTriangulationLogicOptions(IEllipseNdmPrimitive primitive)
        {
            Center = primitive.Center.Clone() as Point2D;
            //to do change to ellipse
            Circle = new CircleShape() { Diameter = primitive.Width };
            DivisionSize = primitive.DivisionSize;
            HeadMaterial = primitive.NdmElement.HeadMaterial;
            Prestrain = ForceTupleServiceLogic.SumTuples(primitive.NdmElement.UsersPrestrain, primitive.NdmElement.AutoPrestrain) as StrainTuple;
        }
    }
}
