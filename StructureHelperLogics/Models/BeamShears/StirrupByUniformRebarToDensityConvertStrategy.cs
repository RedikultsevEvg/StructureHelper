using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.BeamShears
{
    public class StirrupByUniformRebarToDensityConvertStrategy : IConvertStrategy<IStirrupByDensity, IStirrupByUniformRebar>
    {
        private const double stirrupStrengthFactor = 1d;
        private const double maxStirrupStrength = 3e8;

        private IUpdateStrategy<IStirrup> updateStrategy;
        public Dictionary<(Guid id, Type type), ISaveable> ReferenceDictionary { get; set; }
        public IShiftTraceLogger TraceLogger { get; set; }

        public StirrupByUniformRebarToDensityConvertStrategy(IUpdateStrategy<IStirrup> updateStrategy, IShiftTraceLogger traceLogger)
        {
            this.updateStrategy = updateStrategy;
            TraceLogger = traceLogger;
        }

        public IStirrupByDensity Convert(IStirrupByUniformRebar source)
        {
            updateStrategy ??= new StirrupUpdateStrategy();
            StirrupByDensity stirrupByDensity = new(Guid.NewGuid());
            updateStrategy.Update(stirrupByDensity, source);
            stirrupByDensity.StirrupDensity = GetStirrupDensity(source);
            return stirrupByDensity;
        }

        private double GetStirrupDensity(IStirrupByUniformRebar source)
        {
            double area = Math.PI * source.Diameter * source.Diameter / 4d;
            TraceLogger?.AddMessage($"Area of rebar = {Math.PI} * ({source.Diameter})^2 / 4 = {area}, m^2");
            double strength = stirrupStrengthFactor * source.Material.GetStrength(LimitStates.ULS, CalcTerms.ShortTerm).Tensile;
            TraceLogger?.AddMessage($"Strength of rebar = {strength}, Pa");
            strength = Math.Min(strength, maxStirrupStrength);
            double density = strength * area * source.LegCount / source.Step;
            TraceLogger?.AddMessage($"Density of stirrups = {strength} * {area} * {source.LegCount} / {source.Step} = {density}, N/m");
            return density;
        }
    }
}
