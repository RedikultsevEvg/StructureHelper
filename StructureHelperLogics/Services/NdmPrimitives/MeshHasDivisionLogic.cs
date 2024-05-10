using LoaderCalculator.Data.Ndms;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Loggers;
using StructureHelperCommon.Models.Shapes;
using StructureHelperLogics.Models.Primitives;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Services.NdmPrimitives
{
    public class MeshHasDivisionLogic : IMeshHasDivisionLogic
    {
        public List<INdm> NdmCollection { get; set; }
        public IHasDivisionSize Primitive { get; set; }
        public IShiftTraceLogger? TraceLogger { get; set; }

        public void MeshHasDivision()
        {
            TraceLogger?.AddMessage(LoggerStrings.CalculatorType(this), TraceLogStatuses.Service);
            if (Primitive is IHasDivisionSize hasDivision)
            {
                if (hasDivision.ClearUnderlying == true)
                {
                    TraceLogger?.AddMessage("Removing of background part has started", TraceLogStatuses.Service);
                    NdmCollection
                        .RemoveAll(x =>
                        hasDivision
                            .IsPointInside(new Point2D()
                            {
                                X = x.CenterX,
                                Y = x.CenterY
                            }) == true);
                }
            }
        }
    }
}
