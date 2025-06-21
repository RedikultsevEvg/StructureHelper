using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;

namespace StructureHelperLogics.Models.BeamShears
{
    public class StirrupByRebarToDensityConvertStrategy : IConvertStrategy<IStirrupByDensity, IStirrupByRebar>
    {
        private const double stirrupStrengthFactor = 0.8d;
        private const double maxStirrupStrength = 3e8;
        private IUpdateStrategy<IStirrup> updateStrategy;
        public Dictionary<(Guid id, Type type), ISaveable> ReferenceDictionary { get; set; }
        public IShiftTraceLogger TraceLogger { get; set; }

        public StirrupByRebarToDensityConvertStrategy(IShiftTraceLogger traceLogger)
        {
            TraceLogger = traceLogger;
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
            double density = minimizedStrength * area * source.LegCount / source.Spacing;
            TraceLogger?.AddMessage($"Density of stirrups = {minimizedStrength} * {area} * {source.LegCount} / {source.Spacing} = {density}(N/m)");
            return density;
        }
    }
}
