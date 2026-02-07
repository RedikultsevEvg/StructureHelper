using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Shapes;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.DTOs
{
    public class EllipseShapeToDTOConvertStrategy : ConvertStrategy<EllipseShapeDTO, IEllipseShape>
    {
        private IUpdateStrategy<IEllipseShape> updateStrategy;

        public EllipseShapeToDTOConvertStrategy()
        {
        }

        public EllipseShapeToDTOConvertStrategy(IBaseConvertStrategy baseConvertStrategy) : base(baseConvertStrategy)
        {
        }

        private IUpdateStrategy<IEllipseShape> UpdateStrategy => updateStrategy ?? new EllipseShapeUpdateStrategy();
        public override EllipseShapeDTO GetNewItem(IEllipseShape source)
        {
            ChildClass = this;
            NewItem = new(source.Id);
            UpdateStrategy.Update(NewItem, source);
            return NewItem;
        }
    }
}
