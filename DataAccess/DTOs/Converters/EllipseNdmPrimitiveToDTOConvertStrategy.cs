using DataAccess.DTOs.Converters;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Loggers;
using StructureHelperCommon.Models.Shapes;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs
{
    public class EllipseNdmPrimitiveToDTOConvertStrategy : IConvertStrategy<EllipseNdmPrimitiveDTO, IEllipseNdmPrimitive>
    {
        private IUpdateStrategy<IEllipseNdmPrimitive> updateStrategy;
        private IConvertStrategy<RectangleShapeDTO, IRectangleShape> rectangleShapeConvertStrategy;
        private IConvertStrategy<NdmElementDTO, INdmElement> ndmElementConvertStrategy;
        private IConvertStrategy<Point2DDTO, IPoint2D> pointConvertStrategy;
        private IConvertStrategy<VisualPropertyDTO, IVisualProperty> visualPropsConvertStrategy;
        private IConvertStrategy<DivisionSizeDTO, IDivisionSize> divisionConvertStrategy;

        public EllipseNdmPrimitiveToDTOConvertStrategy(
            IUpdateStrategy<IEllipseNdmPrimitive> updateStrategy,
            IConvertStrategy<RectangleShapeDTO, IRectangleShape> rectangleShapeConvertStrategy,
            IConvertStrategy<NdmElementDTO, INdmElement> ndmElementConvertStrategy,
            IConvertStrategy<Point2DDTO, IPoint2D> pointConvertStrategy,
            IConvertStrategy<VisualPropertyDTO, IVisualProperty> visualPropsConvertStrategy,
            IConvertStrategy<DivisionSizeDTO, IDivisionSize> divisionConvertStrategy)
        {
            this.updateStrategy = updateStrategy;
            this.rectangleShapeConvertStrategy = rectangleShapeConvertStrategy;
            this.ndmElementConvertStrategy = ndmElementConvertStrategy;
            this.pointConvertStrategy = pointConvertStrategy;
            this.visualPropsConvertStrategy = visualPropsConvertStrategy;
            this.divisionConvertStrategy = divisionConvertStrategy;
        }

        public EllipseNdmPrimitiveToDTOConvertStrategy() : this(
            new EllipsePrimitiveUpdateStrategy(),
            new RectangleShapeToDTOConvertStrategy(),
            new NdmElementDTOConvertStrategy(),
            new Point2DToDTOConvertStrategy(),
            new VisualPropertyToDTOConvertStrategy(),
            new DivisionSizeToDTOConvertStrategy()
            )
        {
            
        }

        public Dictionary<(Guid id, Type type), ISaveable> ReferenceDictionary { get; set; }
        public IShiftTraceLogger TraceLogger { get; set; }

        public EllipseNdmPrimitiveDTO Convert(IEllipseNdmPrimitive source)
        {
            try
            {
                Check();
                PrepareStrategies();
                return GetNewPrimitive(source);
            }
            catch (Exception ex)
            {
                TraceLogger?.AddMessage(LoggerStrings.LogicType(this), TraceLogStatuses.Debug);
                TraceLogger?.AddMessage(ex.Message, TraceLogStatuses.Error);
                throw;
            }
        }

        private EllipseNdmPrimitiveDTO GetNewPrimitive(IEllipseNdmPrimitive source)
        {
            EllipseNdmPrimitiveDTO newItem = new() { Id = source.Id };
            updateStrategy.Update(newItem, source);
            newItem.NdmElement = ndmElementConvertStrategy.Convert(source.NdmElement);
            newItem.RectangleShape = rectangleShapeConvertStrategy.Convert(source.Shape as IRectangleShape);
            newItem.Center = pointConvertStrategy.Convert(source.Center);
            newItem.VisualProperty = visualPropsConvertStrategy.Convert(source.VisualProperty);
            newItem.DivisionSize = divisionConvertStrategy.Convert(source.DivisionSize);
            return newItem;
        }

        private void PrepareStrategies()
        {
            rectangleShapeConvertStrategy.ReferenceDictionary =
                ndmElementConvertStrategy.ReferenceDictionary =
                pointConvertStrategy.ReferenceDictionary =
                visualPropsConvertStrategy.ReferenceDictionary =
                divisionConvertStrategy.ReferenceDictionary =
                ReferenceDictionary;
            rectangleShapeConvertStrategy.TraceLogger =
                ndmElementConvertStrategy.TraceLogger =
                pointConvertStrategy.TraceLogger =
                visualPropsConvertStrategy.TraceLogger =
                divisionConvertStrategy.TraceLogger =
                TraceLogger;
        }

        private void Check()
        {
            var checkLogic = new CheckConvertLogic<EllipseNdmPrimitiveDTO, IEllipseNdmPrimitive>(this);
            checkLogic.Check();
        }
    }
}
