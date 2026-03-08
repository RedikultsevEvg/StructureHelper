using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperCommon.Models.FeaMaterials
{
    /// <summary>
    /// Concrete mate
    /// </summary>
    public interface IConcreteFeaMaterial : IFeaMaterial
    {
        /// <summary>
        /// Young's moduus (modulus of elasticity) of concrete, Pa
        /// </summary>
        double YoungModulus {  get; set; }
        /// <summary>
        /// Poison's ration, dimensionless
        /// </summary>
        double PoissonRatio { get; set; }
        ICdpProperty CdpProperty { get; }
        /// <summary>
        /// Propertis of concrete on compression
        /// </summary>
        IConcreteFeaCompression CompressionProperties { get; }
        /// <summary>
        /// Properties of concrete in tension
        /// </summary>
        IConcreteFeaTension TensionProperties { get; }
    }
}
