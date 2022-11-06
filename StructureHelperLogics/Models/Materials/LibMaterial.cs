using LoaderCalculator.Data.Materials;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Strings;
using StructureHelperCommon.Models.Materials;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperLogics.Models.Materials
{
    public class LibMaterial : ILibMaterial
    {
        public MaterialTypes MaterialType { get; set; }
        public CodeTypes CodeType { get; set; }
        public string Name { get; set; }
        public double MainStrength { get; set; }

        public LibMaterial(MaterialTypes materialType, CodeTypes codeType, string name, double mainStrength)
        {
            MaterialType = materialType;
            CodeType = codeType;
            Name = name;
            MainStrength = mainStrength;
        }

        public IPrimitiveMaterial GetPrimitiveMaterial()
        {
            if (MaterialType == MaterialTypes.Concrete & CodeType == CodeTypes.EuroCode_2_1990)
            {   return GetConcreteEurocode();}
            else if (MaterialType == MaterialTypes.Reinforcement & CodeType == CodeTypes.EuroCode_2_1990)
            {   return GetReinfrocementeEurocode();}
            if (MaterialType == MaterialTypes.Concrete & CodeType == CodeTypes.SP63_13330_2018)
            {   return GetConcreteSP63(); }
            else if (MaterialType == MaterialTypes.Reinforcement & CodeType == CodeTypes.SP63_13330_2018)
            {   return GetReinfrocementeSP63(); }
            else throw new StructureHelperException($"{ErrorStrings.ObjectTypeIsUnknown}: material type = {MaterialType}, code type = {CodeType}");
        }

        private IPrimitiveMaterial GetReinfrocementeSP63()
        {
            IPrimitiveMaterial primitiveMaterial = new PrimitiveMaterial
            { MaterialType = MaterialType, CodeType = CodeTypes.SP63_13330_2018, ClassName = $"Reinforcement {Name}", Strength = MainStrength };
            return primitiveMaterial;
        }

        private IPrimitiveMaterial GetConcreteSP63()
        {
            IPrimitiveMaterial primitiveMaterial = new PrimitiveMaterial
            { MaterialType = MaterialType, CodeType = CodeTypes.SP63_13330_2018, ClassName = $"Concrete {Name}", Strength = MainStrength };
            return primitiveMaterial;
        }

        private IPrimitiveMaterial GetReinfrocementeEurocode()
        {
            IPrimitiveMaterial primitiveMaterial = new PrimitiveMaterial
            { MaterialType = MaterialType, CodeType = CodeTypes.EuroCode_2_1990, ClassName = $"Reinforcement {Name}", Strength = MainStrength };
            return primitiveMaterial;
        }

        private IPrimitiveMaterial GetConcreteEurocode()
        {
            IPrimitiveMaterial primitiveMaterial = new PrimitiveMaterial
            { MaterialType = MaterialType, CodeType = CodeTypes.EuroCode_2_1990, ClassName = $"Concrete {Name}", Strength = MainStrength };
            return primitiveMaterial;
        }

        public object Clone()
        {
            return new LibMaterial(this.MaterialType, this.CodeType, this.Name, this.MainStrength);
        }
    }
}
