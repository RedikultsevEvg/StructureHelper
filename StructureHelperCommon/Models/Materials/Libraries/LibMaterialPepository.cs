using System.Collections.Generic;
using System.Linq;
using StructureHelperCommon.Infrastructures.Enums;

namespace StructureHelperCommon.Models.Materials.Libraries
{
    public static class LibMaterialPepository
    {
        private static List<ILibMaterialEntity> libMaterials;
        public static List<ILibMaterialEntity> GetRepository()
        {
            if (libMaterials is null) { libMaterials = LibMaterialFactory.GetLibMaterials(); }

            return libMaterials;
        }

        public static IEnumerable<ILibMaterialEntity> GetConcreteRepository(CodeTypes code)
        {
            return GetRepository().Where(x => x.CodeType == code & x is IConcreteMaterialEntity); ;
        }

        public static IEnumerable<ILibMaterialEntity> GetReinforcementRepository(CodeTypes code)
        {
            return GetRepository().Where(x => x.CodeType == code & x is IReinforcementMaterialEntity);
        }
    }
}
