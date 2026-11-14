using LoaderCalculator.Data.Materials;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Materials.Libraries;
using StructureHelperCommon.Models.Materials.Logics;
using StructureHelperCommon.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace StructureHelperCommon.Models.Materials
{
    public class UserMaterial : IUserMaterial
    {
        public Guid Id { get; }
        public string? FilePath { get; set; } = string.Empty;
        public List<IMaterialSafetyFactor> SafetyFactors { get; set; } = [];

        public UserMaterial(Guid id)
        {
            Id = id;
        }

        public IMaterial GetCrackedLoaderMaterial(LimitStates limitState, CalcTerms calcTerm)
        {
            return GetLoaderMaterial(limitState, calcTerm);
        }

        public IMaterial GetLoaderMaterial(LimitStates limitState, CalcTerms calcTerm)
        {
            var logic = new GetUserMaterialPropertyFromFileLogic() { FilePath = FilePath};
            var property = logic.GetUserMaterialProperty();
            var factorsLogic = new MaterialFactorLogic(SafetyFactors);
            var factors = factorsLogic.GetTotalFactor(limitState, calcTerm);
            var convertLogic = new UserMaterialPropertyToLoaderMaterialConvertLogic()
            {
                PositiveStressFactor = factors.Tensile,
                NegativeStressFactor = factors.Compressive,
            };
            return convertLogic.Convert(property);
        }

        public IHelperMaterial GetClone(CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            if (cloneMode == CloneMode.Reference) { return this; }
            Guid id = GuidService.GetGuid(this, cloneMode);
            UserMaterial newItem = new(id);
            var updateStrategy = new UserMaterialUpdateStrategy() { UpdateChildren = false };
            updateStrategy.Update(newItem, this);
            newItem.SafetyFactors.Clear();
            foreach (var item in SafetyFactors)
            {
                var itemClone = item.GetClone(cloneMode);
                newItem.SafetyFactors.Add(itemClone);
            }
            return newItem;
        }
    }
}
