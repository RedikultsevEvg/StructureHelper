using netDxf;
using netDxf.Entities;
using netDxf.Tables;
using StructureHelperCommon.Models.Shapes;
using System.Collections.Generic;

namespace StructureHelperCommon.Services.Exports
{
    public enum LayerNames
    {
        StructiralPrimitives,
        StructuralOpenings,
        StructuralRebars,
        StructuralPoints
    }
    public class ShapesExportToDxfLogic : IExportToFileLogic
    {
        private const double metresToMillimeters = 1000.0;
        private IGetDxfLayerLogic layerLogic = new GetDxfLayerLogic();
        private IShapeConvertStrategy<EntityObject, IShape> shapeConvertStrategy = new ShapeToDxfEntityConvertStrategy() { Scale = metresToMillimeters};
        public List<(IShape shape, LayerNames layerName)> Shapes { get; set; } = [];

        public ShapesExportToDxfLogic(List<(IShape shape, LayerNames layerName)> shapes)
        {
            this.Shapes.AddRange(shapes);
        }

        public ShapesExportToDxfLogic(IShape shape, LayerNames layerName)
        {
            Shapes.Add((shape, layerName));
        }

        public string FileName { get; set; }

        public void Export()
        {
            DxfDocument dxf = new DxfDocument();
            foreach (var shape in Shapes)
            {
                Layer layer = layerLogic.GetOrCreateLayer(dxf, shape.layerName);
                var entity = shapeConvertStrategy.Convert(shape.shape);
                entity.Layer = layer;
                dxf.Entities.Add(entity);
            }
            dxf.Save(FileName);
        }
    }
}
