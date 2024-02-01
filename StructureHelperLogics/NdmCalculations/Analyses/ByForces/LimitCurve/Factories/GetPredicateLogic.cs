using LoaderCalculator.Data.Ndms;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Shapes;

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces.LimitCurve.Factories
{
    public class GetPredicateLogic : IGetPredicateLogic
    {
        public IEnumerable<INdm> Ndms { get; set; }
        public PredicateTypes PredicateType { get; set; }
        public IConvert2DPointTo3DPointLogic ConvertLogic { get; set; }
        public string Name { get; set; }
        public ITraceLogger? TraceLogger { get; set; }

        public Predicate<IPoint2D> GetPredicate()
        {
            var factory = new PredicateFactory()
            {
                Ndms = Ndms,
                ConvertLogic = ConvertLogic
            };
            var predicateType = PredicateType;
            var predicate = factory.GetPredicate(predicateType);
            return predicate;
        }
    }
}
