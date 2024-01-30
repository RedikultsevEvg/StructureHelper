using System;

namespace StructureHelperCommon.Infrastructures.Interfaces
{
    public interface ISaveable
    {
        /// <summary>
        /// Unique identifier
        /// </summary>
        Guid Id { get;}
        //void Save();
    }
}
