using LoaderCalculator.Data.Ndms;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces.LimitCurve.Factories
{
    public class GetPredicateLogic : IGetPredicateLogic
    {
        public IEnumerable<INdm> Ndms { get; set; }
        public PredicateTypes PredicateType { get; set; }
        public IConvert2DPointTo3DPointLogic ConvertLogic { get; set; }

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
