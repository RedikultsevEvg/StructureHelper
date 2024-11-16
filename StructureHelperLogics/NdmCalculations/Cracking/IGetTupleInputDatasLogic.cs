using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.NdmCalculations.Primitives;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    /// <summary>
    /// Logic for preparing tuple data for crack calculator
    /// </summary>
    public interface IGetTupleInputDatasLogic : ILogic, IHasPrimitives, IHasForceActions
    {
        LimitStates LimitState { get; set; }
        CalcTerms LongTerm { get; set; }
        CalcTerms ShortTerm { get; set; }
        IUserCrackInputData UserCrackInputData { get; set; }
        /// <summary>
        /// Returns collection of tuples for crack calculations
        /// </summary>
        /// <returns></returns>
        List<TupleCrackInputData> GetTupleInputDatas();
    }
}