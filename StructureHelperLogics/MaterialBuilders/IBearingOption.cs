using LoaderCalculator.Data.Materials.MaterialBuilders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.MaterialBuilders
{
    internal interface IBearingOption : IMaterialOption
    {
        double InitModulus { get; set; }
        double Strength { get; set; }
        LimitStates LimitState { get; set; }
        bool IsShortTerm { get; set; }
        CodesType CodesType { get; set; }
        IPartialFactor ExternalFactor { get; }
    }
}
