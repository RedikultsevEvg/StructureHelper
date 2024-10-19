using DataAccess.DTOs.Converters;
using DataAccess.DTOs.DTOEntities;
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
    public class RebarNdmPrimitiveToDTOConvertStrategy : IConvertStrategy<RebarNdmPrimitiveDTO, IRebarNdmPrimitive>
    {
        private IUpdateStrategy<IRebarNdmPrimitive> updateStrategy;
        private IConvertStrategy<NdmElementDTO, INdmElement> ndmElementConvertStrategy;
        private IConvertStrategy<Point2DDTO, IPoint2D> pointConvertStrategy;
        private IConvertStrategy<VisualPropertyDTO, IVisualProperty> visualPropsConvertStrategy;
        private IConvertStrategy<INdmPrimitive, INdmPrimitive> hostPrimitiveConvertStrategy;

        public RebarNdmPrimitiveToDTOConvertStrategy(
            IUpdateStrategy<IRebarNdmPrimitive> updateStrategy,
            IConvertStrategy<NdmElementDTO, INdmElement> ndmElementConvertStrategy,
            IConvertStrategy<Point2DDTO, IPoint2D> pointConvertStrategy,
            IConvertStrategy<VisualPropertyDTO, IVisualProperty> visualPropsConvertStrategy)
        {
            this.updateStrategy = updateStrategy;
            this.ndmElementConvertStrategy = ndmElementConvertStrategy;
            this.pointConvertStrategy = pointConvertStrategy;
            this.visualPropsConvertStrategy = visualPropsConvertStrategy;
        }

        public RebarNdmPrimitiveToDTOConvertStrategy() : this(
            new RebarNdmPrimitiveUpdateStrategy(),
            new NdmElementDTOConvertStrategy(),
            new Point2DToDTOConvertStrategy(),
            new VisualPropertyToDTOConvertStrategy()
         ) { }

        public Dictionary<(Guid id, Type type), ISaveable> ReferenceDictionary { get; set; }
        public IShiftTraceLogger TraceLogger { get; set; }

        public RebarNdmPrimitiveDTO Convert(IRebarNdmPrimitive source)
        {
            try
            {
                Check();
                PrepareStrategies();
                return GetNewPrimitive(source);
            }
            catch (Exception ex)
            {
                TraceLogger?.AddMessage(LoggerStrings.LogicType(this), TraceLogStatuses.Error);
                TraceLogger?.AddMessage(ex.Message, TraceLogStatuses.Error);
                throw;
            }
        }

        private RebarNdmPrimitiveDTO GetNewPrimitive(IRebarNdmPrimitive source)
        {
            RebarNdmPrimitiveDTO newItem = new() { Id = source.Id };
            //updateStrategy.Update(newItem, source);
            newItem.NdmElement = ndmElementConvertStrategy.Convert(source.NdmElement);
            newItem.Center = pointConvertStrategy.Convert(source.Center);
            newItem.VisualProperty = visualPropsConvertStrategy.Convert(source.VisualProperty);
            if (source.HostPrimitive is not null)
            {
                hostPrimitiveConvertStrategy = new NdmPrimitiveToDTOConvertStrategy(null, null,
                    new EllipseNdmPrimitiveToDTOConvertStrategy(),
                    new RectangleNdmPrimitiveToDTOConvertStrategy());
                hostPrimitiveConvertStrategy.ReferenceDictionary = ReferenceDictionary;
                hostPrimitiveConvertStrategy.TraceLogger = TraceLogger;
                newItem.HostPrimitive = hostPrimitiveConvertStrategy.Convert(source.HostPrimitive);
            }
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
            var checkLogic = new CheckConvertLogic<RebarNdmPrimitiveDTO, IRebarNdmPrimitive>(this);
            checkLogic.Check();
        }
    }
}
