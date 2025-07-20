using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Calculators;

namespace StructureHelperLogics.Models.BeamShears
{
    internal class StirrupBySearchLogic : IBeamShearStrenghLogic
    {
        private ConcreteStrengthLogic concreteLogic;
        private StirrupStrengthLogic stirrupLogic;
        public IShiftTraceLogger? TraceLogger { get; set; }
        public IBeamShearSectionLogicInputData InputData { get; internal set; }
        public ISectionEffectiveness SectionEffectiveness { get; internal set; }
        public StirrupBySearchLogic(IShiftTraceLogger? traceLogger)
        {
            TraceLogger = traceLogger;
        }

        public double GetShearStrength()
        {
            double parameter = GetInclinedCrackRatio();
            BeamShearSectionLogicInputData newInputData = GetNewInputDataByCrackLengthRatio(parameter);
            TraceLogger?.AddMessage($"New value of dangerous inclinated crack has been obtained: start point Xstart = {newInputData.InclinedSection.StartCoord}(m), end point Xend = {newInputData.InclinedSection.EndCoord}(m)");
            stirrupLogic = new(newInputData, TraceLogger);
            double stirrupStrength = stirrupLogic.GetShearStrength();
            return stirrupStrength;
        }

        /// <summary>
        /// Calculates ratio of inclined crack length to inclined section length
        /// </summary>
        /// <returns></returns>
        /// <exception cref="StructureHelperException"></exception>
        private double GetInclinedCrackRatio()
        {
            var parameterCalculator = new FindParameterCalculator();
            parameterCalculator.InputData.Predicate = GetPredicate;
            parameterCalculator.Accuracy.IterationAccuracy = 0.0001d;
            parameterCalculator.Accuracy.MaxIterationCount = 1000;
            parameterCalculator.Run();
            if (parameterCalculator.Result.IsValid == false)
            {
                throw new StructureHelperException(ErrorStrings.DataIsInCorrect + $": predicate error");
            }
            var result = parameterCalculator.Result as FindParameterResult;
            var crackLengthRatio = result.Parameter;
            while (GetPredicate(crackLengthRatio) == false)
            {
                crackLengthRatio += 0.0001;
            }
            return crackLengthRatio;
        }
        /// <summary>
        /// Predicate of comparing of stirrup strength and concrete strength
        /// </summary>
        /// <param name="crackLengthRatio">Ratio of inclined crack length and inclined section length</param>
        /// <returns>Is true when stirrup strength is greate than concrete strength</returns>
        private bool GetPredicate(double crackLengthRatio)
        {
            BeamShearSectionLogicInputData newInputData = GetNewInputDataByCrackLengthRatio(crackLengthRatio);
            concreteLogic = new(SectionEffectiveness, newInputData.InclinedSection, null);
            stirrupLogic = new(newInputData, null);
            double concreteStrength = concreteLogic.GetShearStrength();
            double stirrupStrength = stirrupLogic.GetShearStrength();
            return stirrupStrength > concreteStrength;
        }

        private BeamShearSectionLogicInputData GetNewInputDataByCrackLengthRatio(double crackLengthRatio)
        {
            double sourceCrackLength = InputData.InclinedSection.EndCoord - InputData.InclinedSection.StartCoord;
            double newCrackLength = sourceCrackLength * crackLengthRatio;
            double newStartCoord = InputData.InclinedSection.EndCoord - newCrackLength;
            InclinedSection newSection = new();
            var updateStrategy = new InclinedSectionUpdateStrategy();
            updateStrategy.Update(newSection, InputData.InclinedSection);
            newSection.StartCoord = newStartCoord;
            BeamShearSectionLogicInputData newInputData = new(Guid.Empty)
            {
                InclinedSection = newSection,
                LimitState = InputData.LimitState,
                CalcTerm = InputData.CalcTerm,
                Stirrup = InputData.Stirrup,
                ForceTuple = InputData.ForceTuple
            };
            return newInputData;
        }
    }
}
