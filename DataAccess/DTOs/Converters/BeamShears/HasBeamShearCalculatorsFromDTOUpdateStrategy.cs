using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Services;
using StructureHelperLogics.Models.BeamShears;

namespace DataAccess.DTOs
{
    internal class HasBeamShearCalculatorsFromDTOUpdateStrategy : IUpdateStrategy<IHasCalculators>
    {
        private Dictionary<(Guid id, Type type), ISaveable> referenceDictionary;
        private IShiftTraceLogger traceLogger;

        public HasBeamShearCalculatorsFromDTOUpdateStrategy(Dictionary<(Guid id, Type type), ISaveable> referenceDictionary, IShiftTraceLogger traceLogger)
        {
            this.referenceDictionary = referenceDictionary;
            this.traceLogger = traceLogger;
        }

        public void Update(IHasCalculators targetObject, IHasCalculators sourceObject)
        {
            CheckObject.ThrowIfNull(targetObject);
            CheckObject.ThrowIfNull(sourceObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            CheckObject.ThrowIfNull(sourceObject.Calculators);
            CheckObject.ThrowIfNull(targetObject.Calculators);
            targetObject.Calculators.Clear();
            List<ICalculator> calculators = GetCalculators(sourceObject.Calculators);
            targetObject.Calculators.AddRange(calculators);
        }

        private List<ICalculator> GetCalculators(IEnumerable<ICalculator> sourceCalculators)
        {
            List<ICalculator> calculators = new();
            foreach (var calculator in sourceCalculators)
            {
                ICalculator newCalculator = ProcessCalculator(calculator);
                calculators.Add(newCalculator);
            }
            return calculators;
        }

        private ICalculator ProcessCalculator(ICalculator calculator)
        {
            if (calculator is BeamShearCalculatorDTO shearCalculator)
            {
                return ProcessShearCalculator(shearCalculator);
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(calculator));
            }
        }

        private ICalculator ProcessShearCalculator(BeamShearCalculatorDTO shearCalculator)
        {
            traceLogger?.AddMessage("Calcultor is beam shear calculator", TraceLogStatuses.Debug);
            var convertStrategy = new DictionaryConvertStrategy<BeamShearCalculator, BeamShearCalculatorDTO>
                (referenceDictionary, traceLogger, new BeamShearCalculatorFromDTOConvertStrategy(referenceDictionary, traceLogger));
            return convertStrategy.Convert(shearCalculator);
        }
    }
}
