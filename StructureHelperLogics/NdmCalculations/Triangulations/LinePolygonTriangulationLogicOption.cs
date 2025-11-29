using StructureHelper.Models.Materials;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Shapes;
using StructureHelperCommon.Services.Forces;
using StructureHelperLogics.Models.Primitives;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Triangulations
{
    public class LinePolygonTriangulationLogicOption : IShapeTriangulationLogicOptions
    {
        private IForceTupleServiceLogic forceTupleServiceLogic;
        private IForceTupleServiceLogic ForceTupleServiceLogic => forceTupleServiceLogic ??= new ForceTupleServiceLogic();
        public IPoint2D Center { get; set; }
        public IDivisionSize DivisionSize { get; set; }
        public ITriangulationOptions TriangulationOptions { get; set; }
        public StrainTuple Prestrain { get; set; }
        public IHeadMaterial HeadMaterial { get; set; }
        public double RotationAngle { get; set; } = 0;
        public IShape Shape { get; set; }
        public LinePolygonTriangulationLogicOption(IShapeNdmPrimitive primitive, ITriangulationOptions triangulationOptions)
        {
            Center = primitive.Center;
            DivisionSize = primitive.DivisionSize;
            TriangulationOptions = triangulationOptions;
            Shape = primitive.Shape;
            HeadMaterial = primitive.NdmElement.HeadMaterial;
            Prestrain = ForceTupleServiceLogic.SumTuples(primitive.NdmElement.UsersPrestrain, primitive.NdmElement.AutoPrestrain) as StrainTuple;
        }
    }
}
