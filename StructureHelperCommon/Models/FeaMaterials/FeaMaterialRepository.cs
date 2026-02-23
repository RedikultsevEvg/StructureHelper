using System;
using System.Collections.Generic;

namespace StructureHelperCommon.Models.FeaMaterials
{
    /// <inheritdoc/>
    public class FeaMaterialRepository : IFeaMaterialRepository
    {
        /// <inheritdoc/>
        public Guid Id { get; }
        /// <inheritdoc/>
        public List<IFeaMaterial> FeaMaterials { get; } = [];

        public FeaMaterialRepository(Guid id)
        {
            Id = id;
        }
    }
}
