using StructureHelperCommon.Infrastructures.Interfaces;
using System;

namespace StructureHelperCommon.Models.Forces
{
    public interface IAction : ISaveable, ICloneable
    {
        string Name { get; set; }
    }
}
