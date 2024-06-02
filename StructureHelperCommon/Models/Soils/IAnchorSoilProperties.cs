using StructureHelperCommon.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Soils
{
    public enum SoilType
    {
        SandAndSemiSand,
        SemiClay,
        Clay
    }
    /// <summary>
    /// Properties of soil for calculating of soil anchor
    /// </summary>
    public interface IAnchorSoilProperties : ISaveable
    {
        /// <summary>
        /// Young's modulus, Pa
        /// </summary>
        double YoungsModulus { get; set; }
        /// <summary>
        /// Poison ratio
        /// </summary>
        double PoissonRatio { get; set; }
        /// <summary>
        /// Volumetric weight, N/m^3
        /// </summary>
        double VolumetricWeight { get; set; }
        /// <summary>
        /// Angle of internal friction, degree
        /// </summary>
        double FrictionAngle { get; set; }
        /// <summary>
        /// Coheasion, Pa
        /// </summary>
        double Coheasion { get; set; }
        /// <summary>
        /// Type of Soil
        /// </summary>
        SoilType SoilType { get; set; }
    }
}
