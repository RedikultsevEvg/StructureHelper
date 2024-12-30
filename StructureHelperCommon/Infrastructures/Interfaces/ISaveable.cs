using System;

namespace StructureHelperCommon.Infrastructures.Interfaces
{
    /// <summary>
    /// Interface for entities which able to save (has unique identifier)
    /// </summary>
    public interface ISaveable
    {
        /// <summary>
        /// Unique identifier
        /// </summary>
        Guid Id { get;}
    }
}
