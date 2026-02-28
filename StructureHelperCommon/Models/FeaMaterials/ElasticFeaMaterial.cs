using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperCommon.Models.FeaMaterials
{
    /// <inheritdoc/>
    public class ElasticFeaMaterial : IElasticFeaMaterial
    {
        /// <inheritdoc/>
        public Guid Id { get; }
        /// <inheritdoc/>
        public string Name { get; set; }
        /// <inheritdoc/>
        public double YoungsModulus { get; set; } = 2.0e11;
        /// <inheritdoc/>
        public double PoissonsRatio { get; set; } = 0.3;


        public ElasticFeaMaterial(Guid id)
        {
            Id = id;
        }
    }
}
