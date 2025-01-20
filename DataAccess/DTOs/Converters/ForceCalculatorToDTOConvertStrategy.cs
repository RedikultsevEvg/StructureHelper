using DataAccess.DTOs.Converters;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Loggers;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces.Logics;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs
{
    public class ForceCalculatorToDTOConvertStrategy : ConvertStrategy<ForceCalculatorDTO, IForceCalculator>
    {
        private IUpdateStrategy<IForceCalculator> updateStrategy;
        private IConvertStrategy<ForceCalculatorInputDataDTO, IForceCalculatorInputData> inputDataConvertStrategy;

        public override ForceCalculatorDTO GetNewItem(IForceCalculator source)
        {
            InitializeStrategies();
            try
            {
                GetNewItemBySource(source);
                return NewItem;
            }
            catch (Exception ex)
            {
                TraceErrorByEntity(this, ex.Message);
                throw;
            }
        }

        private void InitializeStrategies()
        {
            updateStrategy ??= new ForceCalculatorUpdateStrategy();
            inputDataConvertStrategy ??= new ForceCalculatorInputDataToDTOConvertStrategy() { ReferenceDictionary = ReferenceDictionary, TraceLogger = TraceLogger};
        }

        private void GetNewItemBySource(IForceCalculator source)
        {
            NewItem = new() { Id = source.Id};
            updateStrategy.Update(NewItem, source);
            NewItem.InputData = inputDataConvertStrategy.Convert(source.InputData);
        }
    }
}
