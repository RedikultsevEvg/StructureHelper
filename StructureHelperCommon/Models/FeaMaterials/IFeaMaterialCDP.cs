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
        /// Young's modulus (modulus of elasticity) of concrete, Pa
        /// </summary>
        double YoungModulus { get; set; }
        /// <summary>
        /// Poisson's ratio, dimensionless
        /// </summary>
        double PoissonRatio { get; set; }
        /// <summary>
        /// Density of concrete
        /// </summary>
        double Density { get; set; }
        ICdpProperty CdpProperty { get;}
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
