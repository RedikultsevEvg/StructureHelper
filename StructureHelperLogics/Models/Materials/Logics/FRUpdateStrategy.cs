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
    internal class FRUpdateStrategy : IUpdateStrategy<IFRMaterial>
    {
        public void Update(IFRMaterial targetObject, IFRMaterial sourceObject)
        {
            CheckObject.ReferenceEquals(targetObject, sourceObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            targetObject.Modulus = sourceObject.Modulus;
            targetObject.CompressiveStrength = sourceObject.CompressiveStrength;
            targetObject.TensileStrength = sourceObject.TensileStrength;
            targetObject.ULSConcreteStrength = sourceObject.ULSConcreteStrength;
            targetObject.SumThickness = sourceObject.SumThickness;
        }
    }
}
