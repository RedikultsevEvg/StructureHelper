using StructureHelperCommon.Infrastructures.Interfaces;
using System;

namespace StructureHelperCommon.Models.Materials.Libraries
{
    public interface IPartialFactor : ISaveable, ICloneable
    {
        double FactorValue {get;set;}
    }
}
