using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperCommon.Models.FeaMaterials
{
    public class FeaMaterialRepositoryUpdateStrategy : IParentUpdateStrategy<IFeaMaterialRepository>
    {
        private ICloneStrategy<IFeaMaterial> cloneStrategy;
        private ICloneStrategy<IFeaMaterial> CloneStrategy => cloneStrategy ??= new FeaMaterialCloneStrategy();
        public bool UpdateChildren { get; set; } = true;

        public void Update(IFeaMaterialRepository targetObject, IFeaMaterialRepository sourceObject)
        {
            CheckObject.ThrowIfNull(sourceObject, ErrorStrings.SourceObject);
            CheckObject.ThrowIfNull(targetObject, ErrorStrings.TargetObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            CheckObject.ThrowIfNull(sourceObject.FeaMaterials);
            CheckObject.ThrowIfNull(targetObject.FeaMaterials);
            if (UpdateChildren == true)
            {
                targetObject.FeaMaterials.Clear();
                foreach (var item in sourceObject.FeaMaterials)
                {
                    targetObject.FeaMaterials.Add(CloneStrategy.GetClone(item));
                }
            }
        }
    }
}
