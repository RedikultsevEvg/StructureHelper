using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Strings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Materials.Libraries
{
    public static class LibMaterialFactory
    {
        public static List<ILibMaterialEntity> GetLibMaterials()
        {
            List<ILibMaterialEntity> libMaterials = new List<ILibMaterialEntity>();
            libMaterials.AddRange(GetConcreteEurocode());
            libMaterials.AddRange(GetConcreteSP63());
            libMaterials.AddRange(GetReinforcementEurocode());
            libMaterials.AddRange(GetReinforcementSP63());
            return libMaterials;
        }

        private static IEnumerable<ILibMaterialEntity> GetConcreteEurocode()
        {
            var code = CodeTypes.EuroCode_2_1990;
            List<ILibMaterialEntity> libMaterials = new List<ILibMaterialEntity>();
            libMaterials.Add(new ConcreteMaterialEntity() { CodeType = code, Name = "C12",  MainStrength = 12e6 });
            libMaterials.Add(new ConcreteMaterialEntity() { CodeType = code, Name = "C20", MainStrength = 20e6 });
            libMaterials.Add(new ConcreteMaterialEntity() { CodeType = code, Name = "C30", MainStrength = 30e6 });
            libMaterials.Add(new ConcreteMaterialEntity() { CodeType = code, Name = "C40", MainStrength = 40e6 });
            libMaterials.Add(new ConcreteMaterialEntity() { CodeType = code, Name = "C50", MainStrength = 50e6 });
            libMaterials.Add(new ConcreteMaterialEntity() { CodeType = code, Name = "C60", MainStrength = 60e6 });
            libMaterials.Add(new ConcreteMaterialEntity() { CodeType = code, Name = "C70", MainStrength = 70e6 });
            libMaterials.Add(new ConcreteMaterialEntity() { CodeType = code, Name = "C80", MainStrength = 80e6 });
            return libMaterials;
        }
        private static IEnumerable<ILibMaterialEntity> GetReinforcementEurocode()
        {
            var code = CodeTypes.EuroCode_2_1990;
            List<ILibMaterialEntity> libMaterials = new List<ILibMaterialEntity>();
            libMaterials.Add(new ReinforcementMaterialEntity() { CodeType = code, Name = "S240", MainStrength = 240e6 });
            libMaterials.Add(new ReinforcementMaterialEntity() { CodeType = code, Name = "S400", MainStrength = 400e6 });
            libMaterials.Add(new ReinforcementMaterialEntity() { CodeType = code, Name = "S500", MainStrength = 500e6 });
            return libMaterials;
        }
        private static IEnumerable<ILibMaterialEntity> GetConcreteSP63()
        {
            var code = CodeTypes.SP63_13330_2018;
            List<ILibMaterialEntity> libMaterials = new List<ILibMaterialEntity>();
            libMaterials.Add(new ConcreteMaterialEntity() { CodeType = code, Name = "B5", MainStrength = 5e6 });
            libMaterials.Add(new ConcreteMaterialEntity() { CodeType = code, Name = "B7,5", MainStrength = 7.5e6 });
            libMaterials.Add(new ConcreteMaterialEntity() { CodeType = code, Name = "B10", MainStrength = 10e6 });
            libMaterials.Add(new ConcreteMaterialEntity() { CodeType = code, Name = "B15", MainStrength = 15e6 });
            libMaterials.Add(new ConcreteMaterialEntity() { CodeType = code, Name = "B20", MainStrength = 20e6 });
            libMaterials.Add(new ConcreteMaterialEntity() { CodeType = code, Name = "B25", MainStrength = 25e6 });
            libMaterials.Add(new ConcreteMaterialEntity() { CodeType = code, Name = "B30", MainStrength = 30e6 });
            libMaterials.Add(new ConcreteMaterialEntity() { CodeType = code, Name = "B35", MainStrength = 35e6 });
            libMaterials.Add(new ConcreteMaterialEntity() { CodeType = code, Name = "B40", MainStrength = 40e6 });
            libMaterials.Add(new ConcreteMaterialEntity() { CodeType = code, Name = "B50", MainStrength = 50e6 });
            libMaterials.Add(new ConcreteMaterialEntity() { CodeType = code, Name = "B60", MainStrength = 60e6 });
            return libMaterials;
        }
        private static IEnumerable<ILibMaterialEntity> GetReinforcementSP63()
        {
            var code = CodeTypes.SP63_13330_2018;
            List<ILibMaterialEntity> libMaterials = new List<ILibMaterialEntity>();
            libMaterials.Add(new ReinforcementMaterialEntity() { CodeType = code, Name = "A240", MainStrength = 240e6 });
            libMaterials.Add(new ReinforcementMaterialEntity() { CodeType = code, Name = "A400", MainStrength = 400e6 });
            libMaterials.Add(new ReinforcementMaterialEntity() { CodeType = code, Name = "A500", MainStrength = 500e6 });
            return libMaterials;
        }
    }
}
