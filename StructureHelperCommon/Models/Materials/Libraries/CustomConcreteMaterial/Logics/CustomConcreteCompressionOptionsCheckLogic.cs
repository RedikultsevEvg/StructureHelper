using StructureHelperCommon.Infrastructures.Interfaces;

namespace StructureHelperCommon.Models.Materials.Logics
{
    /// <summary>
    /// Checks the compression branch of a custom concrete diagram.
    /// All stresses and strains must be positive finite values.
    /// </summary>
    public class CustomConcreteCompressionOptionsCheckLogic
        : CheckEntityLogic<ICustomConcreteCompressionOptions>
    {
        public override bool Check()
        {
            CheckResult = string.Empty;
            if (Entity is null)
            {
                TraceMessage("Compression options are not assigned");
                return false;
            }

            bool result = true;
            if (!IsPositiveFinite(Entity.PeakStress))
            {
                TraceMessage($"Peak stress must be a positive finite value, but was {Entity.PeakStress}");
                result = false;
            }
            if (!IsPositiveFinite(Entity.PeakStrain))
            {
                TraceMessage($"Peak strain must be a positive finite value, but was {Entity.PeakStrain}");
                result = false;
            }
            if (!IsPositiveFinite(Entity.LimitStrain))
            {
                TraceMessage($"Limit strain must be a positive finite value, but was {Entity.LimitStrain}");
                result = false;
            }
            if (!IsPositiveFinite(Entity.DescendingScaleFactor))
            {
                TraceMessage($"Descending scale factor must be a positive finite value, but was {Entity.DescendingScaleFactor}");
                result = false;
            }
            if (IsPositiveFinite(Entity.PeakStrain)
                && IsPositiveFinite(Entity.LimitStrain)
                && Entity.LimitStrain <= Entity.PeakStrain)
            {
                TraceMessage("Compression limit strain must be greater than peak strain");
                result = false;
            }
            return result;
        }

        private static bool IsPositiveFinite(double value)
            => !double.IsNaN(value) && !double.IsInfinity(value) && value > 0d;
    }
}
