using System;

namespace StructureHelperCommon.Models.Forces
{
    /// <summary>
    /// Interface for generic force for beams
    /// </summary>
    public interface IForceTuple : ICloneable
    {
        /// <summary>
        /// Bending moment round about x-axis
        /// </summary>
        double Mx { get; set; }
        /// <summary>
        /// Bending moment round about y-axis
        /// </summary>
        double My { get; set; }
        /// <summary>
        /// Longitudinal force along x-axis
        /// </summary>
        double Nz { get; set; }
        /// <summary>
        /// Shear force along x-axis
        /// </summary>
        double Qx { get; set; }
        /// <summary>
        /// Shear force along z-axis
        /// </summary>
        double Qy { get; set; }
        /// <summary>
        /// Twisting moment round about z-axis
        /// </summary>
        double Mz { get; set; }
    }
}
