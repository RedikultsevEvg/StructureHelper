using System;

namespace StructureHelperCommon.Models.FeaMaterials
{
    /// <inheritdoc/>
    public class ElasticFeaMaterial : IElasticFeaMaterial, ICloneable
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

        public object Clone()
        {
            var cloneStrategy = new ElasticFeaMaterialCloneStrategy();
            return cloneStrategy.GetClone(this);
        }
    }
}
