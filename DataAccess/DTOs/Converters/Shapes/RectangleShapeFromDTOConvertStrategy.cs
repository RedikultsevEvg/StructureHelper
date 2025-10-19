using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Shapes;

namespace DataAccess.DTOs
{
    internal class RectangleShapeFromDTOConvertStrategy : ConvertStrategy<RectangleShape, RectangleShapeDTO>
    {
        private IUpdateStrategy<IRectangleShape> updateStrategy;

        public RectangleShapeFromDTOConvertStrategy(IBaseConvertStrategy baseConvertStrategy) : base(baseConvertStrategy)
        {
        }

        public override RectangleShape GetNewItem(RectangleShapeDTO source)
        {
            InitializeStrategies();
            NewItem = new(source.Id);
            updateStrategy.Update(NewItem, source);
            return NewItem;
        }

        private void InitializeStrategies()
        {
            updateStrategy ??= new RectangleShapeUpdateStrategy();
        }
    }
}
