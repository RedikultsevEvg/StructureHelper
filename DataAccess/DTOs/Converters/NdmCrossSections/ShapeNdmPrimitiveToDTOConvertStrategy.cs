using DataAccess.DTOs.Converters;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Shapes;
using StructureHelperLogics.NdmCalculations.Primitives;

namespace DataAccess.DTOs
{
    public class ShapeNdmPrimitiveToDTOConvertStrategy : ConvertStrategy<ShapeNdmPrimitiveDTO, IShapeNdmPrimitive>
    {
        private IUpdateStrategy<IShapeNdmPrimitive> updateStrategy;
        private IConvertStrategy<IShape, IShape> shapeConvertStrategy;
        private IConvertStrategy<NdmElementDTO, INdmElement> ndmElementConvertStrategy;
        private IConvertStrategy<Point2DDTO, IPoint2D> pointConvertStrategy;
        private IConvertStrategy<VisualPropertyDTO, IVisualProperty> visualPropsConvertStrategy;
        private IConvertStrategy<DivisionSizeDTO, IDivisionSize> divisionConvertStrategy;

        public ShapeNdmPrimitiveToDTOConvertStrategy(IBaseConvertStrategy baseConvertStrategy) : base(baseConvertStrategy)
        {
        }

        public override ShapeNdmPrimitiveDTO GetNewItem(IShapeNdmPrimitive source)
        {
            ChildClass = this;
            NewItem = new(source.Id);
            InitializeStrategies();
            updateStrategy.Update(NewItem, source);
            updateChildProperties(source);
            return NewItem;
        }

        private void updateChildProperties(IShapeNdmPrimitive source)
        {
            NewItem.SetShape(shapeConvertStrategy.Convert(source.Shape));
            NewItem.NdmElement = ndmElementConvertStrategy.Convert(source.NdmElement);
            NewItem.Center = pointConvertStrategy.Convert(source.Center);
            NewItem.VisualProperty = visualPropsConvertStrategy.Convert(source.VisualProperty);
            NewItem.DivisionSize = divisionConvertStrategy.Convert(source.DivisionSize);
        }

        private void InitializeStrategies()
        {
            updateStrategy = new ShapeNdmPrimitiveUpdateStrategy() { UpdateChildren = false };
            shapeConvertStrategy = new DictionaryConvertStrategy<IShape, IShape>(this, new ShapeToDTOConvertStrategy(this));
            ndmElementConvertStrategy = new NdmElementToDTOConvertStrategy() { ReferenceDictionary = ReferenceDictionary, TraceLogger = TraceLogger};
            pointConvertStrategy = new Point2DToDTOConvertStrategy() { ReferenceDictionary = ReferenceDictionary, TraceLogger = TraceLogger };
            visualPropsConvertStrategy = new VisualPropertyToDTOConvertStrategy() { ReferenceDictionary = ReferenceDictionary, TraceLogger = TraceLogger };
            divisionConvertStrategy = new DivisionSizeToDTOConvertStrategy() { ReferenceDictionary = ReferenceDictionary, TraceLogger = TraceLogger };
        }
    }
}
