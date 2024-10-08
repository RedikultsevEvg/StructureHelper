using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Materials.Libraries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Loadermaterials = LoaderCalculator.Data.Materials;
using LMBuilders = LoaderCalculator.Data.Materials.MaterialBuilders;
using LoaderMaterialLogics = LoaderCalculator.Data.Materials.MaterialBuilders.MaterialLogics;
using LoaderCalculator.Data.Materials;
using StructureHelperCommon.Models.Materials;
using StructureHelperCommon.Infrastructures.Settings;

namespace StructureHelperLogics.Models.Materials
{
    public class ReinforcementLibMaterial : IReinforcementLibMaterial
    {
        const MaterialTypes materialType = MaterialTypes.Reinforcement;

        private IFactorLogic factorLogic => new FactorLogic(SafetyFactors);
        private LoaderMaterialLogics.ITrueStrengthLogic strengthLogic;
        private readonly List<IMaterialLogic> materialLogics;

        public Guid Id { get; }
        public ILibMaterialEntity MaterialEntity { get; set; }
        public List<IMaterialSafetyFactor> SafetyFactors { get; set; } = new();
        public IMaterialLogic MaterialLogic { get; set; }
        public List<IMaterialLogic> MaterialLogics => materialLogics;


        public ReinforcementLibMaterial(Guid id)
        {
            Id = id;
            materialLogics = ProgramSetting.MaterialLogics.Where(x => x.MaterialType == materialType).ToList();
            MaterialLogic = materialLogics.First();
        }

        public ReinforcementLibMaterial() : this (Guid.NewGuid())
        {
            
        }

        public object Clone()
        {
            var newItem = new ReinforcementLibMaterial();
            var updateStrategy = new ReinforcementLibUpdateStrategy();
            updateStrategy.Update(newItem, this);
            return newItem;
        }

        public IMaterial GetLoaderMaterial(LimitStates limitState, CalcTerms calcTerm)
        {
            ReinforcementLogicOptions options = SetOptions(limitState, calcTerm);
            MaterialLogic.Options = options;
            var material = MaterialLogic.GetLoaderMaterial();
            return material;
        }

        private ReinforcementLogicOptions SetOptions(LimitStates limitState, CalcTerms calcTerm)
        {
            var options = new ReinforcementLogicOptions();
            options.SafetyFactors = SafetyFactors;
            options.MaterialEntity = MaterialEntity;
            options.LimitState = limitState;
            options.CalcTerm = calcTerm;
            return options;
        }

        public (double Compressive, double Tensile) GetStrength(LimitStates limitState, CalcTerms calcTerm)
        {
            strengthLogic = new LoaderMaterialLogics.TrueStrengthReinforcementLogic(MaterialEntity.MainStrength);
            var strength = strengthLogic.GetTrueStrength();
            double compressionFactor = 1d;
            double tensionFactor = 1d;
            if (limitState == LimitStates.ULS)
            {
                compressionFactor /= 1.15d;
                tensionFactor /= 1.15d;
            }
            var factors = factorLogic.GetTotalFactor(limitState, calcTerm);
            compressionFactor *= factors.Compressive;
            tensionFactor *= factors.Tensile;
            var compressiveStrength = strength.Compressive * compressionFactor;
            if (limitState == LimitStates.ULS)
            {
                if (calcTerm == CalcTerms.ShortTerm)
                {
                    compressiveStrength = Math.Min(4e8, compressiveStrength);
                }
                else
                {
                    compressiveStrength = Math.Min(5e8, compressiveStrength);
                }
            }
            var tensileStrength = strength.Tensile * tensionFactor;
            return (compressiveStrength, tensileStrength);
        }

        public IMaterial GetCrackedLoaderMaterial(LimitStates limitState, CalcTerms calcTerm)
        {
            return GetLoaderMaterial(limitState, calcTerm);
        }
    }
}
