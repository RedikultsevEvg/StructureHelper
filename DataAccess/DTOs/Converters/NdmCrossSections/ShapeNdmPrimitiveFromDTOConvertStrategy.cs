using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Shapes;
using StructureHelperLogics.NdmCalculations.Primitives;

namespace DataAccess.DTOs
{
    public class ShapeNdmPrimitiveFromDTOConvertStrategy : ConvertStrategy<IShapeNdmPrimitive, IShapeNdmPrimitive>
    {
        IUpdateStrategy<IShapeNdmPrimitive> updateStrategy;
        private IConvertStrategy<IShape, IShape> shapeConvertStrategy;
        private IConvertStrategy<INdmElement, INdmElement> ndmElementConvertStrategy;
        private IConvertStrategy<Point2D, Point2DDTO> pointConvertStrategy;
        private IConvertStrategy<IVisualProperty, VisualPropertyDTO> visualPropsConvertStrategy;
        private IConvertStrategy<IDivisionSize, IDivisionSize> divisionConvertStrategy;

        private ShapeNdmPrimitive newItem;

        public ShapeNdmPrimitiveFromDTOConvertStrategy(IBaseConvertStrategy baseConvertStrategy) : base(baseConvertStrategy)
        {
            updateStrategy = new ShapeNdmPrimitiveUpdateStrategy() { UpdateChildren = false };
            shapeConvertStrategy = new ShapeFromDTOConvertStrategy(this);
            ndmElementConvertStrategy = new NdmElementFromDTOConvertStrategy(this);
            pointConvertStrategy = new Point2DFromDTOConvertStrategy(this);
            visualPropsConvertStrategy = new VisualPropertyFromDTOConvertStrategy(this);
            divisionConvertStrategy = new DivisionSizeFromDTOConvertStrategy(this);
        }

        public override IShapeNdmPrimitive GetNewItem(IShapeNdmPrimitive source)
        {
            ChildClass = this;
            if (source is not ShapeNdmPrimitiveDTO sourceDTO)
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(source) + ": source shape is not DTO object");
            }
            newItem = new(source.Id);
            updateStrategy.Update(newItem, sourceDTO);
            updateChildProperties(sourceDTO);
            NewItem = newItem;
            return NewItem;
        }
        private void updateChildProperties(ShapeNdmPrimitiveDTO source)
        {
            newItem.SetShape(shapeConvertStrategy.Convert(source.Shape));
            newItem.NdmElement = ndmElementConvertStrategy.Convert(source.NdmElement);
            newItem.Center = pointConvertStrategy.Convert(source.Center as Point2DDTO);
            newItem.VisualProperty = visualPropsConvertStrategy.Convert(source.VisualProperty as VisualPropertyDTO);
            newItem.DivisionSize = divisionConvertStrategy.Convert(source.DivisionSize);
        }

    }
}
