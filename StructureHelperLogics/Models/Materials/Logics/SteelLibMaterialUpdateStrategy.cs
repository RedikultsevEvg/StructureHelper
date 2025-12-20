using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Materials;
using StructureHelperCommon.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperLogics.Models.Materials
{
    public class SteelLibMaterialUpdateStrategy : IUpdateStrategy<ISteelLibMaterial>
    {
        private IUpdateStrategy<ILibMaterial> libUpdateStrategy;
        private IUpdateStrategy<ILibMaterial> LibUpdateStrategy => libUpdateStrategy ??= new LibMaterialUpdateStrategy();

        public SteelLibMaterialUpdateStrategy(IUpdateStrategy<ILibMaterial> libUpdateStrategy)
        {
            this.libUpdateStrategy = libUpdateStrategy;
        }

        public SteelLibMaterialUpdateStrategy()
        {
            
        }

        public void Update(ISteelLibMaterial targetObject, ISteelLibMaterial sourceObject)
        {
            CheckObject.ThrowIfNull(sourceObject);
            CheckObject.ThrowIfNull(targetObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            LibUpdateStrategy.Update(targetObject, sourceObject);
            targetObject.MaxPlasticStrainRatio = sourceObject.MaxPlasticStrainRatio;
            targetObject.UlsFactor = sourceObject.UlsFactor;
            targetObject.ThicknessFactor = sourceObject.ThicknessFactor;
            targetObject.WorkConditionFactor = sourceObject.WorkConditionFactor;
        }
    }
}
