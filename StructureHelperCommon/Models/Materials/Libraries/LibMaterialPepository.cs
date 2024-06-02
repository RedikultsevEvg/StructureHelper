using System.Collections.Generic;
using System.Linq;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Settings;

namespace StructureHelperCommon.Models.Materials.Libraries
{
    public static class LibMaterialPepository
    {
        private static List<ILibMaterialEntity> libMaterials;
        public static List<ILibMaterialEntity> GetRepository()
        {
            libMaterials = LibMaterialFactory.GetLibMaterials();
            //if (libMaterials is null)
            //{
            //}
            return libMaterials;
        }

        public static List<ILibMaterialEntity> GetConcreteRepository()
        {
            var natCodes = ProgramSetting.CodesList;
            var rep = GetRepository();
            var repository = rep
                .Where(x => natCodes.Contains(x.Code) & x is IConcreteMaterialEntity)
                .ToList();
            return repository;
        }

        public static List<ILibMaterialEntity> GetReinforcementRepository()
        {
            var natCodes = ProgramSetting.CodesList;
            var rep = GetRepository();
            var repository = rep
                .Where(x => natCodes.Contains(x.Code) & x is IReinforcementMaterialEntity)
                .ToList();
            return repository;
        }
    }
}
