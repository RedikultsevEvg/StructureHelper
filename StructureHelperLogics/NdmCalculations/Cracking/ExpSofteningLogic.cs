using StructureHelperCommon.Infrastructures.Exceptions;
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
        public double FiMin {get;set;}
        public ExpSofteningLogic()
        {
            FiMin = 0.2d;
            PowerFactor = 2d;
            BettaFactor = 0.8d;
        }
        public double GetSofteningFactor()
        {
            double fi;
            fi = 1 - BettaFactor * Math.Pow(ForceRatio, PowerFactor);
            fi = Math.Max(fi, FiMin);
            return fi;
        }
    }
}
