using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs.Converters
{
    public class HasPrimitivesProcessLogic : IHasPrimitivesProcessLogic
    {
        private const string convertStarted = " converting is started";
        private const string convertFinished = " converting has been finished successfully";

        public IHasPrimitives Source { get; set; }
        public IHasPrimitives Target { get; set; }
        public Dictionary<(Guid id, Type type), ISaveable> ReferenceDictionary { get; set; }
        public IShiftTraceLogger TraceLogger { get; set; }

        public void Process()
        {
            TraceLogger?.AddMessage("Primitives" + convertStarted);
            NdmPrimitiveFromDTOConvertStrategy convertStrategy = new()
            {
                ReferenceDictionary = ReferenceDictionary,
                TraceLogger = TraceLogger
            };
            DictionaryConvertStrategy<INdmPrimitive, INdmPrimitive> convertLogic = new()
            {
                ReferenceDictionary = ReferenceDictionary,
                TraceLogger = TraceLogger,
                ConvertStrategy = convertStrategy
            };
            HasPrimitivesFromDTOUpdateStrategy updateStrategy = new(convertLogic);
            updateStrategy.Update(Target, Source);
            TraceLogger?.AddMessage($"Primitives {convertFinished}, totally {Target.Primitives.Count} have been obtained");
        }
    }
}
