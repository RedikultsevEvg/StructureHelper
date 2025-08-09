using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Calculators;

namespace StructureHelperLogics.Models.BeamShears
{
    internal class StirrupBySearchLogic : IBeamShearStrenghLogic
    {
        private ConcreteStrengthLogic concreteLogic;
        private StirrupStrengthLogic stirrupLogic;
        private IFindParameterCalculator parameterCalculator;

        public IShiftTraceLogger? TraceLogger { get; set; }
        public IInclinedSection InclinedCrack { get; private set; }
        public IBeamShearSectionLogicInputData InputData { get; internal set; }
        public ISectionEffectiveness SectionEffectiveness { get; internal set; }
        public StirrupBySearchLogic(IShiftTraceLogger? traceLogger)
        {
            TraceLogger = traceLogger;
        }

        public double GetShearStrength()
        {
            double parameter = GetCrackLengthRatio();
            BeamShearSectionLogicInputData newInputData = GetNewInputDataByCrackLengthRatio(parameter);
            InclinedCrack = newInputData.InclinedCrack;
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
        private double GetCrackLengthRatio()
        {
            if (GetPredicate(1) == false)
            {
                return 1;
            }
            parameterCalculator = new FindParameterCalculator();
            parameterCalculator.InputData.Predicate = GetPredicate;
            parameterCalculator.Accuracy.IterationAccuracy = 0.0001d;
            parameterCalculator.Accuracy.MaxIterationCount = 1000;
            parameterCalculator.Run();
            if (parameterCalculator.Result.IsValid == false)
            {
                throw new StructureHelperException(ErrorStrings.DataIsInCorrect + $": Binary search error {parameterCalculator.Result.Description}");
            }
            var result = parameterCalculator.Result as FindParameterResult;
            var crackLengthRatio = result.Parameter;
            while (GetPredicate(crackLengthRatio) == false)
            {
                crackLengthRatio += 0.0001;
            }
            crackLengthRatio = Math.Max(crackLengthRatio, 0);
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
            concreteLogic = new(SectionEffectiveness, newInputData.InclinedCrack, null);
            stirrupLogic = new(newInputData, null);
            double concreteStrength = concreteLogic.GetShearStrength();
            double stirrupStrength = stirrupLogic.GetShearStrength();
            bool predicateResult = stirrupStrength > concreteStrength;
            if (crackLengthRatio == 1 & predicateResult == false)
            {

            }
            return predicateResult;
        }

        private BeamShearSectionLogicInputData GetNewInputDataByCrackLengthRatio(double crackLengthRatio)
        {
            IInclinedSection inclinedSection = InputData.InclinedCrack;
            double sourceCrackLength = inclinedSection.EndCoord - inclinedSection.StartCoord;
            double newCrackLength = sourceCrackLength * crackLengthRatio;
            double newStartCoord = inclinedSection.EndCoord - newCrackLength;
            InclinedSection newSection = new();
            var updateStrategy = new InclinedSectionUpdateStrategy();
            updateStrategy.Update(newSection, inclinedSection);
            newSection.StartCoord = newStartCoord;
            BeamShearSectionLogicInputData newInputData = new(Guid.Empty)
            {
                InclinedSection = newSection,
                InclinedCrack = newSection,
                LimitState = InputData.LimitState,
                CalcTerm = InputData.CalcTerm,
                Stirrup = InputData.Stirrup,
                ForceTuple = InputData.ForceTuple
            };
            return newInputData;
        }
    }
}
