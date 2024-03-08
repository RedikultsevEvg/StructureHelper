using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces
{
    /// <summary>
    /// Find parameter by point and predicate
    /// </summary>
    public interface ILimitCurveParameterLogic : IHasActionByResult, ICloneable
    {
        Predicate<Point2D> LimitPredicate { get; set; }
        IPoint2D CurrentPoint { get; set; }
        double GetParameter();
        IShiftTraceLogger? TraceLogger { get; set; }
    }
}
