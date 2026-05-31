using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperCommon.Models.Materials
{
    public interface IUserMaterialProperty
    {
        double YoungModulus { get; set; }
        double LimitPositiveStrain { get; set; }
        double LimitNegativeStrain { get; set; }

        List<IStressStrainTuple> StressStrainPairs { get; }
    }
}
