using netDxf;
using netDxf.Entities;
using netDxf.Header;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Services.Exports
{
    public class ShapesAllImportFromDxfLogic : IImportFromFileLogic
    {
        public string FileName { get; set; }
        public List<EntityObject> Entities { get; set; } = [];
        public void Import()
        {
            // this check is optional but recommended before loading a DXF file
            DxfVersion dxfVersion = DxfDocument.CheckDxfFileVersion(FileName);
            // netDxf is only compatible with AutoCad2000 and higher DXF versions
            if (dxfVersion < DxfVersion.AutoCad2000) return;
            // load file
            DxfDocument dxf = DxfDocument.Load(FileName);
            Entities.Clear();
            if (dxf != null)
            {
                Entities.AddRange(dxf.Entities.All);
            }
        }
    }
}
