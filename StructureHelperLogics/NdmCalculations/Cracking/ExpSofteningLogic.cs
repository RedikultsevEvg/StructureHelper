using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    public class ExpSofteningLogic : ICrackSofteningLogic
    {
        private double forceRatio;
        private double powerFactor;

        public double PowerFactor
        {
            get => powerFactor;
            set
            {
                if (value < 0)
                {
                    throw new StructureHelperException(ErrorStrings.DataIsInCorrect + ": Power Factor must not be less than zero");
                }
                powerFactor = value;
            }
        }
        public double BettaFactor { get; set; }
        public double ForceRatio
        {
            get => forceRatio;
            set
            {
                if (value < 0)
                {
                    throw new StructureHelperException(ErrorStrings.DataIsInCorrect + ": Force Ratio must not be less than zero");
                }
                else if (value > 1)
                {
                    throw new StructureHelperException(ErrorStrings.DataIsInCorrect + ": Force Ratio must not be greater than 1.0");
                }
                forceRatio = value;
            }
        }
        public double PsiSMin {get;set;}
        public IShiftTraceLogger? TraceLogger { get; set; }

        public ExpSofteningLogic()
        {
            PsiSMin = 0.2d;
            PowerFactor = 2d;
            BettaFactor = 0.8d;
        }
        public double GetSofteningFactor()
        {
            TraceLogger?.AddMessage($"Calculator type: {GetType()}", TraceLogStatuses.Service);
            TraceLogger?.AddMessage($"Logic of calculation of psi_s factor based on exponential softening model");
            TraceLogger?.AddMessage($"psi_s = 1 - BettaFactor * ForceRatio ^ PowerFactor");
            TraceLogger?.AddMessage($"But not less than psi_s_min = {PsiSMin}");
            TraceLogger?.AddMessage($"BettaFactor = {BettaFactor}");
            TraceLogger?.AddMessage($"ForceRatio = {ForceRatio}");
            TraceLogger?.AddMessage($"PowerFactor = {BettaFactor}");
            double psi;
            psi = 1 - BettaFactor * Math.Pow(ForceRatio, PowerFactor);
            TraceLogger?.AddMessage($"psi_s = 1 - BettaFactor * ForceRatio ^ PowerFactor = 1 - {BettaFactor} * {ForceRatio} ^ {PowerFactor} = {psi}");
            double psi_c = Math.Max(psi, PsiSMin);
            TraceLogger?.AddMessage($"Since psi_s = {psi} and psi_s_min = {PsiSMin},\npsi_s = {psi_c}");
            return psi_c;
        }
    }
}
