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
    public class RebarTriangulationLogicOptions : ITriangulationLogicOptions
    {
        private IForceTupleServiceLogic forceTupleServiceLogic;
        private IForceTupleServiceLogic ForceTupleServiceLogic => forceTupleServiceLogic ??= new ForceTupleServiceLogic();
        public ITriangulationOptions TriangulationOptions { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public IPoint2D Center { get; }
        /// <inheritdoc />
        public double Area { get; }
        public StrainTuple InitialPrestrain { get; set; }
        public IHeadMaterial HeadMaterial { get; set; }
        public INdmPrimitive HostPrimitive { get; set; }
        public StrainTuple HistoryPrestrain { get; set; }


        /// <inheritdoc />
        public RebarTriangulationLogicOptions(RebarNdmPrimitive primitive)
        {
            Center = primitive.Center.Clone() as Point2D;
            Area = primitive.Area;
            HeadMaterial = primitive.NdmElement.HeadMaterial;
            HostPrimitive = primitive.HostPrimitive;
            InitialPrestrain = primitive.NdmElement.UsersPrestrain as StrainTuple;
            HistoryPrestrain = ForceTupleServiceLogic.SumTuples(primitive.NdmElement.HistoryPrestrain, primitive.NdmElement.AutoPrestrain) as StrainTuple;
        }
    }
}
