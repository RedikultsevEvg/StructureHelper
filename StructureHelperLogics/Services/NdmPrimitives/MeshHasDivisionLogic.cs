using LoaderCalculator.Data.Ndms;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
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
    /// <inheritdoc/>
    public class MeshHasDivisionLogic : IMeshHasDivisionLogic
    {
        /// <inheritdoc/>
        public List<INdm>? NdmCollection { get; set; }
        /// <inheritdoc/>
        public IHasDivisionSize? Primitive { get; set; }
        /// <inheritdoc/>
        public IShiftTraceLogger? TraceLogger { get; set; }

        public void MeshHasDivision()
        {
            TraceLogger?.AddMessage(LoggerStrings.CalculatorType(this), TraceLogStatuses.Service);
            CheckInputData();
            if (Primitive is IHasDivisionSize hasDivision)
            {
                if (hasDivision.DivisionSize.ClearUnderlying == true)
                {
                    TraceLogger?.AddMessage("Removing of background part has started", TraceLogStatuses.Service);
                    NdmCollection.RemoveAll(x => IsCenterInside(x, hasDivision) == true);
                }
            }
        }

        private static bool IsCenterInside(INdm x, IHasDivisionSize hasDivision)
        {
            Point2D point = new Point2D()
            {
                X = x.CenterX,
                Y = x.CenterY
            };
            return hasDivision.IsPointInside(point);
        }

        private void CheckInputData()
        {
            if (NdmCollection is null)
            {
                var message = ErrorStrings.ParameterIsNull + ": input NdmCollection";
                TraceLogger?.AddMessage(message, TraceLogStatuses.Error);
                throw new StructureHelperException(message);
            }
            if (Primitive is null)
            {
                var message = ErrorStrings.ParameterIsNull + ": input Primitive";
                TraceLogger?.AddMessage(message, TraceLogStatuses.Error);
                throw new StructureHelperException(message);
            }

        }
    }
}
