using StructureHelper.Models.Materials;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace DataAccess.DTOs
{
    public class NdmPrimitiveFromDTOConvertStrategy : ConvertStrategy<INdmPrimitive, INdmPrimitive>
    {
        private const string PrimitiveIs = "Primtive is";
        private IConvertStrategy<RebarNdmPrimitive, RebarNdmPrimitiveDTO> rebarConvertStrategy;
        private IConvertStrategy<PointNdmPrimitive, PointNdmPrimitiveDTO> pointConvertStrategy;
        private IConvertStrategy<EllipseNdmPrimitive, EllipseNdmPrimitiveDTO> ellipseConvertStrategy;
        private IConvertStrategy<RectangleNdmPrimitive, RectangleNdmPrimitiveDTO> rectangleConvertStrategy;

        private IHeadMaterial headMaterial;

        public NdmPrimitiveFromDTOConvertStrategy(
            IConvertStrategy<RebarNdmPrimitive, RebarNdmPrimitiveDTO> rebarConvertStrategy,
            IConvertStrategy<PointNdmPrimitive, PointNdmPrimitiveDTO> pointConvertStrategy,
            IConvertStrategy<EllipseNdmPrimitive, EllipseNdmPrimitiveDTO> ellipseConvertStrategy,
            IConvertStrategy<RectangleNdmPrimitive, RectangleNdmPrimitiveDTO> rectangleConvertStrategy)
        {
            this.rebarConvertStrategy = rebarConvertStrategy;
            this.pointConvertStrategy = pointConvertStrategy;
            this.ellipseConvertStrategy = ellipseConvertStrategy;
            this.rectangleConvertStrategy = rectangleConvertStrategy;
        }

        public NdmPrimitiveFromDTOConvertStrategy() : this(
            new RebarNdmPrimitiveFromDTOConvertStrategy(),
            new PointNdmPrimitiveFromDTOConvertStrategy(),
            new EllipseNdmPrimitiveFromDTOConvertStrategy(),
            new RectanglePrimitiveFromDTOConvertStrategy())
        {
            
        }

        public override INdmPrimitive GetNewItem(INdmPrimitive source)
        {
            INdmPrimitive newItem = GetNewPrimitive(source);
            if (source.NdmElement.HeadMaterial != null)
            {
                GetMaterial(source.NdmElement.HeadMaterial);
                newItem.NdmElement.HeadMaterial = headMaterial;
            }
            return newItem;
        }

        private INdmPrimitive GetNewPrimitive(INdmPrimitive source)
        {
            if (source is RebarNdmPrimitiveDTO rebar)
            {
                return GetRebar(rebar);
            }
            if (source is PointNdmPrimitiveDTO point)
            {
                return GetPoint(point);
            }
            if (source is EllipseNdmPrimitiveDTO ellipse)
            {
                return GetEllipse(ellipse);
            }
            if (source is RectangleNdmPrimitiveDTO rectangle)
            {
                return GetRectangle(rectangle);
            }
            if (source is ShapeNdmPrimitiveDTO shape)
            {
                return GetShape(shape);
            }
            string errorString = ErrorStrings.ObjectTypeIsUnknownObj(source);
            TraceLogger.AddMessage(errorString, TraceLogStatuses.Error);
            throw new StructureHelperException(errorString);
        }

        private INdmPrimitive GetShape(ShapeNdmPrimitiveDTO shape)
        {
            TraceLogger?.AddMessage($"{PrimitiveIs} shape ndm primitive");
            ShapeNdmPrimitiveFromDTOConvertStrategy convertStrategy = new(this);
            IShapeNdmPrimitive shapeNdmPrimitive = convertStrategy.Convert(shape);
            return shapeNdmPrimitive;
        }

        private void GetMaterial(IHeadMaterial source)
        {
            HeadMaterialFromDTOConvertStrategy convertStrategy = new()
            {
                ReferenceDictionary = ReferenceDictionary,
                TraceLogger = TraceLogger
            };
            DictionaryConvertStrategy<IHeadMaterial, IHeadMaterial> convertLogic = new(this, convertStrategy);
            headMaterial = convertLogic.Convert(source);
        }

        private INdmPrimitive GetRebar(RebarNdmPrimitiveDTO rebar)
        {
            TraceLogger?.AddMessage($"{PrimitiveIs} rebar ndm primitive");
            rebarConvertStrategy.ReferenceDictionary = ReferenceDictionary;
            rebarConvertStrategy.TraceLogger = TraceLogger;
            RebarNdmPrimitive newItem = rebarConvertStrategy.Convert(rebar);
            TraceLogger?.AddMessage($"Primtive has been obtained successfully, Name = {newItem.Name}");
            if (rebar.HostPrimitive != null)
            {
                newItem.HostPrimitive = GetHostPrimitive(rebar);
            }
            return newItem;
        }

        private INdmPrimitive GetHostPrimitive(RebarNdmPrimitiveDTO rebar)
        {
            NdmPrimitiveFromDTOConvertStrategy convertStrategy = new()
            {
                ReferenceDictionary = ReferenceDictionary,
                TraceLogger = TraceLogger
            };
            DictionaryConvertStrategy<INdmPrimitive, INdmPrimitive> convertLogic = new(this, convertStrategy);
            INdmPrimitive hostPrimitive = convertLogic.Convert(rebar.HostPrimitive);
            TraceLogger?.AddMessage($"Host primitive Id = {hostPrimitive.Id}, Name = {hostPrimitive.Name}");
            return hostPrimitive;
        }

        private INdmPrimitive GetPoint(PointNdmPrimitiveDTO point)
        {
            TraceLogger?.AddMessage($"{PrimitiveIs} point ndm primitive");
            pointConvertStrategy.ReferenceDictionary = ReferenceDictionary;
            pointConvertStrategy.TraceLogger = TraceLogger;
            PointNdmPrimitive newItem = pointConvertStrategy.Convert(point);
            TraceLogger?.AddMessage($"Primtive has been obtained successfully, Name = {newItem.Name}");
            return newItem;
        }

        private INdmPrimitive GetEllipse(EllipseNdmPrimitiveDTO ellipse)
        {
            TraceLogger?.AddMessage($"{PrimitiveIs} ellipse ndm primitive");
            ellipseConvertStrategy.ReferenceDictionary = ReferenceDictionary;
            ellipseConvertStrategy.TraceLogger = TraceLogger;
            EllipseNdmPrimitive newItem = ellipseConvertStrategy.Convert(ellipse);
            TraceLogger?.AddMessage($"Primtive has been obtained successfully, Name = {newItem.Name}");
            return newItem;
        }

        private INdmPrimitive GetRectangle(RectangleNdmPrimitiveDTO rectangle)
        {
            TraceLogger?.AddMessage($"{PrimitiveIs} rectangle ndm primitive");
            rectangleConvertStrategy.ReferenceDictionary = ReferenceDictionary;
            rectangleConvertStrategy.TraceLogger = TraceLogger;
            RectangleNdmPrimitive newItem = rectangleConvertStrategy.Convert(rectangle);
            TraceLogger?.AddMessage($"Primtive has been obtained successfully, Name = {newItem.Name}");
            return newItem;
        }
    }
}
