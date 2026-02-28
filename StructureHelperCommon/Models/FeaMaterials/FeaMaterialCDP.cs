namespace StructureHelperCommon.Models.FeaMaterials
{
    /// <inheritdoc/>
    public class FeaMaterialCDP : IFeaMaterialCDP
    {
        /// <inheritdoc/>
        public string Name { get; set; }
        /// <inheritdoc/>
        public double YoungsModulus { get; set; }
        /// <inheritdoc/>
        public double DilationAngle { get; set; } = 35;
        /// <inheritdoc/>
        public ICDPInelasticStrain CompressionStrain { get; set; } = new CDPInelasticStrain();
        /// <inheritdoc/>
        public ICDPInelasticStrain TensionStrain { get; set; } = new CDPInelasticStrain();

    }
}
