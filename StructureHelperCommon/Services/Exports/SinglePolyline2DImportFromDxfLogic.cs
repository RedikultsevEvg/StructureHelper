using netDxf.Entities;
using System.Collections.Generic;
using System.Linq;

namespace StructureHelperCommon.Services.Exports
{
    public class SinglePolyline2DImportFromDxfLogic : IImportFromFileLogic
    {
        public string? FileName { get; set; }
        public LayerNames LayerName { get; set; } = LayerNames.StructiralPrimitives;
        public List<Polyline2D> Polyline2Ds { get; private set; } = [];
        public void Import()
        {
            var importLogic = new EntitiesImportFromDxfLogic()
            {
                FileName = FileName
            };
            importLogic.Import();
            var entities = importLogic.Entities;
            string layerName = new GetDxfLayerLogic().GetLayerName(LayerName);
            var collection = entities.Where(x => x is Polyline2D && x.Layer.Name == layerName).ToList();
            collection.ForEach(x => Polyline2Ds.Add(x as Polyline2D));
        }
    }
}
