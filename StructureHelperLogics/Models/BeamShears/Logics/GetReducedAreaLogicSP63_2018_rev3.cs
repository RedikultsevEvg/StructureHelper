using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Loggers;

namespace StructureHelperLogics.Models.BeamShears
{
    public class GetReducedAreaLogicSP63_2018_rev3 : IGetReducedAreaLogic
    {
        private const double epsilon_b0 = 0.002;
        private const double epsilon_bt0 = 0.0001;

        private double longitudinalForce;
        private IInclinedSection inclinedSection;
        private (double Compressive, double Tensile) concreteStrength;
        private double reinforcementModulus;
        private double concreteModulus;
        private double reducingFactor;
        private double concreteArea;
        private double reinforcementArea;

        public IShiftTraceLogger? TraceLogger { get; set; }
        public GetReducedAreaLogicSP63_2018_rev3(double longitudinalForce, IInclinedSection inclinedSection, IShiftTraceLogger? traceLogger)
        {
            this.longitudinalForce = longitudinalForce;
            this.inclinedSection = inclinedSection;
            TraceLogger = traceLogger;
        }

        public double GetArea()
        {
            TraceLogger?.AddMessage(LoggerStrings.LogicType(this), TraceLogStatuses.Service);
            TraceLogger?.AddMessage($"Calculting of reduced area of cross-section according to SP 63.13330.2018 rev.3");
            GetSuplimentaryParemeters();
            double area = GetReducedArea();
            return area;
        }

        private double GetReducedArea()
        {
            if (longitudinalForce <= 0)
            {
                TraceLogger?.AddMessage($"Longitudinal force Nz = {longitudinalForce} is negative, section under compression");
                return GetNegForceArea();
            }
            else
            {
                TraceLogger?.AddMessage($"Longitudinal force Nz = {longitudinalForce} is positive, section under tension");
                return GetPosForceArea();
            }
        }

        private void GetSuplimentaryParemeters()
        {
            GetConcreteModulus();
            GetReinforcementModulus();
            GetConcreteStrength();
            GetAreas();
        }

        private double GetPosForceArea()
        {
            double tensileStrength = concreteStrength.Tensile;
            TraceLogger?.AddMessage($"Modulus of elasticity of reinforcement Es = {reinforcementModulus }(Pa)");
            TraceLogger?.AddMessage($"Modulus of elasticity of concrete Eb = {concreteModulus }(Pa)");
            TraceLogger?.AddMessage($"Ratio of modulus of elasticity alpha = {reducingFactor }(dimensionless)");
            TraceLogger?.AddMessage($"Tensile strength of concrete Rbt = {tensileStrength }(Pa)");
            TraceLogger?.AddMessage($"Reference strain of concrete Epsilonbt0 = {epsilon_bt0 }(dimensionless)");
            double factorOfInelasticity = tensileStrength / (epsilon_bt0 * concreteModulus);
            TraceLogger?.AddMessage($"Factor of inelastisity nu = Rbt / (Epsilon_bt0 * Eb) = {tensileStrength } / ({epsilon_bt0} * {concreteModulus}) = {factorOfInelasticity}(dimensionless)");
            double factor = reducingFactor / factorOfInelasticity;
            double area = concreteArea + reinforcementArea * factor;
            TraceLogger?.AddMessage($"Reduced area Ared = {concreteArea} + {reducingFactor} / {factorOfInelasticity} * {reinforcementArea} = {area}(m^2)");
            return area;
        }

        private double GetNegForceArea()
        {
            double compressiveStrength = concreteStrength.Compressive;
            TraceLogger?.AddMessage($"Modulus of elasticity of reinforcement Es = {reinforcementModulus }(Pa)");
            TraceLogger?.AddMessage($"Modulus of elasticity of concrete Eb = {concreteModulus }(Pa)");
            TraceLogger?.AddMessage($"Ratio of modulus of elasticity alpha = {reducingFactor }(dimensionless)");
            TraceLogger?.AddMessage($"Compressive strength of concrete Rb = {compressiveStrength }(Pa)");
            TraceLogger?.AddMessage($"Reference strain of concrete Epsilonb0 = {epsilon_b0 }(dimensionless)");
            double factorOfInelasticity = compressiveStrength / (epsilon_b0 * concreteModulus);
            TraceLogger?.AddMessage($"Factor of inelastisity nu = Rb / (Epsilon_b0 * Eb) = {compressiveStrength } / ({epsilon_b0} * {concreteModulus}) = {factorOfInelasticity}(dimensionless)");
            double factor = reducingFactor / factorOfInelasticity;
            double area = concreteArea + reinforcementArea * factor;
            TraceLogger?.AddMessage($"Reduced area Ared = {concreteArea} + {reducingFactor} / {factorOfInelasticity} * {reinforcementArea} = {area}(m^2)");
            return area;
        }

        private void GetAreas()
        {
            reducingFactor = reinforcementModulus / concreteModulus;
            concreteArea = inclinedSection.WebWidth * inclinedSection.FullDepth;
            TraceLogger?.AddMessage($"Concrete area Ac = {concreteArea}(m^2)");
            reinforcementArea = inclinedSection.BeamShearSection.ReinforcementArea;
            TraceLogger?.AddMessage($"Reinforcement area As = {reinforcementArea}(m^2)");
        }

        private void GetConcreteModulus()
        {
            var concreteMaterial = inclinedSection.BeamShearSection.ConcreteMaterial;
            var loaderMaterial = concreteMaterial.GetLoaderMaterial(inclinedSection.LimitState, inclinedSection.CalcTerm);
            concreteModulus = loaderMaterial.InitModulus;
        }

        private void GetReinforcementModulus()
        {
            var reinforcementMaterial = inclinedSection.BeamShearSection.ReinforcementMaterial;
            var loaderMaterial = reinforcementMaterial.GetLoaderMaterial(inclinedSection.LimitState, inclinedSection.CalcTerm);
            reinforcementModulus = loaderMaterial.InitModulus;
        }

        private void GetConcreteStrength()
        {
            concreteStrength = inclinedSection.BeamShearSection.ConcreteMaterial.GetStrength(inclinedSection.LimitState, inclinedSection.CalcTerm);
        }
    }
}
