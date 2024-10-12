﻿using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Materials.Libraries;
using StructureHelperCommon.Services;
using StructureHelperLogics.Models.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs.Converters
{
    public class HelperMaterialDTOSafetyFactorUpdateStrategy : IUpdateStrategy<IHelperMaterial>
    {
        private IUpdateStrategy<IMaterialSafetyFactor> safetyFactorUpdateStrategy;
        private IUpdateStrategy<IMaterialPartialFactor> partialFactorUpdateStrategy;

        public HelperMaterialDTOSafetyFactorUpdateStrategy(
            IUpdateStrategy<IMaterialSafetyFactor> safetyFactorUpdateStrategy,
            IUpdateStrategy<IMaterialPartialFactor> partialFactorUpdateStrategy)
        {
            this.safetyFactorUpdateStrategy = safetyFactorUpdateStrategy;
            this.partialFactorUpdateStrategy = partialFactorUpdateStrategy;
        }

        public HelperMaterialDTOSafetyFactorUpdateStrategy() : this(
            new MaterialSafetyFactorUpdateStrategy(),
            new MaterialPartialFactorUpdateStrategy())
        {
            
        }

        public void Update(IHelperMaterial targetObject, IHelperMaterial sourceObject)
        {
            CheckObject.IsNull(sourceObject);
            CheckObject.IsNull(targetObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            if (sourceObject.SafetyFactors is not null)
            {
                targetObject.SafetyFactors.Clear();
                foreach (var safetyFactor in sourceObject.SafetyFactors)
                {
                    MaterialSafetyFactorDTO newSafetyFactor = GetNewSafetyFactorByOld(safetyFactor);
                    targetObject.SafetyFactors.Add(newSafetyFactor);
                }
            }
        }

        private MaterialSafetyFactorDTO GetNewSafetyFactorByOld(IMaterialSafetyFactor safetyFactor)
        {
            MaterialSafetyFactorDTO newSafetyFactor = new()
            {
                Id = safetyFactor.Id
            };
            safetyFactorUpdateStrategy.Update(newSafetyFactor, safetyFactor);
            newSafetyFactor.PartialFactors.Clear();
            foreach (var partialFactor in safetyFactor.PartialFactors)
            {
                MaterialPartialFactorDTO newPartialFactor = GetNewPartialFactorByOld(partialFactor);
                newSafetyFactor.PartialFactors.Add(newPartialFactor);
            }

            return newSafetyFactor;
        }

        private MaterialPartialFactorDTO GetNewPartialFactorByOld(IMaterialPartialFactor partialFactor)
        {
            MaterialPartialFactorDTO newPartialFactor = new()
            {
                Id = partialFactor.Id
            };
            partialFactorUpdateStrategy.Update(newPartialFactor, partialFactor);
            return newPartialFactor;
        }
    }
}
