using netDxf.Entities;
using System.Linq;

namespace StructureHelperCommon.Services.Exports
{
    public class SinglePolyline2DImportFromDxfLogic : IImportFromFileLogic
    {
        public string FileName { get; set; }
        public Polyline2D Polyline2D { get; private set; }
        public void Import()
        {
            var importLogic = new ShapesAllImportFromDxfLogic() { FileName = FileName};
            importLogic.Import();
            var entities = importLogic.Entities;
            Polyline2D = entities.Single(x => x is Polyline2D) as Polyline2D;
        }
    }
}
