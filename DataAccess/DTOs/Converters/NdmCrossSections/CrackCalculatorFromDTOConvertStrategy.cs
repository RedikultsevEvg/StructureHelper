using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using StructureHelperLogics.NdmCalculations.Cracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs
{
    public class CrackCalculatorFromDTOConvertStrategy : ConvertStrategy<CrackCalculator, CrackCalculatorDTO>
    {
        private IConvertStrategy<CrackCalculatorInputData, CrackCalculatorInputDataDTO> convertStrategy = new CrackCalculatorInputDataFromDTOConvertStrategy();

        public override CrackCalculator GetNewItem(CrackCalculatorDTO source)
        {
            NewItem = new(source.Id);
            NewItem.Name = source.Name;
            convertStrategy.ReferenceDictionary = ReferenceDictionary;
            convertStrategy.TraceLogger = TraceLogger;
            NewItem.InputData = convertStrategy.Convert(source.InputData as CrackCalculatorInputDataDTO);
            return NewItem;
        }
    }
}
