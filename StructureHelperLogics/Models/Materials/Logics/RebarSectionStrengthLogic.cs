using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.Materials
{
    public class RebarSectionStrengthLogic : IRebarSectionStrengthLogic
    {
        public double RebarStrengthFactor { get; set; } = 1;
        public double MaxRebarStrength { get; set; } = double.PositiveInfinity;
        /// <summary>
        /// Limit state, default - ULS
        /// </summary>
        public LimitStates LimitState { get; set; } = LimitStates.ULS;
        /// <summary>
        /// Calc term, default - short term
        /// </summary>
        public CalcTerms CalcTerm { get; set; } = CalcTerms.ShortTerm;
        public IRebarSection RebarSection { get; set; }
        public IShiftTraceLogger? TraceLogger { get; set; }
        public double GetRebarMaxTensileForce()
        {
            double rebarArea = Math.PI * RebarSection.Diameter * RebarSection.Diameter / 4;
            TraceLogger?.AddMessage($"Rebar area = {rebarArea}(m2)");
            double materialStrength = RebarSection.Material.GetStrength(LimitState, CalcTerm).Tensile;
            TraceLogger?.AddMessage($"Stirrup material strength Rsw = {materialStrength}");
            double rebarStrength = RebarStrengthFactor * materialStrength;
            TraceLogger?.AddMessage($"Strength of rebar Rs = {RebarStrengthFactor} * {materialStrength} = {rebarStrength}(Pa)");
            double minimizedStrength = Math.Min(rebarStrength, MaxRebarStrength);
            TraceLogger?.AddMessage($"Strength of rebar Rs = Min({rebarStrength}, {MaxRebarStrength})= {minimizedStrength}(Pa)");
            double rebarForce = minimizedStrength * rebarArea;
            TraceLogger?.AddMessage($"Force in rebar Ns = {minimizedStrength}(Pa) * {rebarArea}(m2) = {rebarForce}");
            return rebarForce;
        }
    }
}
