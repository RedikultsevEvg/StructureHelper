using LoaderCalculator.Logics;
using StructureHelper.Infrastructure.UI.Converters.Units;
using StructureHelperCommon.Infrastructures.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelper.Services.ResultViewers
{
    public enum FuncsTypes
    {
        Strain,
        Stress,
        Forces,
        Full,
    }
    public static class ResultFuncFactory
    {
        static readonly IStressLogic stressLogic = new StressLogic();
        public static List<IResultFunc> GetResultFuncs(FuncsTypes funcsType = FuncsTypes.Full)
        {
            List<IResultFunc> results = new();
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
        private static List<IResultFunc> GetStrainResultFuncs()
        {
            List<IResultFunc> resultFuncs = new List<IResultFunc>();
            resultFuncs.Add(new ResultFunc() { Name = "Total Strain", ResultFunction = stressLogic.GetTotalStrain });
            resultFuncs.Add(new ResultFunc() { Name = "Total Strain with prestrain", ResultFunction = stressLogic.GetTotalStrainWithPresrain });
            resultFuncs.Add(new ResultFunc() { Name = "Elastic Strain", ResultFunction = stressLogic.GetElasticStrain });
            resultFuncs.Add(new ResultFunc() { Name = "Plastic Strain", ResultFunction = stressLogic.GetPlasticStrain });
            return resultFuncs;
        }
        private static List<IResultFunc> GetStressResultFuncs()
        {
            List<IResultFunc> resultFuncs = new List<IResultFunc>();
            resultFuncs.Add(new ResultFunc() { Name = "Stress", ResultFunction = stressLogic.GetStress, UnitFactor = UnitConstants.Stress });
            resultFuncs.Add(new ResultFunc() { Name = "Secant modulus", ResultFunction = stressLogic.GetSecantModulus, UnitFactor = UnitConstants.Stress });
            resultFuncs.Add(new ResultFunc() { Name = "Modulus degradation", ResultFunction = stressLogic.GetModulusDegradation });
            return resultFuncs;
        }
        private static List<IResultFunc> GetForcesResultFuncs()
        {
            List<IResultFunc> resultFuncs = new List<IResultFunc>();
            resultFuncs.Add(new ResultFunc() { Name = "Force", ResultFunction = stressLogic.GetForce, UnitFactor = UnitConstants.Force });
            resultFuncs.Add(new ResultFunc() { Name = "Moment X", ResultFunction = stressLogic.GetMomentX, UnitFactor = UnitConstants.Force });
            resultFuncs.Add(new ResultFunc() { Name = "Moment Y", ResultFunction = stressLogic.GetMomentY, UnitFactor = UnitConstants.Force });
            return resultFuncs;
        }
    }
}
