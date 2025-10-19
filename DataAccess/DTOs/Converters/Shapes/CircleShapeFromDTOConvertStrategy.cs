using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs
{
    public class CircleShapeFromDTOConvertStrategy : ConvertStrategy<CircleShape, CircleShapeDTO>
    {
        private IUpdateStrategy<ICircleShape> updateStrategy;

        public CircleShapeFromDTOConvertStrategy(IBaseConvertStrategy baseConvertStrategy) : base(baseConvertStrategy)
        {
        }

        public override CircleShape GetNewItem(CircleShapeDTO source)
        {
            ChildClass = this;
            ChildClass = this;
            updateStrategy ??= new CircleShapeUpdateStrategy();
            NewItem = new CircleShape(source.Id);
            updateStrategy.Update(NewItem, source);
            return NewItem;
        }
    }
}
