using StructureHelperCommon.Infrastructures.Interfaces;

namespace StructureHelperCommon.Models.Materials.Logics
{
    /// <summary>
    /// Checks a single custom concrete model, including its compression and tension options.
    /// </summary>
    public class CustomConcreteModelCheckLogic : CheckEntityLogic<ICustomConcreteModel>
    {
        private CustomConcreteCompressionOptionsCheckLogic compressionCheckLogic;
        private CustomConcreteTensionOptionsCheckLogic tensionCheckLogic;

        private CustomConcreteCompressionOptionsCheckLogic CompressionCheckLogic =>
            compressionCheckLogic ??= new CustomConcreteCompressionOptionsCheckLogic();

        private CustomConcreteTensionOptionsCheckLogic TensionCheckLogic =>
            tensionCheckLogic ??= new CustomConcreteTensionOptionsCheckLogic();

        public override bool Check()
        {
            CheckResult = string.Empty;
            if (Entity is null)
            {
                TraceMessage("Custom concrete model is not assigned");
                return false;
            }

            bool result = true;
            if (!IsPositiveFinite(Entity.YoungModulus))
            {
                TraceMessage($"Young modulus must be a positive finite value, but was {Entity.YoungModulus}");
                result = false;
            }
            if (Entity.StateCalcTermPair is null)
            {
                TraceMessage("Limit state and calculation term are not assigned");
                result = false;
            }

            if (Entity.CompressionOptions is null)
            {
                TraceMessage("Compression options are not assigned");
                result = false;
            }
            else
            {
                CompressionCheckLogic.Entity = Entity.CompressionOptions;
                CompressionCheckLogic.TraceLogger = TraceLogger;
                CompressionCheckLogic.YoungsModulus = Entity.YoungModulus;
                if (!compressionCheckLogic.Check())
                {
                    TraceMessage(compressionCheckLogic.CheckResult.Trim());
                    result = false;
                }
            }

            if (Entity.TensionOptions is null)
            {
                TraceMessage("Tension options are not assigned");
                result = false;
            }
            else
            {
                tensionCheckLogic ??= new CustomConcreteTensionOptionsCheckLogic();
                tensionCheckLogic.Entity = Entity.TensionOptions;
                tensionCheckLogic.TraceLogger = TraceLogger;
                if (!tensionCheckLogic.Check())
                {
                    TraceMessage(tensionCheckLogic.CheckResult.Trim());
                    result = false;
                }
            }

            return result;
        }

        private static bool IsPositiveFinite(double value)
            => !double.IsNaN(value) && !double.IsInfinity(value) && value > 0d;
    }
}
