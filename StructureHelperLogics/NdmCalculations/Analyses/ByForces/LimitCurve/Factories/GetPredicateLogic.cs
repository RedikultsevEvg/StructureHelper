using LoaderCalculator.Data.Ndms;
using StructureHelperCommon.Infrastructures.Interfaces;
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
        public IShiftTraceLogger? TraceLogger { get; set; }

        public object Clone()
        {
            throw new NotImplementedException();
        }

        public Predicate<IPoint2D> GetPredicate()
        {
            var factory = new PredicateFactory()
            {
                Ndms = Ndms,
                ConvertLogic = ConvertLogic
            };
            if (TraceLogger is not null)
            {
                factory.TraceLogger = TraceLogger;
            }
            TraceLogger?.AddMessage($"Predicate factory was obtained succsefully", TraceLogStatuses.Debug);
            var predicateType = PredicateType;
            var predicate = factory.GetPredicate(predicateType);
            return predicate;
        }
    }
}
