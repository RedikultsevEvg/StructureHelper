using DataAccess.DTOs.Converters;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Calculators;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs
{
    public class ForceCalculatorInputDataToDTOConvertStrategy : ConvertStrategy<ForceCalculatorInputDataDTO, IForceCalculatorInputData>
    {
        private IUpdateStrategy<IForceCalculatorInputData> updateStrategy;
        private IHasPrimitivesProcessLogic primitivesProcessLogic;
        private IHasForceActionsProcessLogic actionsProcessLogic;
        private IConvertStrategy<AccuracyDTO, IAccuracy> accuracyConvertStrategy;

        public override ForceCalculatorInputDataDTO GetNewItem(IForceCalculatorInputData source)
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

        private void GetNewItemBySource(IForceCalculatorInputData source)
        {
            NewItem = new(source.Id);
            updateStrategy.Update(NewItem, source);
            NewItem.Accuracy = accuracyConvertStrategy.Convert(source.Accuracy);
            ProcessPrimitives(source);
            ProcessActions(source);
        }

        private void ProcessPrimitives(IHasPrimitives source)
        {
            primitivesProcessLogic.Source = source;
            primitivesProcessLogic.Target = NewItem;
            primitivesProcessLogic.ReferenceDictionary = ReferenceDictionary;
            primitivesProcessLogic.TraceLogger = TraceLogger;
            primitivesProcessLogic.Process();
        }
        private void ProcessActions(IHasForceActions source)
        {
            actionsProcessLogic.Source = source;
            actionsProcessLogic.Target = NewItem;
            actionsProcessLogic.ReferenceDictionary = ReferenceDictionary;
            actionsProcessLogic.TraceLogger = TraceLogger;
            actionsProcessLogic.Process();
        }
        private void InitializeStrategies()
        {
            updateStrategy ??= new ForceCalculatorInputDataUpdateStrategy();
            primitivesProcessLogic ??= new HasPrimitivesProcessLogic(ConvertDirection.ToDTO) { ReferenceDictionary = ReferenceDictionary, TraceLogger = TraceLogger };
            actionsProcessLogic ??= new HasForceActionsProcessLogic(ConvertDirection.ToDTO) { ReferenceDictionary = ReferenceDictionary, TraceLogger = TraceLogger };
            accuracyConvertStrategy ??= new AccuracyToDTOConvertStrategy() { ReferenceDictionary = ReferenceDictionary, TraceLogger = TraceLogger };
        }
    }
}
