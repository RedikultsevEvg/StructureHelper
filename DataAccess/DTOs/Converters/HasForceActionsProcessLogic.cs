using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs
{
    public class HasForceActionsProcessLogic : IHasForceActionsProcessLogic
    {
        private const string convertStarted = " converting is started";
        private const string convertFinished = " converting has been finished successfully";

        public IShiftTraceLogger TraceLogger { get; set; }
        public Dictionary<(Guid id, Type type), ISaveable> ReferenceDictionary { get; set; }

        public IHasForceActions Source { get; set; }
        public IHasForceActions Target { get; set; }
        public void Process()
        {
            TraceLogger?.AddMessage("Actions" + convertStarted);
            ForceActionsFromDTOConvertStrategy convertStrategy = new()
            {
                ReferenceDictionary = ReferenceDictionary,
                TraceLogger = TraceLogger
            };
            DictionaryConvertStrategy<IForceAction, IForceAction> convertLogic = new()
            {
                ReferenceDictionary = ReferenceDictionary,
                TraceLogger = TraceLogger,
                ConvertStrategy = convertStrategy
            };
            HasForceActionsFromDTOUpdateStrategy updateStrategy = new(convertLogic);
            updateStrategy.Update(Target, Source);
            TraceLogger?.AddMessage("Actions" + convertFinished);
        }
    }
}
