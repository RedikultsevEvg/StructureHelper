using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Shapes;

namespace DataAccess.DTOs
{
    public class CircleShapeToDTOConvertStrategy : ConvertStrategy<CircleShapeDTO, ICircleShape>
    {
        private IUpdateStrategy<ICircleShape> updateStrategy;

        public CircleShapeToDTOConvertStrategy(IBaseConvertStrategy baseConvertStrategy) : base(baseConvertStrategy)
        {
        }

        public override CircleShapeDTO GetNewItem(ICircleShape source)
        {
            ChildClass = this;
            updateStrategy ??= new CircleShapeUpdateStrategy();
            NewItem = new(source.Id);
            updateStrategy.Update(NewItem, source);
            return NewItem;
        }
    }
}
