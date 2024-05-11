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
    public class LengthBetweenCracksLogicSP63 : ILengthBetweenCracksLogic
    {
        const double minDiameterFactor = 10d;
        const double maxDiameterFactor = 40d;
        const double minLength = 0.1d;
        const double maxLength = 0.4d;
        private const double areaFactor = 0.5d;
        readonly IAverageDiameterLogic diameterLogic;
        readonly ITensileAreaLogic tensileAreaLogic;
        IStressLogic stressLogic;
        public IEnumerable<INdm> NdmCollection { get; set; }
        public IStrainMatrix StrainMatrix { get; set; }
        public IShiftTraceLogger? TraceLogger { get; set; }

        public LengthBetweenCracksLogicSP63(IAverageDiameterLogic diameterLogic, ITensileAreaLogic tensileAreaLogic)
        {
            this.diameterLogic = diameterLogic;
            this.tensileAreaLogic = tensileAreaLogic;
            stressLogic = new StressLogic();
        }
        public LengthBetweenCracksLogicSP63() :
            this
            (   new EquivalentDiameterLogic(),
                new TensileAreaLogicSP63())
        {      }
        public double GetLength()
        {
            TraceLogger?.AddMessage(LoggerStrings.CalculatorType(this), TraceLogStatuses.Service);
            var rebars = NdmCollection
                .Where(x => x is RebarNdm & stressLogic.GetTotalStrain(StrainMatrix, x) > 0d)
                .Select(x => x as RebarNdm);
            if (! rebars.Any())
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
            TraceLogger?.AddMessage($"Summary rebar area As = {rebarArea}");
            diameterLogic.TraceLogger = TraceLogger?.GetSimilarTraceLogger(50);
            diameterLogic.Rebars = rebars;
            var rebarDiameter = diameterLogic.GetAverageDiameter();
            TraceLogger?.AddMessage($"Average rebar diameter ds = {rebarDiameter}");
            tensileAreaLogic.TraceLogger = TraceLogger?.GetSimilarTraceLogger(50);
            tensileAreaLogic.NdmCollection = NdmCollection;
            tensileAreaLogic.StrainMatrix = StrainMatrix;
            var concreteArea = tensileAreaLogic.GetTensileArea();
            TraceLogger?.AddMessage($"Concrete effective area Ac,eff = {concreteArea}(m^2)");
            var length = areaFactor * concreteArea / rebarArea * rebarDiameter;
            TraceLogger?.AddMessage($"Base length between cracks Lcrc = {areaFactor} * {concreteArea} / {rebarArea} * {rebarDiameter} = {length}(m)");
            double minLengthByDiameter = minDiameterFactor * rebarDiameter;
            TraceLogger?.AddMessage($"Minimum length by diameter Lcrc = {minDiameterFactor} * {rebarDiameter} = {minLengthByDiameter}(m)");
            TraceLogger?.AddMessage($"Minimum absolute length Lcrc = {minLength}(m)");
            var restrictedByMinLength = new List<double> { length, minLengthByDiameter, minLength }.Max();
            TraceLogger?.AddMessage($"Consider minimum length restriction Lcrc = max({length}(m), {minLengthByDiameter}(m), {minLength}(m)) = {length}(m)");
            double maxLengthByDiameter = maxDiameterFactor * rebarDiameter;
            TraceLogger?.AddMessage($"Maximum length by diameter Lcrc = {maxDiameterFactor} * {rebarDiameter} = {maxLengthByDiameter}(m)");
            TraceLogger?.AddMessage($"Maximum absolute length Lcrc = {maxLength}(m)");
            var restrictedByMaxLength = new List<double> { restrictedByMinLength, maxLengthByDiameter, maxLength }.Min();
            TraceLogger?.AddMessage($"Consider maximum length restriction Lcrc = max({restrictedByMinLength}(m), {maxLengthByDiameter}(m), {maxLength}(m)) = {restrictedByMaxLength}(m)");
            length = restrictedByMaxLength;
            TraceLogger?.AddMessage($"Finally Lcrc = {length}(m)");
            return length;
        }
    }
}
