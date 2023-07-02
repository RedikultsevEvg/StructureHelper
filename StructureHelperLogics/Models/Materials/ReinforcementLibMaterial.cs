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


namespace StructureHelperLogics.Models.Materials
{
    public class ReinforcementLibMaterial : IReinforcementLibMaterial
    {
        private LMBuilders.ReinforcementOptions lmOptions;
        private IMaterialOptionLogic optionLogic;
        private IFactorLogic factorLogic => new FactorLogic(SafetyFactors);
        private LoaderMaterialLogics.ITrueStrengthLogic strengthLogic;
        public ILibMaterialEntity MaterialEntity { get; set; }
        public List<IMaterialSafetyFactor> SafetyFactors { get; }


        public ReinforcementLibMaterial()
        {
            SafetyFactors = new List<IMaterialSafetyFactor>();
            lmOptions = new LMBuilders.ReinforcementOptions();
        }

        public object Clone()
        {
            var newItem = new ReinforcementLibMaterial();
            var updateStrategy = new ReinforcementLibUpdateStrategy();
            updateStrategy.Update(newItem, this);
            return newItem;
        }

        public Loadermaterials.IMaterial GetLoaderMaterial(LimitStates limitState, CalcTerms calcTerm)
        {
            optionLogic = new MaterialCommonOptionLogic(MaterialEntity, limitState, calcTerm);
            optionLogic.SetMaterialOptions(lmOptions);
            var factors = factorLogic.GetTotalFactor(limitState, calcTerm);
            lmOptions.ExternalFactor.Compressive = factors.Compressive;
            lmOptions.ExternalFactor.Tensile = factors.Tensile;
            LMBuilders.IMaterialBuilder builder = new LMBuilders.ReinforcementBuilder(lmOptions);
            LMBuilders.IBuilderDirector director = new LMBuilders.BuilderDirector(builder);
            return director.BuildMaterial();
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
    }
}
