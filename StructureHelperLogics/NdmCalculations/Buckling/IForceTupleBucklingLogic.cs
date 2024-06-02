using LoaderCalculator.Data.Ndms;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;

namespace StructureHelperLogics.NdmCalculations.Buckling
{
    public interface IForceTupleBucklingLogic : ILogic
    {
        GenericResult<IForceTuple> GetForceTupleByBuckling();
    }
}