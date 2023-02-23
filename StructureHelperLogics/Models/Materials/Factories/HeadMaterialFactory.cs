using StructureHelper.Models.Materials;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Settings;
using StructureHelperCommon.Infrastructures.Strings;
using StructureHelperCommon.Models.Materials.Libraries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.Materials
{
    public enum HeadmaterialType
    {
        Concrete40,
        Reinforecement400,
        Elastic200
    }     

    public static class HeadMaterialFactory
    {
        private static CodeTypes codeType;
        private static IEnumerable<ILibMaterialEntity> LibConcreteMaterials => LibMaterialPepository.GetConcreteRepository(codeType);
        private static IEnumerable<ILibMaterialEntity> LibReinforcementMaterials => LibMaterialPepository.GetReinforcementRepository(codeType);

        public static IHeadMaterial GetHeadMaterial(HeadmaterialType type, CodeTypes code)
        {
            codeType = code;
            if (type == HeadmaterialType.Concrete40) { return GetConcrete40(); }
            if (type == HeadmaterialType.Reinforecement400) { return GetReinforcement400(); }
            if (type == HeadmaterialType.Elastic200) { return GetElastic200(); }
            else throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknown + nameof(type));
        }

        private static IHeadMaterial GetElastic200()
        {
            var material = new HeadMaterial();
            material.HelperMaterial = new ElasticMaterial() { Modulus = 2e11d, CompressiveStrength = 4e8d, TensileStrength = 4e8d };
            return material;
        }

        private static IHeadMaterial GetReinforcement400()
        {
            var material = new HeadMaterial() { Name = "New reinforcement" };
            var libMaterial = LibReinforcementMaterials.Where(x => x.Name.Contains("400")).First();
            var libMat = new ReinforcementLibMaterial();
            libMat.MaterialEntity = libMaterial;
            material.HelperMaterial = libMat;
            return material;
        }

        private static IHeadMaterial GetConcrete40()
        {
            var material = new HeadMaterial();
            var libMaterial = LibConcreteMaterials.Where(x => x.Name.Contains("40")).First();
            var libMat = new ConcreteLibMaterial();
            libMat.MaterialEntity = libMaterial;
            libMat.TensionForULS = false;
            libMat.TensionForSLS = true;
            material.HelperMaterial = libMat;
            return material;
        }
    }
}
