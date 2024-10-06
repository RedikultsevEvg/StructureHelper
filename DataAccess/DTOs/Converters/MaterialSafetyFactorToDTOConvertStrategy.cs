using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Materials.Libraries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs
{
    public class MaterialSafetyFactorToDTOConvertStrategy : IConvertStrategy<MaterialSafetyFactorDTO, IMaterialSafetyFactor>
    {
        private IUpdateStrategy<IMaterialSafetyFactor> updateStrategy;
        public Dictionary<(Guid id, Type type), ISaveable> ReferenceDictionary { get; set; }
        public IShiftTraceLogger TraceLogger { get; set; }

        public MaterialSafetyFactorToDTOConvertStrategy(IUpdateStrategy<IMaterialSafetyFactor> updateStrategy)
        {
            this.updateStrategy = updateStrategy;
        }

        public MaterialSafetyFactorDTO Convert(IMaterialSafetyFactor source)
        {
            throw new NotImplementedException();
        }
    }
}
