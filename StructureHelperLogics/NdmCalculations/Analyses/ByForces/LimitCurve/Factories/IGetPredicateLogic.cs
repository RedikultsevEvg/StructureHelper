using StructureHelperCommon.Models.Shapes;

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces.LimitCurve.Factories
{
    public interface IGetPredicateLogic
    {
        Predicate<IPoint2D> GetPredicate();
    }
}