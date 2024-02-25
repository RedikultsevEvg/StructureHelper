using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Forces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Sections
{
    /// <summary>
    /// Logic for calculating of value of accidental eccentricity
    /// </summary>
    public interface IAccidentalEccentricityLogic : ILogic
    {
        /// <summary>
        /// Properties of compressed member
        /// </summary>
        ICompressedMember CompressedMember {get;set;}
        /// <summary>
        /// Size of cross-section along X-axis, m
        /// </summary>
        double SizeX { get; set; }
        /// <summary>
        /// Size of cross-section along Y-axis, m
        /// </summary>
        double SizeY { get; set; }
        /// <summary>
        /// Initial tuple of force
        /// </summary>
        IForceTuple InitialForceTuple { get; set; }
        /// <summary>
        /// Returns new force tuple with accidental eccentricity
        /// </summary>
        /// <returns></returns>
        ForceTuple GetForceTuple();
    }
}
