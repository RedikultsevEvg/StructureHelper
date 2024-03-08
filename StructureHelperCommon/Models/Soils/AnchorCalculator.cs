using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Soils
{
    public class AnchorCalculator : ICalculator
    {
        private AnchorResult result;

        public string Name { get; set; }

        public IAnchorSoilProperties Soil { get; set; }
        public SoilAnchor Anchor { get; set; }

        public IResult Result => result;

        public Action<IResult> ActionToOutputResults { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IShiftTraceLogger? TraceLogger { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public AnchorCalculator(SoilAnchor soilAnchor, IAnchorSoilProperties anchorSoilProperties)
        {
            Anchor = soilAnchor;
            Soil = anchorSoilProperties;
        }

        public object Clone()
        {
            throw new NotImplementedException();
        }

        public void Run()
        {
            const double ratioFactor = 1.01d;

            CheckParameters();

            result = new() { IsValid = true };

            var alpha = Math.PI / 180d * Anchor.AngleToHorizont;
            var fi = Math.PI / 180d * Soil.FrictionAngle;
            var sinFi = Math.Sin(fi);

            double ksi0 = GetKsi();

            var hk = Anchor.GroundLevel - Anchor.HeadLevel;
            hk += Anchor.FreeLength * Math.Sin(alpha);
            result.AnchorCenterLevel = hk;

            var decrement = Math.Sqrt(Math.Pow(Math.Cos(alpha), 2) + Math.Pow(ksi0 * Math.Sin(alpha), 2));
            var sigma0g = 0.5d * (Soil.VolumetricWeight * hk + Anchor.AdditionalSurfPressure) * (ksi0 + decrement);
            result.AverageSidePressure = sigma0g;

            var overA1 = (1 + Soil.PoissonRatio) * (sigma0g + Soil.Coheasion / Math.Tan(fi)) * sinFi;
            var a1 = Soil.YoungsModulus / overA1;

            double kp = GetKp(ratioFactor, sinFi, a1);

            var charRak = Math.PI * Anchor.RootDiameter * Anchor.RootLength;
            charRak *= 1 + sinFi;
            charRak *= sigma0g * Math.Tan(fi) + Soil.Coheasion;
            charRak *= kp * GetGammaC();

            result.CharBearingCapacity = charRak;
            result.DesignBearingCapacity = charRak / GetGammaN();

            result.MortarVolumeFstStady = 0.5d * (Anchor.RootDiameter * Anchor.RootDiameter - Anchor.JetTubeDiameter * Anchor.JetTubeDiameter) * (1 + 3.1 * Anchor.WaterCementRatio) * Anchor.RootLength;
            result.MortarVolumeSndStady = 0.5d * (Anchor.RootDiameter * Anchor.RootDiameter - Anchor.BoreHoleDiameter * Anchor.BoreHoleDiameter) * (1 + 3.1d * Anchor.WaterCementRatio) * Anchor.RootLength;
        }

        private void CheckParameters()
        {
            if (Anchor.RootDiameter < Anchor.BoreHoleDiameter)
            {
                throw new StructureHelperException(ErrorStrings.DataIsInCorrect + $": Diameter of root {Anchor.RootDiameter}m must be greater than diameter of borehole {Anchor.BoreHoleDiameter}m");
            }
        }

        private double GetKp(double ratioFactor, double sinFi, double a1)
        {
            var tetta = sinFi / (1 + sinFi);
            var kp = ratioFactor - Math.Pow(Anchor.BoreHoleDiameter / Anchor.RootDiameter, 2);
            kp /= ratioFactor - a1 * a1 / (1 + a1 * a1);
            kp = Math.Pow(kp, tetta);
            return kp;
        }

        private double GetKsi()
        {
            if (Soil.SoilType == SoilType.SandAndSemiSand) { return 0.43d; }
            else if (Soil.SoilType == SoilType.SemiClay) { return 0.55d; }
            else if (Soil.SoilType == SoilType.Clay) { return 0.72d; }
            else { throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknown); }
        }

        private double GetGammaC()
        {
            if (Soil.SoilType == SoilType.SandAndSemiSand) { return 0.72d; }
            else if (Soil.SoilType == SoilType.SemiClay) { return 0.64d; }
            else if (Soil.SoilType == SoilType.Clay) { return 0.64; }
            else { throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknown + $": {Soil.SoilType}"); }
        }

        private double GetGammaN()
        {
            if (Anchor.DurabilityType == DurabilityType.Temporary) { return 1.2d; }
            else if (Anchor.DurabilityType == DurabilityType.Eturnal) { return 1.4d; }
            else { throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknown + $": {Anchor.DurabilityType}"); }
        }
    }
}
