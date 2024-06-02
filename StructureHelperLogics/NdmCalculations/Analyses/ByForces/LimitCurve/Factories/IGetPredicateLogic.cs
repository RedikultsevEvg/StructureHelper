using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Shapes;

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces.LimitCurve.Factories
{
    public interface IGetPredicateLogic : ILogic, ICloneable
    {
        string Name { get; set; }
        Predicate<IPoint2D> GetPredicate();
    }
}