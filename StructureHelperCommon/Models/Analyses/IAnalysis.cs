using StructureHelperCommon.Infrastructures.Interfaces;
using System;
using System.Windows.Media;

namespace StructureHelperCommon.Models.Analyses
{
    /// <summary>
    /// Implements propertis of analysis
    /// </summary>
    public interface IAnalysis : ISaveable, ICloneStrategy<IAnalysis>
    {
        string Name { get; set; }
        string Tags { get; set; }
        string Comment { get; set; }
        Color Color { get; set; }
        /// <summary>
        /// Processor of subversions of analysis
        /// </summary>
        IVersionProcessor VersionProcessor { get; set; }
    }
}
