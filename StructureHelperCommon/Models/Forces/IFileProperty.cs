using StructureHelperCommon.Infrastructures.Interfaces;
using System;

namespace StructureHelperCommon.Models.Forces
{
    public interface IFileProperty : ISaveable, ICloneable
    {
        string FilePath { get; set; }
    }
}