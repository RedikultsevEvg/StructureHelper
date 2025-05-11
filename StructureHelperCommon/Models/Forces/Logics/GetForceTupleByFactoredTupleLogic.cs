using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Services.Forces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Forces.Logics
{
    public class GetForceTupleByFactoredTupleLogic : IGetForceTupleByFactoredTupleLogic
    {
        private IGetLoadFactor getFactorLogic;

        public GetForceTupleByFactoredTupleLogic(IGetLoadFactor getFactorLogic)
        {
            this.getFactorLogic = getFactorLogic;
        }

        public GetForceTupleByFactoredTupleLogic()
        {
            
        }

        public IFactoredForceTuple FactoredForceTuple { get; set; }
        public LimitStates LimitState { get; set; }
        public CalcTerms CalcTerm { get; set; }

        public IForceTuple GetForceTuple()
        {
            getFactorLogic ??= new GetFactorByFactoredCombinationProperty()
            {
                CombinationProperty = FactoredForceTuple.CombinationProperty,
                LimitState = LimitState,
                CalcTerm = CalcTerm
            };
            double factor = getFactorLogic.GetFactor();
            return ForceTupleService.MultiplyTupleByFactor(FactoredForceTuple.ForceTuple, factor);
        }
    }
}
