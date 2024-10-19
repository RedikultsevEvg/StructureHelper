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
    public class PointNdmPrimitiveToDTOConvertStrategy : IConvertStrategy<PointNdmPrimitiveDTO, IPointNdmPrimitive>
    {
        private IUpdateStrategy<IPointNdmPrimitive> updateStrategy;
        private IConvertStrategy<NdmElementDTO, INdmElement> ndmElementConvertStrategy;
        private IConvertStrategy<Point2DDTO, IPoint2D> pointConvertStrategy;
        private IConvertStrategy<VisualPropertyDTO, IVisualProperty> visualPropsConvertStrategy;

        public PointNdmPrimitiveToDTOConvertStrategy(
            IUpdateStrategy<IPointNdmPrimitive> updateStrategy,
            IConvertStrategy<NdmElementDTO, INdmElement> ndmElementConvertStrategy,
            IConvertStrategy<Point2DDTO, IPoint2D> pointConvertStrategy,
            IConvertStrategy<VisualPropertyDTO, IVisualProperty> visualPropsConvertStrategy)
        {
            this.updateStrategy = updateStrategy;
            this.ndmElementConvertStrategy = ndmElementConvertStrategy;
            this.pointConvertStrategy = pointConvertStrategy;
            this.visualPropsConvertStrategy = visualPropsConvertStrategy;
        }

        public PointNdmPrimitiveToDTOConvertStrategy() : this(
            new PointPrimitiveUpdateStrategy(),
            new NdmElementDTOConvertStrategy(),
            new Point2DToDTOConvertStrategy(),
            new VisualPropertyToDTOConvertStrategy()
        ) { }

        public Dictionary<(Guid id, Type type), ISaveable> ReferenceDictionary { get; set; }
        public IShiftTraceLogger TraceLogger { get; set; }

        public PointNdmPrimitiveDTO Convert(IPointNdmPrimitive source)
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

        private PointNdmPrimitiveDTO GetNewPrimitive(IPointNdmPrimitive source)
        {
            PointNdmPrimitiveDTO newItem = new() { Id = source.Id};
            updateStrategy.Update(newItem, source);
            newItem.NdmElement = ndmElementConvertStrategy.Convert(source.NdmElement);
            newItem.Center = pointConvertStrategy.Convert(source.Center);
            newItem.VisualProperty = visualPropsConvertStrategy.Convert(source.VisualProperty);
            return newItem;
        }

        private void PrepareStrategies()
        {
                ndmElementConvertStrategy.ReferenceDictionary =
                pointConvertStrategy.ReferenceDictionary =
                visualPropsConvertStrategy.ReferenceDictionary =
                ReferenceDictionary;
                ndmElementConvertStrategy.TraceLogger =
                pointConvertStrategy.TraceLogger =
                visualPropsConvertStrategy.TraceLogger =
                TraceLogger;
        }

        private void Check()
        {
            var checkLogic = new CheckConvertLogic<PointNdmPrimitiveDTO, IPointNdmPrimitive>(this);
            checkLogic.Check();
        }
    }
}
