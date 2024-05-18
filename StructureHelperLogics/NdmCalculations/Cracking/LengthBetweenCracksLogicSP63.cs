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
    /// <summary>
    /// Logic for obtaining of length between cracks according to SP63.13330.2018
    /// </summary>
    public class LengthBetweenCracksLogicSP63 : ILengthBetweenCracksLogic
    {
        const double minDiameterFactor = 10d;
        const double maxDiameterFactor = 40d;
        const double minLength = 0.1d;
        const double maxLength = 0.4d;
        private const double areaFactor = 0.5d;
        readonly IAverageDiameterLogic diameterLogic;
        readonly ITensileConcreteAreaLogic concreteAreaLogic;
        ITensionRebarAreaLogic rebarAreaLogic;
        private IStressLogic stressLogic => new StressLogic();

        /// <inheritdoc/>
        public IEnumerable<INdm> NdmCollection { get; set; }
        /// <inheritdoc/>
        public IStrainMatrix StrainMatrix { get; set; }
        /// <inheritdoc/>
        public IShiftTraceLogger? TraceLogger { get; set; }

        public LengthBetweenCracksLogicSP63(IAverageDiameterLogic diameterLogic, ITensileConcreteAreaLogic concreteAreaLogic, ITensionRebarAreaLogic rebarAreaLogic)
        {
            this.diameterLogic = diameterLogic;
            this.concreteAreaLogic = concreteAreaLogic;
            this.rebarAreaLogic = rebarAreaLogic;
        }
        public LengthBetweenCracksLogicSP63() :
            this
            (   new EquivalentDiameterLogic(),
                new TensileConcreteAreaLogicSP63(),
                new TensionRebarAreaByStrainLogic())
        {      }
        /// <inheritdoc/>
        public double GetLength()
        {
            TraceLogger?.AddMessage(LoggerStrings.CalculatorType(this), TraceLogStatuses.Service);
            IEnumerable<RebarNdm?> rebars = GetRebars();
            double rebarArea = GetRebarArea(rebars);
            double rebarDiameter = GetAverageDiameter(rebars);
            double concreteArea = GetConcreteArea();
            double length = GetLengthBetweenCracks(rebarArea, rebarDiameter, concreteArea);
            return length;
        }

        private IEnumerable<RebarNdm?> GetRebars()
        {
            var rebars = NdmCollection
                .Where(x => x is RebarNdm)
                .Select(x => x as RebarNdm);
            if (!rebars.Any())
            {
                string errorString = ErrorStrings.DataIsInCorrect + ": Collection of rebars does not contain any tensile rebars";
                TraceLogger?.AddMessage(errorString, TraceLogStatuses.Error);
                throw new StructureHelperException(errorString);
            }

            return rebars;
        }

        private double GetLengthBetweenCracks(double rebarArea, double rebarDiameter, double concreteArea)
        {
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

        private double GetConcreteArea()
        {
            concreteAreaLogic.TraceLogger = TraceLogger?.GetSimilarTraceLogger(50);
            concreteAreaLogic.NdmCollection = NdmCollection;
            concreteAreaLogic.StrainMatrix = StrainMatrix;
            var concreteArea = concreteAreaLogic.GetTensileArea();
            TraceLogger?.AddMessage($"Concrete effective area Ac,eff = {concreteArea}(m^2)");
            return concreteArea;
        }

        private double GetAverageDiameter(IEnumerable<RebarNdm?> rebars)
        {
            var tesileRebars = rebars
                .Where(x => stressLogic.GetTotalStrain(StrainMatrix, x) > 0d);
            diameterLogic.TraceLogger = TraceLogger?.GetSimilarTraceLogger(50);
            diameterLogic.Rebars = tesileRebars;
            var rebarDiameter = diameterLogic.GetAverageDiameter();
            TraceLogger?.AddMessage($"Average rebar diameter ds = {rebarDiameter}");
            return rebarDiameter;
        }

        private double GetRebarArea(IEnumerable<RebarNdm?> rebars)
        {
            rebarAreaLogic.StrainMatrix = StrainMatrix;
            rebarAreaLogic.Rebars = rebars;
            rebarAreaLogic.TraceLogger = TraceLogger?.GetSimilarTraceLogger(50);
            var rebarArea = rebarAreaLogic.GetTensionRebarArea();
            TraceLogger?.AddMessage($"Summary rebar area As = {rebarArea}(m^2)");
            return rebarArea;
        }
    }
}
