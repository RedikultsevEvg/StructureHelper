using LoaderCalculator.Data.Materials;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Infrastructures.Settings;
using StructureHelperCommon.Models.Materials.Libraries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.Materials
{
    public class FRMaterial : IFRMaterial
    {
        private IElasticMaterialLogic elasticMaterialLogic => new ElasticMaterialLogic();
        private MaterialTypes materialType;
        IUpdateStrategy<IFRMaterial> fRUpdateStrategy = new FRUpdateStrategy();
        public double Modulus{ get; set; }
        public double CompressiveStrength { get; set; }
        public double TensileStrength { get; set; }

        public List<IMaterialSafetyFactor> SafetyFactors { get; }
        public double ULSConcreteStrength { get; set; }
        public double SumThickness { get; set; }
        public double GammaF2 => GetGammaF2();

        private double GetGammaF2()
        {
            const double gammaF2Max = 0.9d;
            double gammaF2;
            IFactorLogic factorLogic = new FactorLogic(SafetyFactors);
            var factors = factorLogic.GetTotalFactor(LimitStates.ULS, CalcTerms.ShortTerm);
            var rf = TensileStrength * factors.Tensile;
            var epsUlt = rf / Modulus;
            gammaF2 = 0.4d / epsUlt * Math.Sqrt(ULSConcreteStrength / (Modulus * SumThickness * 1e3d));
            gammaF2 = Math.Min(gammaF2, gammaF2Max);
            return gammaF2;
        }

        public FRMaterial(MaterialTypes materialType)
        {

            ULSConcreteStrength = 14e6d;
            SumThickness = 0.175e-3d;
            SafetyFactors = new List<IMaterialSafetyFactor>();
            this.materialType = materialType;
            SafetyFactors.AddRange(PartialCoefficientFactory.GetDefaultFRSafetyFactors(ProgramSetting.FRCodeType, this.materialType));
        }

        public object Clone()
        {
            var newItem = new FRMaterial(this.materialType);
            var updateStrategy = fRUpdateStrategy;
            updateStrategy.Update(newItem, this);
            return newItem;
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
    }
}
