using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;

namespace StructureHelperLogics.Models.BeamShears
{
    public class StirrupByRebarToDensityConvertStrategy : IConvertStrategy<IStirrupByDensity, IStirrupByRebar>
    {
        private const double stirrupStrengthFactor = 0.8d;
        private const double maxStirrupStrength = 3e8;
        private IInclinedSection inclinedSection;
        private IUpdateStrategy<IStirrup> updateStrategy;
        public Dictionary<(Guid id, Type type), ISaveable> ReferenceDictionary { get; set; }
        public IShiftTraceLogger TraceLogger { get; set; }

        public StirrupByRebarToDensityConvertStrategy(IShiftTraceLogger traceLogger, IInclinedSection inclinedSection)
        {
            TraceLogger = traceLogger;
            this.inclinedSection = inclinedSection;
        }

        public StirrupByRebarToDensityConvertStrategy(IUpdateStrategy<IStirrup> updateStrategy, IShiftTraceLogger traceLogger)
        {
            this.updateStrategy = updateStrategy;
            TraceLogger = traceLogger;
        }

        public IStirrupByDensity Convert(IStirrupByRebar source)
        {
            updateStrategy ??= new StirrupBaseUpdateStrategy();
            StirrupByDensity stirrupByDensity = new(Guid.NewGuid());
            updateStrategy.Update(stirrupByDensity, source);
            stirrupByDensity.StirrupDensity = GetStirrupDensity(source);
            return stirrupByDensity;
        }

        private double GetStirrupDensity(IStirrupByRebar source)
        {
            double area = Math.PI * source.Diameter * source.Diameter / 4d;
            TraceLogger?.AddMessage($"Area of rebar = {Math.PI} * ({source.Diameter})^2 / 4 = {area}(m^2)");
            double materialStrength = source.Material.GetStrength(LimitStates.ULS, CalcTerms.ShortTerm).Tensile;
            TraceLogger?.AddMessage($"Stirrup material strength Rsw = {materialStrength}");
            double stirrupStrength = stirrupStrengthFactor * materialStrength;
            TraceLogger?.AddMessage($"Strength of rebar Rsw = {stirrupStrengthFactor} * {materialStrength} = {stirrupStrength}(Pa)");
            double minimizedStrength = Math.Min(stirrupStrength, maxStirrupStrength);
            TraceLogger?.AddMessage($"Strength of rebar Rsw = Min({stirrupStrength}, {maxStirrupStrength})= {minimizedStrength}(Pa)");
            double spiralEffectiveness = 1;
            if (source.IsSpiral = true)
            {
                spiralEffectiveness = GetSpiralEffectiveness(source);
            }
            double density = minimizedStrength * area * source.LegCount / source.Spacing * spiralEffectiveness;
            TraceLogger?.AddMessage($"Density of stirrups = {minimizedStrength} * {area} * {source.LegCount} / {source.Spacing} * {spiralEffectiveness} = {density}(N/m)");
            return density;
        }

        private double GetSpiralEffectiveness(IStirrupByRebar source)
        {
            if (inclinedSection is null)
            {
                string errorString = ErrorStrings.ParameterIsNull + ": Inclined Section";
                TraceLogger?.AddMessage(errorString, TraceLogStatuses.Error);
                throw new StructureHelperException(errorString);
            }
            double spiralHeight = inclinedSection.EffectiveDepth - (inclinedSection.FullDepth - inclinedSection.EffectiveDepth);
            TraceLogger?.AddMessage($"Spiral height = {spiralHeight}(m)");
            double spiralSpacing = source.Spacing;
            TraceLogger?.AddMessage($"Spiral spacing = {spiralSpacing}(m)");
            double spiralAng = Math.Atan2(spiralHeight, spiralSpacing);
            double spriralEffectiveness = Math.Sin(spiralAng);
            double spiralAngInDegrees = 180 / (Math.PI) * spiralAng;
            TraceLogger?.AddMessage($"Spiral effectiveness factor = sin({spiralAngInDegrees}(deg)) = {spriralEffectiveness}");
            return spriralEffectiveness;
        }
    }
}
