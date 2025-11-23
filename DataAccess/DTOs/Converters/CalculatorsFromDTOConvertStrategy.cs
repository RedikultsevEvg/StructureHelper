using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Calculators;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using StructureHelperLogics.NdmCalculations.Analyses.Curvatures;
using StructureHelperLogics.NdmCalculations.Analyses.ValueDiagrams;
using StructureHelperLogics.NdmCalculations.Cracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs
{
    public class CalculatorsFromDTOConvertStrategy : ConvertStrategy<ICalculator, ICalculator>
    {
        private const string CalculatorIs = "Calculator is";
        private IConvertStrategy<ForceCalculator, ForceCalculatorDTO> forceConvertStrategy = new ForceCalculatorFromDTOConvertStrategy();
        private IConvertStrategy<CrackCalculator, CrackCalculatorDTO> crackConvertStrategy = new CrackCalculatorFromDTOConvertStrategy();

        public override ICalculator GetNewItem(ICalculator source)
        {
            NewItem = GetNewCalculator(source);
            TraceLogger?.AddMessage($"Calculator Id = {NewItem.Id}, Name = {NewItem.Name} has been obtained");
            return NewItem;
        }

        private ICalculator GetNewCalculator(ICalculator source)
        {
            if (source is IForceCalculator forceCalculator)
            {
                TraceLogger?.AddMessage($"{CalculatorIs} force calculator");
                IForceCalculator calculator = GetForcCalculator(forceCalculator);
                return calculator;
            }
            if (source is ICrackCalculator crackCalculator)
            {
                TraceLogger?.AddMessage($"{CalculatorIs} crack calculator");
                ICrackCalculator calculator = GetCrackCalculator(crackCalculator);
                return calculator;
            }
            if (source is ValueDiagramCalculatorDTO valueDiagramCalculator)
            {
                TraceLogger?.AddMessage($"{CalculatorIs} value digram calculator");
                return GetValueDiagramCalculator(valueDiagramCalculator);
            }
            if (source is CurvatureCalculatorDTO curvatureCalculator)
            {
                TraceLogger?.AddMessage($"{CalculatorIs} curvature calculator");
                return GetCurvatureCalculator(curvatureCalculator);
            }
            string errorString = ErrorStrings.ObjectTypeIsUnknownObj(source);
            TraceLogger.AddMessage(errorString, TraceLogStatuses.Error);
            throw new StructureHelperException(errorString);
        }

        private CurvatureCalculator GetCurvatureCalculator(CurvatureCalculatorDTO calculator)
        {
            var convertStrategy = new CurvatureCalculatorFromDTOConvertStrategy()
            {
                ReferenceDictionary = ReferenceDictionary,
                TraceLogger = TraceLogger,
            };
            return convertStrategy.Convert(calculator);
        }

        private ValueDiagramCalculator GetValueDiagramCalculator(ValueDiagramCalculatorDTO valueDiagramCalculator)
        {
            var convertStrategy = new ValueDiagramCalcualtorFromDTOConvertStrategy()
            {
                ReferenceDictionary = ReferenceDictionary,
                TraceLogger = TraceLogger,
            };
            return convertStrategy.Convert(valueDiagramCalculator);
        }

        private ICrackCalculator GetCrackCalculator(ICrackCalculator crackCalculator)
        {
            crackConvertStrategy.ReferenceDictionary = ReferenceDictionary;
            crackConvertStrategy.TraceLogger = TraceLogger;
            CrackCalculator newItem = crackConvertStrategy.Convert(crackCalculator as CrackCalculatorDTO);
            return newItem;
        }

        private IForceCalculator GetForcCalculator(IForceCalculator forceCalculator)
        {
            forceConvertStrategy.ReferenceDictionary = ReferenceDictionary;
            forceConvertStrategy.TraceLogger = TraceLogger;
            ForceCalculator newItem = forceConvertStrategy.Convert(forceCalculator as ForceCalculatorDTO);
            return newItem;
        }
    }
}
