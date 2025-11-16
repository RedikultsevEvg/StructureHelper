using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.VisualProperties;
using StructureHelperCommon.Services;

namespace DataAccess.DTOs
{
    public class HasVisualPropertyFromDTOUpdateStrategy : IUpdateStrategy<IHasVisualProperty>
    {
        private Dictionary<(Guid id, Type type), ISaveable> referenceDictionary;
        private IShiftTraceLogger traceLogger;
        private IConvertStrategy<PrimitiveVisualProperty, PrimitiveVisualPropertyDTO> convertStrategy;

        public HasVisualPropertyFromDTOUpdateStrategy(Dictionary<(Guid id, Type type), ISaveable> referenceDictionary, IShiftTraceLogger traceLogger)
        {
            this.referenceDictionary = referenceDictionary;
            this.traceLogger = traceLogger;
        }

        public void Update(IHasVisualProperty targetObject, IHasVisualProperty sourceObject)
        {
            CheckObject.ThrowIfNull(targetObject);
            CheckObject.ThrowIfNull(sourceObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            convertStrategy = new DictionaryConvertStrategy<PrimitiveVisualProperty, PrimitiveVisualPropertyDTO>(
                referenceDictionary,
                traceLogger,
                new PrimitiveVisualPropertyFromDTOConvertStrategy(referenceDictionary, traceLogger));
            if (sourceObject.VisualProperty is not PrimitiveVisualPropertyDTO visualProperty)
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(sourceObject.VisualProperty));
            }
            targetObject.VisualProperty = convertStrategy.Convert(visualProperty);
        }
    }
}
