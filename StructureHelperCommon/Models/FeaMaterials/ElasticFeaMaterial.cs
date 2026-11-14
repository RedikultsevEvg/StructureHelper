using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Services;
using System;

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
        public double YoungModulus { get; set; } = 2.0e11;
        /// <inheritdoc/>
        public double PoissonRatio { get; set; } = 0.3;


        public ElasticFeaMaterial(Guid id)
        {
            Id = id;
        }

        public IElasticFeaMaterial GetClone(IElasticFeaMaterial sourceObject, CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            var cloneStrategy = new ElasticFeaMaterialCloneStrategy();
            return cloneStrategy.GetClone(this, cloneMode);
        }
    }
}
