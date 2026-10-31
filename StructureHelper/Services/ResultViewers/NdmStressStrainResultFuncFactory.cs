using LoaderCalculator.Logics;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Services.Units;
using System.Collections.Generic;

namespace StructureHelper.Services.ResultViewers
{
    public enum FuncsTypes
    {
        Strain,
        Stress,
        Forces,
        Full,
    }
    public static class NdmStressStrainResultFuncFactory
    {
        static IGetUnitLogic unitLogic = new GetUnitLogic();
        static IUnit unitForce = unitLogic.GetUnit(UnitTypes.Force);
        static IUnit unitStress = unitLogic.GetUnit(UnitTypes.Stress);
        static IUnit unitMoment = unitLogic.GetUnit(UnitTypes.Moment);
        static IUnit unitCurvature = unitLogic.GetUnit(UnitTypes.Curvature);
        static IUnit unitStrain = unitLogic.GetUnit(UnitTypes.Strain);

        static readonly IStressLogic stressLogic = new StressLogic();
        public static List<ResultFuncByStrainMatrix> GetResultFuncs(FuncsTypes funcsType = FuncsTypes.Full)
        {
            List<ResultFuncByStrainMatrix> results = new();
            if (funcsType == FuncsTypes.Strain)
            {
                results.AddRange(GetStrainResultFuncs());
            }
            else if (funcsType == FuncsTypes.Stress)
            {
                results.AddRange(GetStressResultFuncs());
            }
            else if (funcsType == FuncsTypes.Forces)
            {
                results.AddRange(GetForcesResultFuncs());
            }
            else if (funcsType == FuncsTypes.Full)
            {
                results.AddRange(GetStrainResultFuncs());
                results.AddRange(GetStressResultFuncs());
                results.AddRange(GetForcesResultFuncs());
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(funcsType));
            }
            return results;
        }
        private static List<ResultFuncByStrainMatrix> GetStrainResultFuncs()
        {
            List<ResultFuncByStrainMatrix> resultFuncs = [];
            resultFuncs.Add(new ResultFuncByStrainMatrix() { Name = "Section Strain", UnitName = unitStrain.Name, ResultFunction = stressLogic.GetSectionStrain });
            resultFuncs.Add(new ResultFuncByStrainMatrix() { Name = "Total Strain", UnitName = unitStrain.Name, ResultFunction = stressLogic.GetTotalStrain });
            resultFuncs.Add(new ResultFuncByStrainMatrix() { Name = "Prestrain", UnitName = unitStrain.Name, ResultFunction = stressLogic.GetPrestrain });
            resultFuncs.Add(new ResultFuncByStrainMatrix() { Name = "Elastic Strain", UnitName = unitStrain.Name, ResultFunction = stressLogic.GetElasticStrain });
            resultFuncs.Add(new ResultFuncByStrainMatrix() { Name = "Plastic Strain", UnitName = unitStrain.Name, ResultFunction = stressLogic.GetPlasticStrain });
            resultFuncs.Add(new ResultFuncByStrainMatrix() { Name = "Limit Strain Ratio", UnitName = unitStrain.Name, ResultFunction = stressLogic.GetLimitStrainRatio });
            return resultFuncs;
        }
        private static List<ResultFuncByStrainMatrix> GetStressResultFuncs()
        {
            List<ResultFuncByStrainMatrix> resultFuncs = [];
            resultFuncs.Add(new ResultFuncByStrainMatrix() { Name = "Stress", ResultFunction = stressLogic.GetStress, UnitFactor = unitStress.Multiplayer, UnitName = unitStress.Name });
            resultFuncs.Add(new ResultFuncByStrainMatrix() { Name = "Secant modulus", ResultFunction = stressLogic.GetSecantModulus, UnitFactor = unitStress.Multiplayer, UnitName = unitStress.Name });
            resultFuncs.Add(new ResultFuncByStrainMatrix() { Name = "Modulus degradation", ResultFunction = stressLogic.GetModulusDegradation });
            return resultFuncs;
        }
        private static List<ResultFuncByStrainMatrix> GetForcesResultFuncs()
        {
            List<ResultFuncByStrainMatrix> resultFuncs = [];
            resultFuncs.Add(new ResultFuncByStrainMatrix() { Name = "Force", ResultFunction = stressLogic.GetForce, UnitFactor = unitForce.Multiplayer, UnitName = unitForce.Name });
            resultFuncs.Add(new ResultFuncByStrainMatrix() { Name = "Moment X", ResultFunction = stressLogic.GetMomentX, UnitFactor = unitMoment.Multiplayer, UnitName = unitMoment.Name });
            resultFuncs.Add(new ResultFuncByStrainMatrix() { Name = "Moment Y", ResultFunction = stressLogic.GetMomentY, UnitFactor = unitMoment.Multiplayer, UnitName = unitMoment.Name });
            return resultFuncs;
        }
    }
}
