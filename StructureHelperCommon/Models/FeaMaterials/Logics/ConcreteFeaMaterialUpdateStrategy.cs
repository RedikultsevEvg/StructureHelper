using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperCommon.Models.FeaMaterials
{
    public class ConcreteFeaMaterialUpdateStrategy : IParentUpdateStrategy<IConcreteFeaMaterial>
    {
        public bool UpdateChildren { get; set; } = true;

        public void Update(IConcreteFeaMaterial targetObject, IConcreteFeaMaterial sourceObject)
        {
            CheckObject.ThrowIfNull(sourceObject, ErrorStrings.SourceObject);
            CheckObject.ThrowIfNull(targetObject, ErrorStrings.TargetObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            targetObject.Name = sourceObject.Name;
            targetObject.YoungsModulus = sourceObject.YoungsModulus;
            targetObject.PoissonsRatio = sourceObject.PoissonsRatio;
            if (UpdateChildren == true)
            {
                CheckObject.ThrowIfNull(sourceObject.CompressionProperties);
                CheckObject.ThrowIfNull(sourceObject.TensionProperties);
                CheckObject.ThrowIfNull(targetObject.CompressionProperties);
                CheckObject.ThrowIfNull(targetObject.TensionProperties);
            }
        }
    }
}
