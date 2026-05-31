using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperCommon.Models.Materials
{
    public class UserMaterialProperty : IUserMaterialProperty
    {
        public double YoungModulus { get; set; }
        public double LimitPositiveStrain { get; set; }
        public double LimitNegativeStrain { get; set; }

        public List<IStressStrainTuple> StressStrainPairs { get; } = [];
    }
}
