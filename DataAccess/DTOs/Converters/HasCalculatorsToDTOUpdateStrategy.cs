using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Calculators;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs.Converters
{
    public class HasCalculatorsToDTOUpdateStrategy : IUpdateStrategy<IHasCalculators>
    {
        private IConvertStrategy<ICalculator, ICalculator> convertStrategy;

        public Dictionary<(Guid id, Type type), ISaveable> ReferenceDictionary { get; set; }
        public IShiftTraceLogger TraceLogger { get; set; }

        public HasCalculatorsToDTOUpdateStrategy(IConvertStrategy<ICalculator, ICalculator> convertStrategy)
        {
            this.convertStrategy = convertStrategy;
        }

        public HasCalculatorsToDTOUpdateStrategy() : this(new CalculatorToDTOConvertStrategy())
        {
            
        }

        public void Update(IHasCalculators targetObject, IHasCalculators sourceObject)
        {
            if (sourceObject.Calculators is null)
            {
                throw new StructureHelperException(ErrorStrings.ParameterIsNull);
            }
            targetObject.Calculators.Clear();
            ProcessCalculators(targetObject, sourceObject);
        }

        private void ProcessCalculators(IHasCalculators targetObject, IHasCalculators sourceObject)
        {
            convertStrategy.ReferenceDictionary = ReferenceDictionary;
            convertStrategy.TraceLogger = TraceLogger;
            foreach (var item in sourceObject.Calculators)
            {
                ICalculator newItem = convertStrategy.Convert(item);
                targetObject.Calculators.Add(newItem);
            }
        }
    }
}
