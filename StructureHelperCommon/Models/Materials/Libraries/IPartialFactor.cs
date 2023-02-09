using System;

namespace StructureHelperCommon.Models.Materials.Libraries
{
    public interface IPartialFactor : ICloneable
    {
        double FactorValue {get;set;}
    }
}
