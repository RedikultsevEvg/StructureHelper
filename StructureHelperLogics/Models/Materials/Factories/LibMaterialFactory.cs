using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Strings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.Materials.Factories
{
    public static class LibMaterialFactory
    {
        public static List<ILibMaterial> GetLibMaterials(CodeTypes code)
        {
            List<ILibMaterial> libMaterials = new List<ILibMaterial>();
            libMaterials.AddRange(GetConcrete(code));
            libMaterials.AddRange(GetReinforcement(code));
            return libMaterials;
        }

        private static IEnumerable<ILibMaterial> GetReinforcement(CodeTypes code)
        {
            if (code == CodeTypes.EuroCode_2_1990)
            {
                return GetReinforcementEurocode();
            }
            else if (code == CodeTypes.SP63_13330_2018)
            {
                return GetReinforcementSP63();
            }
            else { throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknown); }
        }

        private static IEnumerable<ILibMaterial> GetConcrete(CodeTypes code)
        {
            if (code == CodeTypes.EuroCode_2_1990)
            {
                return GetConcreteEurocode();
            }
            else if (code == CodeTypes.SP63_13330_2018)
            {
                return GetConcreteSP63();
            }
            else { throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknown); }
        }

        private static IEnumerable<ILibMaterial> GetConcreteEurocode()
        {
            var code = CodeTypes.EuroCode_2_1990;
            var material = MaterialTypes.Concrete;
            List<ILibMaterial> libMaterials = new List<ILibMaterial>();
            libMaterials.Add(new LibMaterial(material, code, "C12", 12e6));
            libMaterials.Add(new LibMaterial(material, code, "C20", 20e6));
            libMaterials.Add(new LibMaterial(material, code, "C30", 30e6));
            libMaterials.Add(new LibMaterial(material, code, "C40", 40e6));
            libMaterials.Add(new LibMaterial(material, code, "C50", 50e6));
            libMaterials.Add(new LibMaterial(material, code, "C60", 60e6));
            libMaterials.Add(new LibMaterial(material, code, "C70", 70e6));
            libMaterials.Add(new LibMaterial(material, code, "C80", 80e6));
            return libMaterials;
        }

        private static IEnumerable<ILibMaterial> GetReinforcementEurocode()
        {
            var code = CodeTypes.EuroCode_2_1990;
            var material = MaterialTypes.Reinforcement;
            List<ILibMaterial> libMaterials = new List<ILibMaterial>();
            libMaterials.Add(new LibMaterial(material, code, "S240", 240e6));
            libMaterials.Add(new LibMaterial(material, code, "S400", 400e6));
            libMaterials.Add(new LibMaterial(material, code, "S500", 500e6));
            return libMaterials;
        }

        private static IEnumerable<ILibMaterial> GetConcreteSP63()
        {
            var code = CodeTypes.SP63_13330_2018;
            var material = MaterialTypes.Concrete;
            List<ILibMaterial> libMaterials = new List<ILibMaterial>();
            libMaterials.Add(new LibMaterial(material, code, "B5", 5e6));
            libMaterials.Add(new LibMaterial(material, code, "B7,5", 7.5e6));
            libMaterials.Add(new LibMaterial(material, code, "B10", 10e6));
            libMaterials.Add(new LibMaterial(material, code, "B15", 15e6));
            libMaterials.Add(new LibMaterial(material, code, "B20", 20e6));
            libMaterials.Add(new LibMaterial(material, code, "B25", 25e6));
            libMaterials.Add(new LibMaterial(material, code, "B30", 30e6));
            libMaterials.Add(new LibMaterial(material, code, "B35", 35e6));
            libMaterials.Add(new LibMaterial(material, code, "B40", 40e6));
            libMaterials.Add(new LibMaterial(material, code, "B50", 50e6));
            libMaterials.Add(new LibMaterial(material, code, "B60", 60e6));
            return libMaterials;
        }

        private static IEnumerable<ILibMaterial> GetReinforcementSP63()
        {
            var code = CodeTypes.EuroCode_2_1990;
            var material = MaterialTypes.Reinforcement;
            List<ILibMaterial> libMaterials = new List<ILibMaterial>();
            libMaterials.Add(new LibMaterial(material, code, "A240", 240e6));
            libMaterials.Add(new LibMaterial(material, code, "A400", 400e6));
            libMaterials.Add(new LibMaterial(material, code, "A500", 500e6));
            return libMaterials;
        }
    }
}
