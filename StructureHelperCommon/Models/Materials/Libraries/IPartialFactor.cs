using StructureHelperCommon.Infrastructures.Interfaces;
using System;

namespace StructureHelperCommon.Models.Materials.Libraries
{
    public interface IPartialFactor : ISaveable, ISelfCloneStrategy<IPartialFactor>
    {
        double FactorValue {get;set;}
    }
}
