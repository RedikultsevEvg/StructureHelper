using StructureHelperCommon.Models.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces.LimitCurve
{
    public static class ConvertLogics
    {
        private static readonly List<ConstOneDirectionConverter> converterLogics;
        public static List<ConstOneDirectionConverter> ConverterLogics => converterLogics ??
            new List<ConstOneDirectionConverter>()
            {
                ConvertLogicFactory.GetLogic(ForceCurveLogic.N_Mx),
                ConvertLogicFactory.GetLogic(ForceCurveLogic.N_My),
                ConvertLogicFactory.GetLogic(ForceCurveLogic.Mx_My),
            };
    }
}
