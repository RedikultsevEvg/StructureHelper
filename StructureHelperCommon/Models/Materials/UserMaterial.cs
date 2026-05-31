using LoaderCalculator.Data.Materials;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Materials.Libraries;
using StructureHelperCommon.Models.Materials.Logics;
using System;
using System.Collections.Generic;

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

        public object Clone()
        {
            UserMaterial newItem = new(Guid.NewGuid());
            var updateStrategy = new UserMaterialUpdateStrategy();
            updateStrategy.Update(newItem, this);
            return newItem;
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
    }
}
