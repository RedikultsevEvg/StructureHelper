using StructureHelperCommon.Infrastructures.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Materials.Libraries
{
    public interface IFactorLogic
    {
        List<IMaterialSafetyFactor> SafetyFactors { get; }
        (double Compressive, double Tensile) GetTotalFactor(LimitStates limitState, CalcTerms calcTerm);
    }
}
