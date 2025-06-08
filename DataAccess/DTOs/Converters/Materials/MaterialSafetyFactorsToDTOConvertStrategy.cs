using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Materials.Libraries;

namespace DataAccess.DTOs
{
    public class MaterialSafetyFactorsToDTOConvertStrategy : IConvertStrategy<MaterialSafetyFactorDTO, IMaterialSafetyFactor>
    {
        private IUpdateStrategy<IMaterialSafetyFactor> updateStrategy;
        public Dictionary<(Guid id, Type type), ISaveable> ReferenceDictionary { get; set; }
        public IShiftTraceLogger TraceLogger { get; set; }

        public MaterialSafetyFactorsToDTOConvertStrategy(IUpdateStrategy<IMaterialSafetyFactor> updateStrategy)
        {
            this.updateStrategy = updateStrategy;
        }

        public MaterialSafetyFactorDTO Convert(IMaterialSafetyFactor source)
        {
            throw new NotImplementedException();
        }
    }
}
