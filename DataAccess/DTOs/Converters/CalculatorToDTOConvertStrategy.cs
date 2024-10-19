using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Loggers;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using StructureHelperLogics.NdmCalculations.Cracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs
{
    public class CalculatorToDTOConvertStrategy : IConvertStrategy<ICalculator, ICalculator>
    {
        private readonly IConvertStrategy<ForceCalculatorDTO, IForceCalculator> forceCalculatorStrategy;
        private readonly IConvertStrategy<CrackCalculatorDTO, ICrackCalculator> crackCalculatorStrategy;



        public Dictionary<(Guid id, Type type), ISaveable> ReferenceDictionary { get; set; }
        public IShiftTraceLogger TraceLogger { get; set; }
        public CalculatorToDTOConvertStrategy(
            IConvertStrategy<ForceCalculatorDTO, IForceCalculator> forceCalculatorStrategy,
            IConvertStrategy<CrackCalculatorDTO, ICrackCalculator> crackCalculatorStrategy)
        {
            this.forceCalculatorStrategy = forceCalculatorStrategy;
            this.crackCalculatorStrategy = crackCalculatorStrategy;
        }

        public CalculatorToDTOConvertStrategy() : this (
            new ForceCalculatorToDTOConvertStrategy(),
            new CrackCalculatorToDTOConvertStrategy())
        {
            
        }

        public ICalculator Convert(ICalculator source)
        {
            try
            {
                return ProcessCalculators(source);
            }
            catch (Exception ex)
            {
                TraceLogger?.AddMessage(LoggerStrings.LogicType(this), TraceLogStatuses.Error);
                TraceLogger?.AddMessage(ex.Message, TraceLogStatuses.Error);
                throw;
            }
        }

        private ICalculator ProcessCalculators(ICalculator source)
        {
            if (source is IForceCalculator forceCalculator)
            {
                return ProcessForceCalculator(forceCalculator);
            }
            if (source is ICrackCalculator crackCalculator)
            {
                return ProcessCrackCalculator(crackCalculator);
            }
            string errorString = ErrorStrings.ObjectTypeIsUnknownObj(source);
            TraceLogger.AddMessage(errorString, TraceLogStatuses.Error);
            throw new StructureHelperException(errorString);
        }

        private CrackCalculatorDTO ProcessCrackCalculator(ICrackCalculator crackCalculator)
        {
            crackCalculatorStrategy.ReferenceDictionary = ReferenceDictionary;
            crackCalculatorStrategy.TraceLogger = TraceLogger;
            var logic = new DictionaryConvertStrategy<CrackCalculatorDTO, ICrackCalculator>(this, crackCalculatorStrategy);
            return logic.Convert(crackCalculator);
        }

        private ForceCalculatorDTO ProcessForceCalculator(IForceCalculator forceCalculator)
        {
            forceCalculatorStrategy.ReferenceDictionary = ReferenceDictionary;
            forceCalculatorStrategy.TraceLogger = TraceLogger;
            var logic = new DictionaryConvertStrategy<ForceCalculatorDTO, IForceCalculator>(this, forceCalculatorStrategy);
            return logic.Convert(forceCalculator);
        }
    }
}
