using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Shapes;

namespace DataAccess.DTOs
{
    public class RectangleShapeToDTOConvertStrategy : IConvertStrategy<RectangleShapeDTO, IRectangleShape>
    {
        private IUpdateStrategy<IRectangleShape> updateStrategy;

        public RectangleShapeToDTOConvertStrategy(IUpdateStrategy<IRectangleShape> updateStrategy)
        {
            this.updateStrategy = updateStrategy;
        }

        public RectangleShapeToDTOConvertStrategy() : this (new RectangleShapeUpdateStrategy())
        {
            
        }

        public Dictionary<(Guid id, Type type), ISaveable> ReferenceDictionary { get; set; }
        public IShiftTraceLogger TraceLogger { get; set; }

        public RectangleShapeDTO Convert(IRectangleShape source)
        {
            RectangleShapeDTO newItem = new() { Id = source.Id};
            updateStrategy.Update(newItem, source);
            return newItem;
        }
    }
}
