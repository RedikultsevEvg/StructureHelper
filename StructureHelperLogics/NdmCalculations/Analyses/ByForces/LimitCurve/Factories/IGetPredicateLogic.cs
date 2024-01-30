using StructureHelperCommon.Models.Loggers;
using StructureHelperCommon.Models.Shapes;

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces.LimitCurve.Factories
{
    public interface IGetPredicateLogic
    {
        string Name { get; set; }
        Predicate<IPoint2D> GetPredicate();
        ITraceLogger? TraceLogger { get; set; }
    }
}