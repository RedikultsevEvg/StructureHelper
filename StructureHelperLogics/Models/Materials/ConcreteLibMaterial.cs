using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Settings;
using StructureHelperCommon.Models.Materials.Libraries;
using LMBuilders = LoaderCalculator.Data.Materials.MaterialBuilders;
using LMLogic = LoaderCalculator.Data.Materials.MaterialBuilders.MaterialLogics;
using LM = LoaderCalculator.Data.Materials;

namespace StructureHelperLogics.Models.Materials
{
    public class ConcreteLibMaterial : IConcreteLibMaterial
    {
        private LMBuilders.ConcreteOptions lmOptions;
        private IMaterialOptionLogic optionLogic;
        private IFactorLogic factorLogic => new FactorLogic(SafetyFactors);
        private LMLogic.ITrueStrengthLogic strengthLogic;
        public ILibMaterialEntity MaterialEntity { get; set; }
        public List<IMaterialSafetyFactor> SafetyFactors { get; }
        public bool TensionForULS { get ; set; }
        public bool TensionForSLS { get; set; }
        public double Humidity { get; set; }


        public ConcreteLibMaterial()
        {
            SafetyFactors = new List<IMaterialSafetyFactor>();
            lmOptions = new LMBuilders.ConcreteOptions();
            SafetyFactors.AddRange(PartialCoefficientFactory.GetDefaultConcreteSafetyFactors(ProgramSetting.CodeType));
            TensionForULS = false;
            TensionForSLS = true;
            Humidity = 0.55d;
        }       

        public object Clone()
        {
            return new ConcreteLibMaterial() { MaterialEntity = MaterialEntity, TensionForULS = TensionForULS, TensionForSLS = TensionForSLS };
        }

        public LM.IMaterial GetLoaderMaterial(LimitStates limitState, CalcTerms calcTerm)
        {
            optionLogic = new MaterialCommonOptionLogic(MaterialEntity, limitState, calcTerm);
            optionLogic.SetMaterialOptions(lmOptions);
            optionLogic = new ConcreteMaterialOptionLogic(this, limitState);
            optionLogic.SetMaterialOptions(lmOptions);
            var strength = factorLogic.GetTotalFactor(limitState, calcTerm);
            lmOptions.ExternalFactor.Compressive = strength.Compressive;
            lmOptions.ExternalFactor.Tensile = strength.Tensile;
            LMBuilders.IMaterialBuilder builder = new LMBuilders.ConcreteBuilder(lmOptions);
            LMBuilders.IBuilderDirector director = new LMBuilders.BuilderDirector(builder);
            return director.BuildMaterial();
        }

        public (double Compressive, double Tensile) GetStrength(LimitStates limitState, CalcTerms calcTerm)
        {
            strengthLogic = new LMLogic.TrueStrengthConcreteLogicSP63_2018(MaterialEntity.MainStrength);
            var strength = strengthLogic.GetTrueStrength();
            double compressionFactor = 1d;
            double tensionFactor = 1d;
            if (limitState == LimitStates.ULS)
            {
                compressionFactor /= 1.3d;
                tensionFactor /= 1.5d;
                var factors = factorLogic.GetTotalFactor(limitState, calcTerm);
                compressionFactor *= factors.Compressive;
                tensionFactor *= factors.Tensile;
            }
            return (strength.Compressive * compressionFactor, strength.Tensile * tensionFactor);
        }
    }
}
