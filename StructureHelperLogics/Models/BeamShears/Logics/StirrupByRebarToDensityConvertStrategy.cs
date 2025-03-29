using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;

namespace StructureHelperLogics.Models.BeamShears
{
    public class StirrupByRebarToDensityConvertStrategy : IConvertStrategy<IStirrupByDensity, IStirrupByRebar>
    {
        private const double stirrupStrengthFactor = 1d;
        private const double maxStirrupStrength = 3e8;

        private IUpdateStrategy<IStirrup> updateStrategy;
        public Dictionary<(Guid id, Type type), ISaveable> ReferenceDictionary { get; set; }
        public IShiftTraceLogger TraceLogger { get; set; }

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
            TraceLogger?.AddMessage($"Area of rebar = {Math.PI} * ({source.Diameter})^2 / 4 = {area}, m^2");
            double strength = stirrupStrengthFactor * source.Material.GetStrength(LimitStates.ULS, CalcTerms.ShortTerm).Tensile;
            TraceLogger?.AddMessage($"Strength of rebar = {strength}, Pa");
            strength = Math.Min(strength, maxStirrupStrength);
            double density = strength * area * source.LegCount / source.Spacing;
            TraceLogger?.AddMessage($"Density of stirrups = {strength} * {area} * {source.LegCount} / {source.Spacing} = {density}, N/m");
            return density;
        }
    }
}
