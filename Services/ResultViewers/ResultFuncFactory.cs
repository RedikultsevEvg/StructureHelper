using LoaderCalculator.Logics;
using StructureHelper.Infrastructure.UI.Converters.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelper.Services.ResultViewers
{
    public static class ResultFuncFactory
    {
        public static IEnumerable<IResultFunc> GetResultFuncs()
        {
            List<IResultFunc> resultFuncs = new List<IResultFunc>();
            IStressLogic stressLogic = new StressLogic();
            resultFuncs.Add(new ResultFunc() { Name = "Total Strain", ResultFunction = stressLogic.GetTotalStrain });
            resultFuncs.Add(new ResultFunc() { Name = "Total Strain with prestrain", ResultFunction = stressLogic.GetTotalStrainWithPresrain });
            resultFuncs.Add(new ResultFunc() { Name = "Elastic Strain", ResultFunction = stressLogic.GetElasticStrain });
            resultFuncs.Add(new ResultFunc() { Name = "Plastic Strain", ResultFunction = stressLogic.GetPlasticStrain });
            resultFuncs.Add(new ResultFunc() { Name = "Stress", ResultFunction = stressLogic.GetStress, UnitFactor = UnitConstatnts.Stress });
            resultFuncs.Add(new ResultFunc() { Name = "Secant modulus", ResultFunction = stressLogic.GetSecantModulus, UnitFactor = UnitConstatnts.Stress });
            resultFuncs.Add(new ResultFunc() { Name = "Modulus degradation", ResultFunction = stressLogic.GetModulusDegradation });
            resultFuncs.Add(new ResultFunc() { Name = "Force", ResultFunction = stressLogic.GetForce, UnitFactor = UnitConstatnts.Force });
            resultFuncs.Add(new ResultFunc() { Name = "Moment X", ResultFunction = stressLogic.GetMomentX, UnitFactor = UnitConstatnts.Force });
            resultFuncs.Add(new ResultFunc() { Name = "Moment Y", ResultFunction = stressLogic.GetMomentY, UnitFactor = UnitConstatnts.Force });
            return resultFuncs;
        }
    }
}
