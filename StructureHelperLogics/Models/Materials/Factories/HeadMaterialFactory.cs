using StructureHelper.Models.Materials;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Settings;
using StructureHelperCommon.Models.Codes;
using StructureHelperCommon.Models.Materials.Libraries;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.Materials
{
    public enum HeadmaterialType
    {
        Concrete40,
        Reinforcement400,
        Reinforecement500,
        Elastic200,
        Carbon1400,
        Glass1200
    }     

    public static class HeadMaterialFactory
    {
        private static CodeTypes codeType;
        private static IEnumerable<ILibMaterialEntity> LibConcreteMaterials => LibMaterialPepository.GetConcreteRepository();
        private static IEnumerable<ILibMaterialEntity> LibReinforcementMaterials => LibMaterialPepository.GetReinforcementRepository();

        public static IHeadMaterial GetHeadMaterial(HeadmaterialType type)
        {
            if (type == HeadmaterialType.Concrete40) { return GetConcrete40(); }
            if (type == HeadmaterialType.Reinforcement400) { return GetReinforcement400(); }
            if (type == HeadmaterialType.Reinforecement500) { return GetReinforcement500(); }
            if (type == HeadmaterialType.Elastic200) { return GetElastic200(); }
            if (type == HeadmaterialType.Carbon1400) { return GetCarbon1400(); }
            if (type == HeadmaterialType.Glass1200) { return GetGlass1200(); }
            else throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknown + nameof(type));
        }

        private static IHeadMaterial GetReinforcement500()
        {
            var material = new HeadMaterial() { Name = "New reinforcement" };
            var libMaterial = LibReinforcementMaterials.Where(x => x.Name.Contains("500")).First();
            var libMat = new ReinforcementLibMaterial();
            libMat.MaterialEntity = libMaterial;
            material.HelperMaterial = libMat;
            return material;
        }

        private static IHeadMaterial GetElastic200()
        {
            var material = new HeadMaterial();
            material.HelperMaterial = new ElasticMaterial() { Modulus = 2e11d, CompressiveStrength = 4e8d, TensileStrength = 4e8d };
            return material;
        }

        private static IHeadMaterial GetCarbon1400()
        {
            var material = new HeadMaterial();
            material.HelperMaterial = new FRMaterial(MaterialTypes.CarbonFiber)
            {
                Modulus = 1.2e11d,
                CompressiveStrength = 0d,
                TensileStrength = 1.4e9d
            };
            return material;
        }

        private static IHeadMaterial GetGlass1200()
        {
            var material = new HeadMaterial();
            material.HelperMaterial = new FRMaterial(MaterialTypes.GlassFiber)
            {
                Modulus = 8e10d,
                CompressiveStrength = 1.2e9d,
                TensileStrength = 1.2e9d
            };
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
