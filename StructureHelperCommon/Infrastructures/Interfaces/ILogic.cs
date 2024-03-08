using StructureHelperCommon.Models;

namespace StructureHelperCommon.Infrastructures.Interfaces
{
    /// <summary>
    /// Base interface for logic
    /// </summary>
    public interface ILogic
    {
        /// <summary>
        /// Logger for tracing of actions
        /// </summary>
        IShiftTraceLogger? TraceLogger { get; set; }
    }
}
