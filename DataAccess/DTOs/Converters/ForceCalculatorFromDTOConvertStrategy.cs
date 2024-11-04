using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs
{
    public class ForceCalculatorFromDTOConvertStrategy : ConvertStrategy<ForceCalculator, ForceCalculatorDTO>
    {
        
        private IConvertStrategy<ForceCalculatorInputData, ForceCalculatorInputDataDTO> inputDataConvertStrategy = new ForceCalculatorInputDataFromDTOConvertStrategy();
        public override ForceCalculator GetNewItem(ForceCalculatorDTO source)
        {
            NewItem = new(source.Id);
            NewItem.Name = source.Name;
            inputDataConvertStrategy.ReferenceDictionary = ReferenceDictionary;
            inputDataConvertStrategy.TraceLogger = TraceLogger;
            NewItem.InputData = inputDataConvertStrategy.Convert(source.InputData as ForceCalculatorInputDataDTO);
            return NewItem;
        }
    }
}
