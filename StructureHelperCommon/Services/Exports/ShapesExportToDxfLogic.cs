using netDxf;
using netDxf.Entities;
using netDxf.Tables;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.Shapes;
using System.Collections.Generic;

namespace StructureHelperCommon.Services.Exports
{
    public enum LayerNames
    {
        StructiralPrimitives,
        StructuralOpenings,
        StructuralRebars
    }
    public class ShapesExportToDxfLogic : IExportToFileLogic
    {
        private const string ShapeTypeIsUnknown = ": Shape is unknown";
        private const double metresToMillimeters = 1000.0;
        private readonly List<(IShape shape, LayerNames layerName)> shapes = [];
        private IGetDxfLayerLogic layerLogic = new GetDxfLayerLogic();

        public ShapesExportToDxfLogic(List<(IShape shape, LayerNames layerName)> shapes)
        {
            this.shapes.AddRange(shapes);
        }

        public ShapesExportToDxfLogic(IShape shape, LayerNames layerName)
        {
            shapes.Add((shape, layerName));
        }

        public string FileName { get; set; }

        public void Export()
        {
            DxfDocument dxf = new DxfDocument();
            foreach (var shape in shapes)
            {
                ProcessShape(dxf, shape);
            }
            dxf.Save(FileName);
        }

        private void ProcessShape(DxfDocument dxf, (IShape shape, LayerNames layerName) shape)
        {
            Layer layer = layerLogic.GetOrCreateLayer(dxf, shape.layerName);
            ProcessShape(dxf, shape.shape, layer);
        }

        private void ProcessShape(DxfDocument dxf, IShape shape, Layer layer)
        {
            if (shape is ILinePolygonShape linePolygon)
            {
                ProcessLinePolygon(dxf, linePolygon, layer);
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(shape) + ShapeTypeIsUnknown);
            }
        }

        private void ProcessLinePolygon(DxfDocument dxf, ILinePolygonShape linePolygon, Layer layer)
        {
            List<Polyline2DVertex> polylineVertices = [];
            foreach (var item in linePolygon.Vertices)
            {
                Polyline2DVertex vertex = new Polyline2DVertex(item.Point.X * metresToMillimeters, item.Point.Y * metresToMillimeters);
                polylineVertices.Add(vertex);
            }
            Polyline2D polyline2D = new Polyline2D(polylineVertices)
            {
                Layer = layer,
                IsClosed = linePolygon.IsClosed,
            };
            dxf.Entities.Add(polyline2D);
        }
    }
}
