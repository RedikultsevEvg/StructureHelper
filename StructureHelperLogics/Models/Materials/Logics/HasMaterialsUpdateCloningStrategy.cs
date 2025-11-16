using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.Materials.Logics
{
    /// <summary>
    /// Creates deep copy of internal elements of object which has materials
    /// </summary>
    public class HasMaterialsUpdateCloningStrategy : IUpdateStrategy<IHasHeadMaterials>
    {
        private ICloningStrategy cloningStrategy;

        public HasMaterialsUpdateCloningStrategy(ICloningStrategy cloningStrategy)
        {
            this.cloningStrategy = cloningStrategy;
        }

        /// <inheritdoc/>
        public void Update(IHasHeadMaterials targetObject, IHasHeadMaterials sourceObject)
        {
            CheckObject.ThrowIfNull(cloningStrategy);
            CheckObject.ThrowIfNull(sourceObject);
            CheckObject.ThrowIfNull(targetObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            targetObject.HeadMaterials.Clear();
            foreach (var material in sourceObject.HeadMaterials)
            {
                var newMaterial = cloningStrategy.Clone(material);
                targetObject.HeadMaterials.Add(newMaterial);
            }
        }
    }
}
