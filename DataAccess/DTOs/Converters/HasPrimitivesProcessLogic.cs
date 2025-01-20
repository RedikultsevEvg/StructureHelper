using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperLogics.NdmCalculations.Primitives;

namespace DataAccess.DTOs.Converters
{
    public class HasPrimitivesProcessLogic : IHasPrimitivesProcessLogic
    {
        private const string convertStarted = " converting has been started";
        private const string convertFinished = " converting has been finished successfully";
        private ConvertDirection convertDirection;

        public HasPrimitivesProcessLogic(ConvertDirection convertDirection)
        {
            this.convertDirection = convertDirection;
        }

        public IHasPrimitives Source { get; set; }
        public IHasPrimitives Target { get; set; }
        public Dictionary<(Guid id, Type type), ISaveable> ReferenceDictionary { get; set; }
        public IShiftTraceLogger TraceLogger { get; set; }

        public void Process()
        {
            TraceLogger?.AddMessage("Primitives" + convertStarted);
            IUpdateStrategy<IHasPrimitives> updateStrategy = GetUpdateStrategyFactory();
            updateStrategy.Update(Target, Source);
            TraceLogger?.AddMessage($"Primitives {convertFinished}, totally {Target.Primitives.Count} have been obtained");
        }

        private IUpdateStrategy<IHasPrimitives> GetUpdateStrategyFactory()
        {
            if (convertDirection == ConvertDirection.FromDTO)
            {
                NdmPrimitiveFromDTOConvertStrategy convertStrategy = new()
                {
                    ReferenceDictionary = ReferenceDictionary,
                    TraceLogger = TraceLogger
                };
                return GetUpdateStrategy(convertStrategy);
            }
            else if (convertDirection == ConvertDirection.ToDTO)
            {
                NdmPrimitiveToDTOConvertStrategy convertStrategy = new()
                {
                    ReferenceDictionary = ReferenceDictionary,
                    TraceLogger = TraceLogger
                };
                return GetUpdateStrategy(convertStrategy);
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(convertDirection));
            }
        }

        private IUpdateStrategy<IHasPrimitives> GetUpdateStrategy(IConvertStrategy<INdmPrimitive, INdmPrimitive> convertStrategy)
        {

            DictionaryConvertStrategy<INdmPrimitive, INdmPrimitive> convertLogic = new()
            {
                ReferenceDictionary = ReferenceDictionary,
                TraceLogger = TraceLogger,
                ConvertStrategy = convertStrategy
            };
            HasPrimitivesFromDTOUpdateStrategy updateStrategy = new(convertLogic);
            return updateStrategy;
        }
    }
}
