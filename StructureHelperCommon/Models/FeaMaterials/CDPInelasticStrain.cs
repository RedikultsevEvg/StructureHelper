using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperCommon.Models.FeaMaterials
{
    /// <inheritdoc/>
    public class CDPInelasticStrain : ICDPInelasticStrain
    {
        /// <inheritdoc/>
        public List<double> InelasticStrainList { get; } = [];
        /// <inheritdoc/>
        public List<double> StressList { get; } = [];
        /// <inheritdoc/>
        public List<double> DamageList { get; } = [];
        /// <inheritdoc/>
        public List<double> PlasticStrainList { get; } = [];

        public List<double> ElasticStrainList { get; } = [];
    }
}
