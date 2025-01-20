using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StructureHelperCommon.Infrastructures.Exceptions;

namespace DataAccess.DTOs
{
    public class HasForceActionsProcessLogic : IHasForceActionsProcessLogic
    {
        private const string convertStarted = " converting is started";
        private const string convertFinished = " converting has been finished successfully";
        private IConvertStrategy<IForceAction, IForceAction> convertStrategy;
        private DictionaryConvertStrategy<IForceAction, IForceAction> convertLogic;
        private ConvertDirection convertDirection;

        public HasForceActionsProcessLogic(ConvertDirection convertDirection)
        {
            this.convertDirection = convertDirection;
        }

        public IShiftTraceLogger TraceLogger { get; set; }
        public Dictionary<(Guid id, Type type), ISaveable> ReferenceDictionary { get; set; }

        public IHasForceActions Source { get; set; }
        public IHasForceActions Target { get; set; }
        public void Process()
        {
            TraceLogger?.AddMessage("Actions" + convertStarted);
            HasForceActionsFromDTOUpdateStrategy updateStrategy = GetUpdateStrategyFactory();
            updateStrategy.Update(Target, Source);
            TraceLogger?.AddMessage("Actions" + convertFinished);
        }

        private HasForceActionsFromDTOUpdateStrategy GetUpdateStrategyFactory()
        {
            if (convertDirection == ConvertDirection.FromDTO)
            {
                convertStrategy ??= new ForceActionFromDTOConvertStrategy()
                {
                    ReferenceDictionary = ReferenceDictionary,
                    TraceLogger = TraceLogger
                };
            }
            else if (convertDirection == ConvertDirection.ToDTO)
            {
                convertStrategy ??= new ForceActionToDTOConvertStrategy()
                {
                    ReferenceDictionary = ReferenceDictionary,
                    TraceLogger = TraceLogger
                };
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(convertDirection));
            }
            convertLogic ??= new()
            {
                ReferenceDictionary = ReferenceDictionary,
                TraceLogger = TraceLogger,
                ConvertStrategy = convertStrategy
            };
            HasForceActionsFromDTOUpdateStrategy updateStrategy = new(convertLogic);
            return updateStrategy;
        }
    }
}
