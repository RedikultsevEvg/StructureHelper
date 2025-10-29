using netDxf;
using netDxf.Entities;
using netDxf.Tables;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Services.Exports
{
    public enum LayerNames
    {
        StructiralPrimitives,
        StructuralOpenings,
        StructuralRebars
    }
    public class ShapesExportToDxfLogic : IExportResultLogic
    {
        private const string PrimitivesLayer = "STR-PRM";
        private const string OpeningsLayer = "STR-OPN";
        private const string RebarLayer = "STR-REB";
        private const string LayerNameIsUnKnown = ": Layer name is unknown";
        private const string ShapeTypeIsUnknown = ": Shape is unknown";
        private readonly List<(IShape shape, LayerNames layerName)> shapes = [];

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
            Layer primitiveLayer = new Layer(PrimitivesLayer)
            {
                Color = AciColor.Blue
            };
            dxf.Layers.Add(primitiveLayer);

            // save to file
            dxf.Save(FileName);
        }

        private void ProcessShape(DxfDocument dxf, (IShape shape, LayerNames layerName) shape)
        {
            Layer layer = GetOrCreateLayer(dxf, shape.layerName);
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
                Polyline2DVertex vertex = new Polyline2DVertex(item.Point.X, item.Point.Y);
                polylineVertices.Add(vertex);
            }
            Polyline2D polyline2D = new Polyline2D(polylineVertices)
            {
                Layer = layer,
                IsClosed = linePolygon.IsClosed,
            };
            dxf.Entities.Add(polyline2D);
        }

        private Layer GetOrCreateLayer(DxfDocument dxf, LayerNames layerName)
        {
            string newLayerName = GetLayerName(layerName);
            Layer newLayer = dxf.Layers.Contains("newLayerName")
                ? dxf.Layers["newLayerName"]
                : new Layer("newLayerName")
                {
                    Color = GetLayerColor(layerName)
                };

            if (!dxf.Layers.Contains(newLayerName))
                dxf.Layers.Add(newLayer);
            return newLayer;
        }

        private string GetLayerName(LayerNames layerName)
        {
            if (layerName == LayerNames.StructiralPrimitives) { return PrimitivesLayer; }
            else if (layerName == LayerNames.StructuralOpenings) { return OpeningsLayer; }
            else if (layerName == LayerNames.StructuralRebars) { return RebarLayer; }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(layerName) + LayerNameIsUnKnown);
            }
        }

        private AciColor GetLayerColor(LayerNames layerName)
        {
            if (layerName == LayerNames.StructiralPrimitives) { return AciColor.Blue; }
            else if (layerName == LayerNames.StructuralOpenings) { return AciColor.DarkGray; }
            else if (layerName == LayerNames.StructuralRebars) { return AciColor.Magenta; }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(layerName) + LayerNameIsUnKnown);
            }
        }
    }
}
