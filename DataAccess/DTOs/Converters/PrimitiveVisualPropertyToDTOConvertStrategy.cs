using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.VisualProperties;

namespace DataAccess.DTOs
{
    public class PrimitiveVisualPropertyToDTOConvertStrategy : ConvertStrategy<PrimitiveVisualPropertyDTO, IPrimitiveVisualProperty>
    {
        private IUpdateStrategy<IPrimitiveVisualProperty> _updateStrategy;

        public PrimitiveVisualPropertyToDTOConvertStrategy(
            Dictionary<(Guid id, Type type), ISaveable> referenceDictionary,
            IShiftTraceLogger traceLogger) : base(referenceDictionary, traceLogger)
        {
        }

        public override PrimitiveVisualPropertyDTO GetNewItem(IPrimitiveVisualProperty source)
        {
            _updateStrategy ??= new PrimitiveVisualPropertyUpdateStrategy();
            ChildClass = this;
            NewItem = new(source.Id);
            return NewItem;
        }
    }
}
