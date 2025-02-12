using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.Materials.Logics
{
    internal class FunctionUpdateStrategy : IUpdateStrategy<IFunctionMaterial>
    {
        public void Update(IFunctionMaterial targetObject, IFunctionMaterial sourceObject)
        {
            CheckObject.CompareTypes(targetObject, sourceObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }

            /*targetObject.Modulus = sourceObject.Modulus;
            targetObject.CompressiveStrength = sourceObject.CompressiveStrength;
            targetObject.TensileStrength = sourceObject.TensileStrength;*/ //from elastic
        }
    }
}
