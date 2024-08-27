using LoaderCalculator.Data.Matrix;
using LoaderCalculator.Data.Ndms;
using LoaderCalculator.Logics;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.Loggers;
using StructureHelperCommon.Models;
using StructureHelperLogics.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    public class TensionRebarAreaByStrainLogic : ITensionRebarAreaLogic
    {
        IStressLogic stressLogic;
        public IStrainMatrix StrainMatrix { get; set; }
        public IEnumerable<RebarNdm> Rebars { get; set; }
        public IShiftTraceLogger? TraceLogger { get; set; }
        public TensionRebarAreaByStrainLogic(IStressLogic stressLogic)
        {
            this.stressLogic = stressLogic;
        }
        public TensionRebarAreaByStrainLogic() : this(new StressLogic())
        {
            
        }
        public double GetTensionRebarArea()
        {
            TraceLogger?.AddMessage(LoggerStrings.CalculatorType(this), TraceLogStatuses.Debug);
            TraceLogger?.AddMessage("Method of obtaining of summary area of rebars in tension based on areas which are proportional by maximum strain");
            var rebars = Rebars
                .Where(x => stressLogic.GetSectionStrain(StrainMatrix, x) > 0d);
            if (!rebars.Any())
            {
                string errorString = ErrorStrings.DataIsInCorrect + ": Collection of rebars does not contain any tensile rebars";
                TraceLogger?.AddMessage(errorString, TraceLogStatuses.Error);
                throw new StructureHelperException(errorString);
            }
            var maxStrain = rebars
                .Select(x => stressLogic.GetSectionStrain(StrainMatrix, x))
                .Max();
            TraceLogger?.AddMessage($"Maximum strain maxStrain = {maxStrain}");
            if (TraceLogger is not null)
            {
                TraceService.TraceNdmCollection(TraceLogger, rebars);
            }
            double sumArea = 0d;
            foreach (var rebar in rebars)
            {
                double area = rebar.Area * rebar.StressScale;
                double strain = stressLogic.GetSectionStrain(StrainMatrix, rebar);
                TraceLogger?.AddMessage($"Rebar area = {area}(m^2)");
                TraceLogger?.AddMessage($"Rebar strain = {strain}");
                var reducedArea = area * strain / maxStrain;
                TraceLogger?.AddMessage($"Reduced rebar area = area * strain / maxStrain = {area} * {strain} / {maxStrain} = {reducedArea}(m^2)");
                sumArea += reducedArea;
            }
            return sumArea;
        }
    }
}
