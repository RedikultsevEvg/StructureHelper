using StructureHelperCommon.Infrastructures.Interfaces;

namespace StructureHelperCommon.Models.Materials.Logics
{
    /// <summary>
    /// Checks the tension branch of a custom concrete diagram.
    /// Strength and strain values must be positive; the tension limit strain may be infinite.
    /// </summary>
    public class CustomConcreteTensionOptionsCheckLogic
        : CheckEntityLogic<ICustomConcreteTensionOptions>
    {
        public override bool Check()
        {
            CheckResult = string.Empty;
            if (Entity is null)
            {
                TraceMessage("Tension options are not assigned");
                return false;
            }

            bool result = true;
            if (!IsPositiveFinite(Entity.Strength))
            {
                TraceMessage($"Tension strength must be a positive finite value, but was {Entity.Strength}");
                result = false;
            }
            if (!IsPositiveFinite(Entity.UltimateStrain))
            {
                TraceMessage($"Ultimate tension strain must be a positive finite value, but was {Entity.UltimateStrain}");
                result = false;
            }
            if (!IsPositive(Entity.LimitStrain))
            {
                TraceMessage($"Tension limit strain must be a positive value, but was {Entity.LimitStrain}");
                result = false;
            }
            return result;
        }

        private static bool IsPositiveFinite(double value)
            => !double.IsNaN(value) && !double.IsInfinity(value) && value > 0d;

        private static bool IsPositive(double value)
            => !double.IsNaN(value) && value > 0d;
    }
}
