using StructureHelperCommon.Models.Materials.Libraries;
using System.Collections.Generic;

namespace StructureHelperLogics.Models.Materials
{
    public interface IHasSafetyFactors
    {
        List<IMaterialSafetyFactor> SafetyFactors { get; set; }
    }
}