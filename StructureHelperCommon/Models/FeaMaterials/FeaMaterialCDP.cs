using System;

namespace StructureHelperCommon.Models.FeaMaterials
{
    /// <inheritdoc/>
    public class FeaMaterialCDP : IFeaMaterialCDP
    {
        /// <inheritdoc/>
        public string Name { get; set; }
        /// <inheritdoc/>
        public double YoungModulus { get; set; }
        /// <inheritdoc/>
        public ICdpProperty CdpProperty { get; } = new CdpProperty(Guid.NewGuid());
        /// <inheritdoc/>
        public ICDPInelasticStrain CompressionStrain { get; set; } = new CDPInelasticStrain();
        /// <inheritdoc/>
        public ICDPInelasticStrain TensionStrain { get; set; } = new CDPInelasticStrain();
        /// <inheritdoc/>
        public double PoissonRatio { get; set; }
        /// <inheritdoc/>
        public double Density { get; set; } = 2400.0;
    }
}
