using StructureHelperCommon.Infrastructures.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Materials.Libraries
{
    public class FactorLogic : IFactorLogic
    {
        public List<IMaterialSafetyFactor> SafetyFactors { get; }
        public (double Compressive, double Tensile) GetTotalFactor(LimitStates limitState, CalcTerms calcTerm)
        {
            double compressionVal = 1d;
            double tensionVal = 1d;
            foreach (var item in SafetyFactors.Where(x => x.Take == true))
            {
                compressionVal *= item.GetFactor(StressStates.Compression, calcTerm, limitState);
                tensionVal *= item.GetFactor(StressStates.Tension, calcTerm, limitState);
            }
            return (compressionVal, tensionVal);
        }
        public FactorLogic(List<IMaterialSafetyFactor> safetyFactors)
        {
            SafetyFactors = safetyFactors;
        }
    }
}
