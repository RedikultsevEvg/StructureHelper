using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Calculators;
using StructureHelperLogics.Models.BeamShears.Logics;

namespace StructureHelperLogics.Models.BeamShears
{
    internal class StirrupBySearchLogic : IBeamShearStrenghLogic
    {
        private ConcreteStrengthLogic concreteLogic;
        private StirrupStrengthLogic stirrupLogic;
        private ShiftTraceLogger? localTraceLogger { get; set; }

        public StirrupBySearchLogic(IShiftTraceLogger? traceLogger)
        {
            TraceLogger = traceLogger;
        }

        public IShiftTraceLogger? TraceLogger { get; set; }
        public IBeamShearSectionLogicInputData InputData { get; internal set; }
        public ISectionEffectiveness SectionEffectiveness { get; internal set; }

        public double GetShearStrength()
        {
            double parameter = GetParameter();
            BeamShearSectionLogicInputData newInputData = GetNewInputData(parameter);
            stirrupLogic = new(newInputData, localTraceLogger);
            double stirrupStrength = stirrupLogic.GetShearStrength();
            return stirrupStrength;
        }

        public double GetParameter()
        {
            var parameterCalculator = new FindParameterCalculator(TraceLogger);
            parameterCalculator.InputData.Predicate = GetPredicate;
            parameterCalculator.Accuracy.IterationAccuracy = 0.0001d;
            parameterCalculator.Accuracy.MaxIterationCount = 1000;
            parameterCalculator.Run();
            if (parameterCalculator.Result.IsValid == false)
            {
                throw new StructureHelperException(ErrorStrings.DataIsInCorrect + $": predicate error");
            }
            var result = parameterCalculator.Result as FindParameterResult;
            var parameter = result.Parameter;
            return parameter;
        }

        private bool GetPredicate(double factor)
        {
            BeamShearSectionLogicInputData newInputData = GetNewInputData(factor);
            concreteLogic = new(SectionEffectiveness, newInputData.InclinedSection, localTraceLogger);
            stirrupLogic = new(newInputData, localTraceLogger);
            double concreteStrength = concreteLogic.GetShearStrength();
            double stirrupStrength = stirrupLogic.GetShearStrength();
            return stirrupStrength > concreteStrength;
        }

        private BeamShearSectionLogicInputData GetNewInputData(double factor)
        {
            double sourceCrackLength = InputData.InclinedSection.EndCoord - InputData.InclinedSection.StartCoord;
            double newCrackLength = sourceCrackLength * factor;
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
