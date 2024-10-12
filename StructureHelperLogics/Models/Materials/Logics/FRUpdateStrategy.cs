using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.Materials
{
    /// <inheritdoc/>
    public class FRUpdateStrategy : IUpdateStrategy<IFRMaterial>
    {
        /// <inheritdoc/>
        public void Update(IFRMaterial targetObject, IFRMaterial sourceObject)
        {
            CheckObject.IsNull(targetObject);
            CheckObject.IsNull(sourceObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            targetObject.Modulus = sourceObject.Modulus;
            targetObject.CompressiveStrength = sourceObject.CompressiveStrength;
            targetObject.TensileStrength = sourceObject.TensileStrength;
            targetObject.ULSConcreteStrength = sourceObject.ULSConcreteStrength;
            targetObject.SumThickness = sourceObject.SumThickness;
        }
    }
}
