using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.Materials.Libraries;

namespace StructureHelperLogics.Models.Materials
{
    public enum ConcreteLibTypes
    {
        Concrete15,
        Concrete20,
        Concrete25,
        Concrete30,
        Concrete35,
        Concrete40,
    }
    public static class ConcreteLibMaterialFactory
    {
        private static IEnumerable<ILibMaterialEntity> LibConcreteMaterials => LibMaterialPepository.GetConcreteRepository();
        public static IConcreteLibMaterial GetConcreteLibMaterial(ConcreteLibTypes concreteLibTypes)
        {
            if (concreteLibTypes == ConcreteLibTypes.Concrete25)
            {
                return GetConcrete(25);
            }
            if (concreteLibTypes == ConcreteLibTypes.Concrete40)
            {
                return GetConcrete(40);
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(concreteLibTypes));
            }
        }

        private static IConcreteLibMaterial GetConcrete(double strength)
        {
            string stringStrength = Convert.ToString(strength);
            var libMaterial = LibConcreteMaterials
                .Where(x => x.Name.Contains(stringStrength))
                .First();
            var libMat = new ConcreteLibMaterial
            {
                MaterialEntity = libMaterial,
                TensionForULS = false,
                TensionForSLS = true
            };
            return libMat;
        }
    }
}
