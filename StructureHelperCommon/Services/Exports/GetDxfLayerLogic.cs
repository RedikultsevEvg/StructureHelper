using netDxf;
using netDxf.Tables;
using StructureHelperCommon.Infrastructures.Exceptions;

namespace StructureHelperCommon.Services.Exports
{
    public class GetDxfLayerLogic : IGetDxfLayerLogic
    {
        private const string PrimitivesLayer = "STR-PRM";
        private const string OpeningsLayer = "STR-OPN";
        private const string RebarLayer = "STR-REB";
        private const string PointLayer = "STR-PNT";
        private const string LayerNameIsUnKnown = ": Layer name is unknown";
        public Layer GetOrCreateLayer(DxfDocument dxf, LayerNames layerName)
        {
            string newLayerName = GetLayerName(layerName);
            Layer newLayer = dxf.Layers.Contains(newLayerName)
                ? dxf.Layers[newLayerName]
                : new Layer(newLayerName)
                {
                    Color = GetLayerColor(layerName)
                };

            if (!dxf.Layers.Contains(newLayerName))
                dxf.Layers.Add(newLayer);
            return newLayer;
        }
        public string GetLayerName(LayerNames layerName)
        {
            if (layerName == LayerNames.StructiralPrimitives) { return PrimitivesLayer; }
            else if (layerName == LayerNames.StructuralOpenings) { return OpeningsLayer; }
            else if (layerName == LayerNames.StructuralRebars) { return RebarLayer; }
            else if (layerName == LayerNames.StructuralPoints) { return PointLayer; }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(layerName) + LayerNameIsUnKnown);
            }
        }

        public AciColor GetLayerColor(LayerNames layerName)
        {
            if (layerName == LayerNames.StructiralPrimitives) { return AciColor.Blue; }
            else if (layerName == LayerNames.StructuralOpenings) { return AciColor.DarkGray; }
            else if (layerName == LayerNames.StructuralRebars) { return AciColor.Magenta; }
            else if (layerName == LayerNames.StructuralPoints) { return AciColor.Green; }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(layerName) + LayerNameIsUnKnown);
            }
        }
    }
}
