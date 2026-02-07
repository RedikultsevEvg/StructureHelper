using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Shapes;
using StructureHelperLogics.NdmCalculations.Primitives;

namespace DataAccess.DTOs
{
    public class EllipseNdmPrimitiveToDTOConvertStrategy : ConvertStrategy<EllipseNdmPrimitiveDTO, IEllipseNdmPrimitive>
    {
        private IUpdateStrategy<IEllipseNdmPrimitive> updateStrategy;
        private IConvertStrategy<EllipseShapeDTO, IEllipseShape> ellipseShapeConvertStrategy;
        private IConvertStrategy<NdmElementDTO, INdmElement> ndmElementConvertStrategy;
        private IConvertStrategy<Point2DDTO, IPoint2D> pointConvertStrategy;
        private IConvertStrategy<VisualPropertyDTO, IVisualProperty> visualPropsConvertStrategy;
        private IConvertStrategy<DivisionSizeDTO, IDivisionSize> divisionConvertStrategy;

        public EllipseNdmPrimitiveToDTOConvertStrategy(
            IUpdateStrategy<IEllipseNdmPrimitive> updateStrategy,
            IConvertStrategy<EllipseShapeDTO, IEllipseShape> ellipseShapeConvertStrategy,
            IConvertStrategy<NdmElementDTO, INdmElement> ndmElementConvertStrategy,
            IConvertStrategy<Point2DDTO, IPoint2D> pointConvertStrategy,
            IConvertStrategy<VisualPropertyDTO, IVisualProperty> visualPropsConvertStrategy,
            IConvertStrategy<DivisionSizeDTO, IDivisionSize> divisionConvertStrategy)
        {
            this.updateStrategy = updateStrategy;
            this.ellipseShapeConvertStrategy = ellipseShapeConvertStrategy;
            this.ndmElementConvertStrategy = ndmElementConvertStrategy;
            this.pointConvertStrategy = pointConvertStrategy;
            this.visualPropsConvertStrategy = visualPropsConvertStrategy;
            this.divisionConvertStrategy = divisionConvertStrategy;
        }

        public EllipseNdmPrimitiveToDTOConvertStrategy() : this(
            new EllipsePrimitiveUpdateStrategy(),
            new EllipseShapeToDTOConvertStrategy(),
            new NdmElementToDTOConvertStrategy(),
            new Point2DToDTOConvertStrategy(),
            new VisualPropertyToDTOConvertStrategy(),
            new DivisionSizeToDTOConvertStrategy()
            )
        {
            
        }

        public override EllipseNdmPrimitiveDTO GetNewItem(IEllipseNdmPrimitive source)
        {
            ChildClass = this;
            PrepareStrategies();
            NewItem = GetNewPrimitive(source);
            return NewItem;
        }

        private EllipseNdmPrimitiveDTO GetNewPrimitive(IEllipseNdmPrimitive source)
        {
            EllipseNdmPrimitiveDTO newItem = new(source.Id);
            updateStrategy.Update(newItem, source);
            newItem.NdmElement = ndmElementConvertStrategy.Convert(source.NdmElement);
            newItem.EllipseShape = ellipseShapeConvertStrategy.Convert(source.Shape as IEllipseShape);
            newItem.Center = pointConvertStrategy.Convert(source.Center);
            newItem.VisualProperty = visualPropsConvertStrategy.Convert(source.VisualProperty);
            newItem.DivisionSize = divisionConvertStrategy.Convert(source.DivisionSize);
            return newItem;
        }

        private void PrepareStrategies()
        {
            ellipseShapeConvertStrategy.ReferenceDictionary =
                ndmElementConvertStrategy.ReferenceDictionary =
                pointConvertStrategy.ReferenceDictionary =
                visualPropsConvertStrategy.ReferenceDictionary =
                divisionConvertStrategy.ReferenceDictionary =
                ReferenceDictionary;
            ellipseShapeConvertStrategy.TraceLogger =
                ndmElementConvertStrategy.TraceLogger =
                pointConvertStrategy.TraceLogger =
                visualPropsConvertStrategy.TraceLogger =
                divisionConvertStrategy.TraceLogger =
                TraceLogger;
        }


    }
}
