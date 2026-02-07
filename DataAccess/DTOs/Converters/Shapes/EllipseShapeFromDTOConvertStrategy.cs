using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Shapes;

namespace DataAccess.DTOs
{
    public class EllipseShapeFromDTOConvertStrategy : ConvertStrategy<EllipseShape, EllipseShapeDTO>
    {
        private IUpdateStrategy<IEllipseShape> updateStrategy;

        public EllipseShapeFromDTOConvertStrategy(IBaseConvertStrategy baseConvertStrategy) : base(baseConvertStrategy)
        {
        }

        private IUpdateStrategy<IEllipseShape> UpdateStrategy => updateStrategy ?? new EllipseShapeUpdateStrategy();
        public override EllipseShape GetNewItem(EllipseShapeDTO source)
        {
            ChildClass = this;
            NewItem = new(source.Id);
            UpdateStrategy.Update(NewItem, source);
            return NewItem;
        }
    }
}
