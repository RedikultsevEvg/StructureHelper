using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperCommon.Models.FeaMaterials
{
    /// <summary>
    /// Properties of concrete for Abaqus CDP material model
    /// </summary>
    public interface IFeaMaterialCDP
    {
        string Name { get; set; }
        /// <summary>
        /// Young's moduus (modulus of elasticity) of concrete, Pa
        /// </summary>
        double YoungsModulus { get; set; }
        /// <summary>
        /// Angle of dilatancy, degree
        /// </summary>
        double DilationAngle { get; set; }
        /// <summary>
        /// Properties of concrete in compression
        /// </summary>
        ICDPInelasticStrain CompressionStrain { get; }
        /// <summary>
        /// Properties of concrete in tension
        /// </summary>
        ICDPInelasticStrain TensionStrain { get; }
    }
}
