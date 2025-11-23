using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Forces;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperLogics.NdmCalculations.Analyses.Curvatures
{
    /// <summary>
    /// Implements properties for calculation deflections
    /// </summary>
    public interface IDeflectionFactor : ISaveable
    {
        /// <summary>
        /// Factors for calculating deflections, dimensionless
        /// </summary>
        IForceTuple DeflectionFactors { get; set; }
        /// <summary>
        /// Maximum deflections, metres
        /// </summary>
        double SpanLength { get; set; }
        IForceTuple MaxDeflections { get; set; }
    }
}
