using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperCommon.Models.FeaMaterials
{
    public class ConcreteFeaTensionUpdateStrategy : IUpdateStrategy<IConcreteFeaTension>
    {
        public void Update(IConcreteFeaTension targetObject, IConcreteFeaTension sourceObject)
        {
            CheckObject.ThrowIfNull(sourceObject, ErrorStrings.SourceObject);
            CheckObject.ThrowIfNull(targetObject, ErrorStrings.TargetObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            targetObject.Strength = sourceObject.Strength;
            targetObject.FractureEnergy = sourceObject.FractureEnergy;
            targetObject.FeSize = sourceObject.FeSize;
        }
    }
}
