using DataAccess.DTOs.DTOEntities;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.NdmCalculations.Primitives;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DataAccess.DTOs.Converters
{
    public class NdmPrimitiveToDTOConvertStrategy : IConvertStrategy<INdmPrimitive, INdmPrimitive>
    {
        private readonly IConvertStrategy<RebarNdmPrimitiveDTO, IRebarNdmPrimitive> rebarConvertStrategy;
        private readonly IConvertStrategy<PointNdmPrimitiveDTO, IPointNdmPrimitive> pointConvertStrategy;
        private readonly IConvertStrategy<EllipseNdmPrimitiveDTO, IEllipseNdmPrimitive> ellipseConvertStrategy;
        private readonly IConvertStrategy<RectangleNdmPrimitiveDTO, IRectangleNdmPrimitive> rectangleConvertStrategy;

        public Dictionary<(Guid id, Type type), ISaveable> ReferenceDictionary { get; set; }
        public IShiftTraceLogger TraceLogger { get; set; }

        public NdmPrimitiveToDTOConvertStrategy(
            IConvertStrategy<RebarNdmPrimitiveDTO,IRebarNdmPrimitive> rebarConvertStrategy,
            IConvertStrategy<PointNdmPrimitiveDTO, IPointNdmPrimitive> pointConvertStrategy,
            IConvertStrategy<EllipseNdmPrimitiveDTO, IEllipseNdmPrimitive> ellipseConvertStrategy,
            IConvertStrategy<RectangleNdmPrimitiveDTO, IRectangleNdmPrimitive> rectangleConvertStrategy)
        {
            this.rebarConvertStrategy = rebarConvertStrategy;
            this.pointConvertStrategy = pointConvertStrategy;
            this.ellipseConvertStrategy = ellipseConvertStrategy;
            this.rectangleConvertStrategy = rectangleConvertStrategy;
        }

        public NdmPrimitiveToDTOConvertStrategy() : this(
            new RebarNdmPrimitiveToDTOConvertStrategy(),
            new PointNdmPrimitiveToDTOConvertStrategy(),
            new EllipseNdmPrimitiveToDTOConvertStrategy(),
            new RectangleNdmPrimitiveToDTOConvertStrategy())
        {
            
        }


        public INdmPrimitive Convert(INdmPrimitive source)
        {
            if (source is IRebarNdmPrimitive rebar)
            {
                return ProcessRebar(rebar);
            }
            if (source is IPointNdmPrimitive point)
            {
                return ProcessPoint(point);
            }
            if (source is IEllipseNdmPrimitive ellipse)
            {
                return ProcessEllipse(ellipse);
            }
            if (source is IRectangleNdmPrimitive rectangle)
            {
                return ProcessRectangle(rectangle);
            }
            TraceLogger.AddMessage("Object type is unknown", TraceLogStatuses.Error);
            throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(source));
        }

        private RebarNdmPrimitiveDTO ProcessRebar(IRebarNdmPrimitive rebar)
        {
            rebarConvertStrategy.ReferenceDictionary = ReferenceDictionary;
            rebarConvertStrategy.TraceLogger = TraceLogger;
            var convertLogic = new DictionaryConvertStrategy<RebarNdmPrimitiveDTO, IRebarNdmPrimitive>(this, rebarConvertStrategy);
            return convertLogic.Convert(rebar);
        }

        private PointNdmPrimitiveDTO ProcessPoint(IPointNdmPrimitive point)
        {
            pointConvertStrategy.ReferenceDictionary = ReferenceDictionary;
            pointConvertStrategy.TraceLogger = TraceLogger;
            var convertLogic = new DictionaryConvertStrategy<PointNdmPrimitiveDTO, IPointNdmPrimitive>(this, pointConvertStrategy);
            return convertLogic.Convert(point);
        }

        private EllipseNdmPrimitiveDTO ProcessEllipse(IEllipseNdmPrimitive ellipse)
        {
            ellipseConvertStrategy.ReferenceDictionary = ReferenceDictionary;
            ellipseConvertStrategy.TraceLogger = TraceLogger;
            var convertLogic = new DictionaryConvertStrategy<EllipseNdmPrimitiveDTO, IEllipseNdmPrimitive>(this, ellipseConvertStrategy);
            return convertLogic.Convert(ellipse);
        }

        private RectangleNdmPrimitiveDTO ProcessRectangle(IRectangleNdmPrimitive rectangle)
        {
            rectangleConvertStrategy.ReferenceDictionary = ReferenceDictionary;
            rectangleConvertStrategy.TraceLogger = TraceLogger;
            var convertLogic = new DictionaryConvertStrategy<RectangleNdmPrimitiveDTO, IRectangleNdmPrimitive>(this, rectangleConvertStrategy);
            return convertLogic.Convert(rectangle);
        }
    }
}
