using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.VisualProperties;
using StructureHelperCommon.Services;

namespace DataAccess.DTOs
{
    public class HasVisualPropertyToDTOUpdateStrategy : IUpdateStrategy<IHasVisualProperty>
    {
        private Dictionary<(Guid id, Type type), ISaveable> referenceDictionary;
        private IShiftTraceLogger traceLogger;
        private IConvertStrategy<PrimitiveVisualPropertyDTO, IPrimitiveVisualProperty> convertStrategy;


        public HasVisualPropertyToDTOUpdateStrategy(Dictionary<(Guid id, Type type), ISaveable> referenceDictionary, IShiftTraceLogger traceLogger)
        {
            this.referenceDictionary = referenceDictionary;
            this.traceLogger = traceLogger;
        }

        public void Update(IHasVisualProperty targetObject, IHasVisualProperty sourceObject)
        {
            CheckObject.ThrowIfNull(targetObject);
            CheckObject.ThrowIfNull(sourceObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            convertStrategy = new DictionaryConvertStrategy<PrimitiveVisualPropertyDTO, IPrimitiveVisualProperty>(
                referenceDictionary,
                traceLogger,
                new PrimitiveVisualPropertyToDTOConvertStrategy(referenceDictionary, traceLogger));
            targetObject.VisualProperty = convertStrategy.Convert(sourceObject.VisualProperty);
        }
    }
}
