using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Services;
using StructureHelperLogics.Models.BeamShears;

namespace DataAccess.DTOs
{
    internal class BeamShearCalculatorFromDTOConvertStrategy : ConvertStrategy<BeamShearCalculator, BeamShearCalculatorDTO>
    {
        private IUpdateStrategy<IBeamShearCalculator> updateStrategy;
        private IConvertStrategy<BeamShearCalculatorInputData, BeamShearCalculatorInputDataDTO> inputDataConvertStrategy;

        public BeamShearCalculatorFromDTOConvertStrategy(Dictionary<(Guid id, Type type), ISaveable> referenceDictionary, IShiftTraceLogger traceLogger)
            : base(referenceDictionary, traceLogger)
        {
        }

        public override BeamShearCalculator GetNewItem(BeamShearCalculatorDTO source)
        {
            CheckObject.IsNull(source);
            CheckObject.IsNull(source.InputData);
            GetNewCalculator(source);
            return NewItem;
        }

        private void GetNewCalculator(BeamShearCalculatorDTO source)
        {
            if (source.InputData is not BeamShearCalculatorInputDataDTO inputDataDTO)
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(source.InputData));
            }
            InitializeStrategies();
            NewItem = new(source.Id);
            updateStrategy.Update(NewItem, source);
            NewItem.InputData = inputDataConvertStrategy.Convert(inputDataDTO);
        }

        private void InitializeStrategies()
        {
            updateStrategy ??= new BeamShearCalculatorUpdateStrategy();
            inputDataConvertStrategy = new DictionaryConvertStrategy<BeamShearCalculatorInputData, BeamShearCalculatorInputDataDTO>
                (this, new BeamShearCalculatorInputDataFromDTOConvertStrategy(this));
        }
    }
}
