using netDxf;
using netDxf.Entities;
using netDxf.Tables;
using StructureHelperCommon.Models.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Services.Exports
{
    public class EntitiesToDxfExportLogic : IExportToFileLogic
    {
        private IGetDxfLayerLogic layerLogic = new GetDxfLayerLogic();
        public List<(EntityObject entity, LayerNames layerName)> Entities { get; set; } = [];

        public string FileName { get; set; }

        public void Export()
        {
            DxfDocument dxf = new DxfDocument();
            foreach (var shape in Entities)
            {
                Layer layer = layerLogic.GetOrCreateLayer(dxf, shape.layerName);
                var entity = shape.entity;
                entity.Layer = layer;
                dxf.Entities.Add(entity);
            }
            dxf.Save(FileName);
        }
    }
}
