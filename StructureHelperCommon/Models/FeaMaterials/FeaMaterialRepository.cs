using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;

namespace StructureHelperCommon.Models.FeaMaterials
{
    /// <inheritdoc/>
    public class FeaMaterialRepository : IFeaMaterialRepository
    {

        public IFeaMaterialRepository GetClone(IFeaMaterialRepository sourceObject, CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            return new FeaMaterialRepositoryCloneStrategy().GetClone(sourceObject, cloneMode);
        }

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
