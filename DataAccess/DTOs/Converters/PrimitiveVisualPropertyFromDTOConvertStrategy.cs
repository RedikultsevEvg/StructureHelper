using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.VisualProperties;

namespace DataAccess.DTOs
{
    internal class PrimitiveVisualPropertyFromDTOConvertStrategy : ConvertStrategy<PrimitiveVisualProperty, PrimitiveVisualPropertyDTO>
    {
        private IUpdateStrategy<IPrimitiveVisualProperty> _updateStrategy;

        public PrimitiveVisualPropertyFromDTOConvertStrategy(Dictionary<(Guid id, Type type), ISaveable> referenceDictionary, IShiftTraceLogger traceLogger) : base(referenceDictionary, traceLogger)
        {
        }

        public override PrimitiveVisualProperty GetNewItem(PrimitiveVisualPropertyDTO source)
        {
            _updateStrategy ??= new PrimitiveVisualPropertyUpdateStrategy();
            ChildClass = this;
            NewItem = new(source.Id);
            return NewItem;
        }
    }
}
