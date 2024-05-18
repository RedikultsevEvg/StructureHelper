using LoaderCalculator.Data.Matrix;
using LoaderCalculator.Data.Ndms;
using LoaderCalculator.Logics;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Loggers;
using StructureHelperLogics.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    public class TensionRebarAreaSimpleSumLogic : ITensionRebarAreaLogic
    {
        IStressLogic stressLogic;
        public IStrainMatrix StrainMatrix { get; set; }
        public IEnumerable<RebarNdm> Rebars { get; set; }
        public IShiftTraceLogger? TraceLogger { get; set; }
        public TensionRebarAreaSimpleSumLogic(IStressLogic stressLogic)
        {
            this.stressLogic = stressLogic;
        }
        public TensionRebarAreaSimpleSumLogic() : this(new StressLogic())
        {
            
        }
        public double GetTensionRebarArea()
        {
            TraceLogger?.AddMessage(LoggerStrings.CalculatorType(this), TraceLogStatuses.Service);
            TraceLogger?.AddMessage("Method of obtaining of summary area of rebars in tension based on ordinary summarizing of areas");
            var rebars = Rebars
                .Where(x => stressLogic.GetTotalStrain(StrainMatrix, x) > 0d);
            if (!rebars.Any())
            {
                string errorString = ErrorStrings.DataIsInCorrect + ": Collection of rebars does not contain any tensile rebars";
                TraceLogger?.AddMessage(errorString, TraceLogStatuses.Error);
                throw new StructureHelperException(errorString);
            }
            if (TraceLogger is not null)
            {
                TraceService.TraceNdmCollection(TraceLogger, rebars);
            }
            var rebarArea = rebars.Sum(x => x.Area * x.StressScale);
            return rebarArea;
        }
    }
}
