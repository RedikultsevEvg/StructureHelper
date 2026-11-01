using LoaderCalculator;
using LoaderCalculator.Data.Ndms;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Services.Units;
using System.Collections.Generic;

namespace StructureHelper.Services.ResultViewers
{
    internal static class NdmPrestrainFuncFactory
    {
        private static IGetUnitLogic unitLogic = new GetUnitLogic();
        private static IUnit unitStrain = unitLogic.GetUnit(UnitTypes.Strain);
        private static readonly GetNdmPrestrainLogic prestrainLogic = new GetNdmPrestrainLogic();
        public static List<ResultFuncByNdm> GetResultFuncs()
        {
            List<ResultFuncByNdm> resultFuncs = [];
            resultFuncs.Add(new ResultFuncByNdm() { Name = "Total prestrain", UnitName = unitStrain.Name, ResultFunction = prestrainLogic.GetPrestrainValueAtCenter});
            resultFuncs.Add(new ResultFuncByNdm() { Name = "Initial prestrain", UnitName = unitStrain.Name, ResultFunction = GetInitialPrestrain});
            resultFuncs.Add(new ResultFuncByNdm() { Name = "History prestrain", UnitName = unitStrain.Name, ResultFunction = GetHistoryPrestrain});
            resultFuncs.Add(new ResultFuncByNdm() { Name = "Thermal prestrain", UnitName = unitStrain.Name, ResultFunction = GetThermalPrestrain});
            return resultFuncs;
        }

        private static double GetThermalPrestrain(INdm ndm)
        {
            return prestrainLogic.GetPrestrainValueAtCenter(ndm, PrestrainTypes.Thermal);
        }

        private static double GetHistoryPrestrain(INdm ndm)
        {
            return prestrainLogic.GetPrestrainValueAtCenter(ndm, PrestrainTypes.History);
        }

        private static double GetInitialPrestrain(INdm ndm)
        {
            return prestrainLogic.GetPrestrainValueAtCenter(ndm, PrestrainTypes.Initial);
        }
    }
}
