using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Materials.Libraries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Loadermaterials = LoaderCalculator.Data.Materials;
using LoaderMaterialBuilders = LoaderCalculator.Data.Materials.MaterialBuilders;
using LoaderMaterialLogics = LoaderCalculator.Data.Materials.MaterialBuilders.MaterialLogics;

namespace StructureHelperLogics.Models.Materials
{
    public class ReinforcementLibMaterial : IReinforcementLibMaterial
    {
        public ILibMaterialEntity MaterialEntity { get; set; }
        public List<IMaterialSafetyFactor> SafetyFactors { get; }

        private IMaterialOptionLogic optionLogic;
        private LoaderMaterialLogics.ITrueStrengthLogic strengthLogic;

        public ReinforcementLibMaterial()
        {
            SafetyFactors = new List<IMaterialSafetyFactor>();
            optionLogic = new MaterialOptionLogic(new LoaderMaterialBuilders.ReinforcementOptions());
        }

        public object Clone()
        {
            return new ReinforcementLibMaterial() { MaterialEntity = MaterialEntity};
        }

        public Loadermaterials.IMaterial GetLoaderMaterial(LimitStates limitState, CalcTerms calcTerm)
        {
            var materialOptions = optionLogic.SetMaterialOptions(MaterialEntity, limitState, calcTerm);
            var strength = GetStrengthFactors(limitState, calcTerm);
            materialOptions.ExternalFactor.Compressive = strength.Compressive;
            materialOptions.ExternalFactor.Tensile = strength.Tensile;
            LoaderMaterialBuilders.IMaterialBuilder builder = new LoaderMaterialBuilders.ReinforcementBuilder(materialOptions);
            LoaderMaterialBuilders.IBuilderDirector director = new LoaderMaterialBuilders.BuilderDirector(builder);
            return director.BuildMaterial();
        }

        public (double Compressive, double Tensile) GetStrengthFactors(LimitStates limitState, CalcTerms calcTerm)
        {
            double compressionVal = 1d;
            double tensionVal = 1d;
            foreach (var item in SafetyFactors.Where(x => x.Take == true))
            {
                compressionVal *= item.GetFactor(StressStates.Compression, calcTerm, limitState);
                tensionVal *= item.GetFactor(StressStates.Tension, calcTerm, limitState);
            }
            return (compressionVal, tensionVal);
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
            var factors = GetStrengthFactors(limitState, calcTerm);
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
