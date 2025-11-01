using netDxf;
using netDxf.Tables;

namespace StructureHelperCommon.Services.Exports
{
    public interface IGetDxfLayerLogic
    {
        AciColor GetLayerColor(LayerNames layerName);
        string GetLayerName(LayerNames layerName);
        Layer GetOrCreateLayer(DxfDocument dxf, LayerNames layerName);
    }
}