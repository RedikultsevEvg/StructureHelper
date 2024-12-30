using System.Collections.Generic;

//Copyright (c) 2025 Redikultsev Evgeny, Ekaterinburg, Russia
//All rights reserved.

namespace StructureHelperCommon.Models
{
    /// <summary>
    /// Creates collection of entries for trace logger of some entity
    /// </summary>
    public interface ITraceEntityLogic
    {
        /// <summary>
        /// Default priority for created trace logger entries
        /// </summary>
        int Priority { get; set; }
        /// <summary>
        /// Returns list of entries
        /// </summary>
        /// <returns>List of entries</returns>
        List<ITraceLoggerEntry> GetTraceEntries();
        /// <summary>
        /// Creates new collection of entries and adds it into trace logger
        /// </summary>
        /// <param name="traceLogger"></param>
        void AddEntriesToTraceLogger(IShiftTraceLogger traceLogger);
    }
}