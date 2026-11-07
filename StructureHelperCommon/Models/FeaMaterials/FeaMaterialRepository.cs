using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;

namespace StructureHelperCommon.Models.FeaMaterials
{
    /// <inheritdoc/>
    public class FeaMaterialRepository : IFeaMaterialRepository
    {
        public IFeaMaterialRepository GetClone(IFeaMaterialRepository sourceObject, CloningMode cloningMode)
        {
            return new FeaMaterialRepositoryCloneStrategy().GetClone(sourceObject, cloningMode);
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
