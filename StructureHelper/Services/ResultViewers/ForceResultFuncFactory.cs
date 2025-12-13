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
    public static class ForceResultFuncFactory
    {
        static IGetUnitLogic unitLogic = new GetUnitLogic();
        static IUnit unitForce = unitLogic.GetUnit(UnitTypes.Force);
        static IUnit unitStress = unitLogic.GetUnit(UnitTypes.Stress);
        static IUnit unitMoment = unitLogic.GetUnit(UnitTypes.Moment);
        static IUnit unitCurvature = unitLogic.GetUnit(UnitTypes.Curvature);
        static IUnit unitStrain = unitLogic.GetUnit(UnitTypes.Strain);

        static readonly IStressLogic stressLogic = new StressLogic();
        public static List<ForceResultFunc> GetResultFuncs(FuncsTypes funcsType = FuncsTypes.Full)
        {
            List<ForceResultFunc> results = new();
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
        private static List<ForceResultFunc> GetStrainResultFuncs()
        {
            List<ForceResultFunc> resultFuncs = [];
            resultFuncs.Add(new ForceResultFunc() { Name = "Section Strain", UnitName = unitStrain.Name, ResultFunction = stressLogic.GetSectionStrain });
            resultFuncs.Add(new ForceResultFunc() { Name = "Total Strain", UnitName = unitStrain.Name, ResultFunction = stressLogic.GetTotalStrain });
            resultFuncs.Add(new ForceResultFunc() { Name = "Prestrain", UnitName = unitStrain.Name, ResultFunction = stressLogic.GetPrestrain });
            resultFuncs.Add(new ForceResultFunc() { Name = "Elastic Strain", UnitName = unitStrain.Name, ResultFunction = stressLogic.GetElasticStrain });
            resultFuncs.Add(new ForceResultFunc() { Name = "Plastic Strain", UnitName = unitStrain.Name, ResultFunction = stressLogic.GetPlasticStrain });
            resultFuncs.Add(new ForceResultFunc() { Name = "Limit Strain Ratio", UnitName = unitStrain.Name, ResultFunction = stressLogic.GetLimitStrainRatio });
            return resultFuncs;
        }
        private static List<ForceResultFunc> GetStressResultFuncs()
        {
            List<ForceResultFunc> resultFuncs = [];
            resultFuncs.Add(new ForceResultFunc() { Name = "Stress", ResultFunction = stressLogic.GetStress, UnitFactor = unitStress.Multiplyer, UnitName = unitStress.Name });
            resultFuncs.Add(new ForceResultFunc() { Name = "Secant modulus", ResultFunction = stressLogic.GetSecantModulus, UnitFactor = unitStress.Multiplyer, UnitName = unitStress.Name });
            resultFuncs.Add(new ForceResultFunc() { Name = "Modulus degradation", ResultFunction = stressLogic.GetModulusDegradation });
            return resultFuncs;
        }
        private static List<ForceResultFunc> GetForcesResultFuncs()
        {
            List<ForceResultFunc> resultFuncs = [];
            resultFuncs.Add(new ForceResultFunc() { Name = "Force", ResultFunction = stressLogic.GetForce, UnitFactor = unitForce.Multiplyer, UnitName = unitForce.Name });
            resultFuncs.Add(new ForceResultFunc() { Name = "Moment X", ResultFunction = stressLogic.GetMomentX, UnitFactor = unitMoment.Multiplyer, UnitName = unitMoment.Name });
            resultFuncs.Add(new ForceResultFunc() { Name = "Moment Y", ResultFunction = stressLogic.GetMomentY, UnitFactor = unitMoment.Multiplyer, UnitName = unitMoment.Name });
            return resultFuncs;
        }
    }
}
