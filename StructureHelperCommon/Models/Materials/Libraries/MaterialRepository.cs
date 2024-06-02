using StructureHelperCommon.Infrastructures.Settings;
using StructureHelperCommon.Models.Codes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Materials.Libraries
{
    public class MaterialRepository : IMaterialRepository
    {
        public List<ILibMaterialEntity> Repository { get; }

        public MaterialRepository(IEnumerable<ICodeEntity> codes)
        {
            Repository = LibMaterialFactory.GetLibMaterials()
                .Where(x => codes.Contains(x.Code))
                .ToList();
        }
    }
}
