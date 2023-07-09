﻿using StructureHelperCommon.Infrastructures.Interfaces;
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
            targetObject.Modulus = sourceObject.Modulus;
            targetObject.CompressiveStrength = sourceObject.CompressiveStrength;
            targetObject.TensileStrength = targetObject.TensileStrength;
            targetObject.ULSConcreteStrength = targetObject.ULSConcreteStrength;
            targetObject.SumThickness = targetObject.SumThickness;
        }
    }
}