using LoaderCalculator.Logics;
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
            resultFuncs.Add(new ResultFunc() { Name = "Elastic Strain", ResultFunction = stressLogic.GetElasticStrain });
            resultFuncs.Add(new ResultFunc() { Name = "Plastic Strain", ResultFunction = stressLogic.GetPlasticStrain });
            resultFuncs.Add(new ResultFunc() { Name = "Stress", ResultFunction = stressLogic.GetStress });
            return resultFuncs;
        }
    }
}
