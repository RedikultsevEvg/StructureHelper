using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Materials;
using StructureHelperCommon.Models.Materials.Libraries;
using StructureHelperCommon.Services;

namespace DataAccess.DTOs
{
    public class HelperMaterialDTOSafetyFactorUpdateStrategy : IUpdateStrategy<IHelperMaterial>
    {
        private IUpdateStrategy<IMaterialSafetyFactor> safetyFactorUpdateStrategy;
        private IUpdateStrategy<IMaterialPartialFactor> partialFactorUpdateStrategy;
        private IMaterialSafetyFactorDTOLogic factorDTOLogic;

        public HelperMaterialDTOSafetyFactorUpdateStrategy(
            IUpdateStrategy<IMaterialSafetyFactor> safetyFactorUpdateStrategy,
            IUpdateStrategy<IMaterialPartialFactor> partialFactorUpdateStrategy)
        {
            this.safetyFactorUpdateStrategy = safetyFactorUpdateStrategy;
            this.partialFactorUpdateStrategy = partialFactorUpdateStrategy;
        }

        public HelperMaterialDTOSafetyFactorUpdateStrategy(IMaterialSafetyFactorDTOLogic factorDTOLogic) : this(
            new MaterialSafetyFactorBaseUpdateStrategy(),
            new MaterialPartialFactorUpdateStrategy())
        {
            this.factorDTOLogic = factorDTOLogic;
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
                    IMaterialSafetyFactor newSafetyFactor = GetNewSafetyFactorByOld(safetyFactor);
                    targetObject.SafetyFactors.Add(newSafetyFactor);
                }
            }
        }

        private IMaterialSafetyFactor GetNewSafetyFactorByOld(IMaterialSafetyFactor safetyFactor)
        {
            IMaterialSafetyFactor newSafetyFactor = factorDTOLogic.GetNewSafetyFactorByOld(safetyFactor);
            safetyFactorUpdateStrategy.Update(newSafetyFactor, safetyFactor);
            newSafetyFactor.PartialFactors.Clear();
            foreach (var partialFactor in safetyFactor.PartialFactors)
            {
                IMaterialPartialFactor newPartialFactor = GetNewPartialFactorByOld(partialFactor);
                newSafetyFactor.PartialFactors.Add(newPartialFactor);
            }

            return newSafetyFactor;
        }

        private IMaterialPartialFactor GetNewPartialFactorByOld(IMaterialPartialFactor partialFactor)
        {
            IMaterialPartialFactor newPartialFactor = factorDTOLogic.GetNewPartialFactorByOld(partialFactor);
            partialFactorUpdateStrategy.Update(newPartialFactor, partialFactor);
            return newPartialFactor;
        }
    }
}
