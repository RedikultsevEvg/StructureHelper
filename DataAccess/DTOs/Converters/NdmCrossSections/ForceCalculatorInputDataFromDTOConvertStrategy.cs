using DataAccess.DTOs.Converters;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Calculators;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using StructureHelperLogics.NdmCalculations.Primitives;

namespace DataAccess.DTOs
{
    public class ForceCalculatorInputDataFromDTOConvertStrategy : ConvertStrategy<ForceCalculatorInputData, ForceCalculatorInputDataDTO>
    {
        private IUpdateStrategy<IForceCalculatorInputData> updateStrategy;
        private IHasPrimitivesProcessLogic primitivesProcessLogic;
        private IHasForceActionsProcessLogic actionsProcessLogic;
        private IConvertStrategy<Accuracy, AccuracyDTO> accuracyConvertStrategy;

        public override ForceCalculatorInputData GetNewItem(ForceCalculatorInputDataDTO source)
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

        private void GetNewItemBySource(ForceCalculatorInputDataDTO source)
        {
            NewItem = new(source.Id);
            updateStrategy.Update(NewItem, source);
            NewItem.Accuracy = accuracyConvertStrategy.Convert((AccuracyDTO)source.Accuracy);
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
            primitivesProcessLogic ??= new HasPrimitivesProcessLogic(ConvertDirection.FromDTO) { ReferenceDictionary = ReferenceDictionary, TraceLogger = TraceLogger };
            actionsProcessLogic ??= new HasForceActionsProcessLogic(ConvertDirection.FromDTO) { ReferenceDictionary = ReferenceDictionary, TraceLogger = TraceLogger };
            accuracyConvertStrategy ??= new AccuracyFromDTOConvertStrategy() { ReferenceDictionary = ReferenceDictionary, TraceLogger = TraceLogger };
        }
    }
}
