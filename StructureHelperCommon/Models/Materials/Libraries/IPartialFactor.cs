using StructureHelperCommon.Infrastructures.Interfaces;
using System;

namespace StructureHelperCommon.Models.Materials.Libraries
{
    public interface IPartialFactor : ISaveable, ICloneStrategy<IPartialFactor>
    {
        double FactorValue {get;set;}
    }
}
