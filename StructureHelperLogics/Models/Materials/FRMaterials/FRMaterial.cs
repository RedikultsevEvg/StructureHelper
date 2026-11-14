using LoaderCalculator.Data.Materials;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Settings;
using StructureHelperCommon.Models.Materials;
using StructureHelperCommon.Models.Materials.Libraries;
using StructureHelperCommon.Services;

namespace StructureHelperLogics.Models.Materials
{
    public class FRMaterial : IFRMaterial
    {
        private IElasticMaterialLogic elasticMaterialLogic => new ElasticMaterialLogic();
        IUpdateStrategy<IFRMaterial> updateStrategy = new FRUpdateStrategy();
        public Guid Id { get; }
        public MaterialTypes MaterialType { get; }
        public double Modulus{ get; set; }
        public double CompressiveStrength { get; set; }
        public double TensileStrength { get; set; }

        public List<IMaterialSafetyFactor> SafetyFactors { get; set; } = new();
        public double ULSConcreteStrength { get; set; }
        public double SumThickness { get; set; }
        public double GammaF2 => GetGammaF2();

        public FRMaterial(Guid id)
            : this(id, MaterialTypes.CarbonFiber)
        {
        }

        public FRMaterial(Guid id, MaterialTypes materialType)
        {
            if (materialType != MaterialTypes.CarbonFiber &
                    materialType != MaterialTypes.GlassFiber)
            {
                throw new StructureHelperException(ErrorStrings.DataIsInCorrect + $": Material type {materialType} is not right");
            }
            Id = id;
            ULSConcreteStrength = 14e6d;
            SumThickness = 0.175e-3d;
            MaterialType = materialType;
            SafetyFactors.AddRange(PartialFactorsFactory.GetDefaultFRSafetyFactors(ProgramSetting.FRCodeType, this.MaterialType));
        }

        public FRMaterial(MaterialTypes materialType) : this (Guid.NewGuid(), materialType)
        {
            
        }

        public IMaterial GetLoaderMaterial(LimitStates limitState, CalcTerms calcTerm)
        {
            double factor = 1d;
            if (limitState == LimitStates.ULS)
                {
                    factor = GetGammaF2();
                }
            var material = elasticMaterialLogic.GetLoaderMaterial(this, limitState, calcTerm, factor);
            return material;
        }

        public IMaterial GetCrackedLoaderMaterial(LimitStates limitState, CalcTerms calcTerm)
        {
            return GetLoaderMaterial(limitState, calcTerm);
        }
        private double GetGammaF2()
        {
            const double gammaF2Max = 0.9d;
            double gammaF2;
            IMaterialFactorLogic factorLogic = new MaterialFactorLogic(SafetyFactors);
            var factors = factorLogic.GetTotalFactor(LimitStates.ULS, CalcTerms.ShortTerm);
            var rf = TensileStrength * factors.Tensile;
            var epsUlt = rf / Modulus;
            gammaF2 = 0.4d / epsUlt * Math.Sqrt(ULSConcreteStrength / (Modulus * SumThickness * 1e3d));
            gammaF2 = Math.Min(gammaF2, gammaF2Max);
            return gammaF2;
        }

        public IHelperMaterial GetClone(IHelperMaterial sourceObject, CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            return CloneFallback.GetClone(sourceObject, cloneMode, cloneDepth);
        }

        public IHelperMaterial GetClone(CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            if (cloneMode == CloneMode.Reference) { return this; }
            Guid id = GuidService.GetGuid(this, cloneMode);
            var newItem = new FRMaterial(id, MaterialType);
            updateStrategy.Update(newItem, this);
            return newItem;
        }
    }
}
