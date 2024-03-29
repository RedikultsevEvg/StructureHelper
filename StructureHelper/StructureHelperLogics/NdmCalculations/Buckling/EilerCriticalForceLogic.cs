﻿using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Strings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Buckling
{
    internal class EilerCriticalForceLogic : IEilerCriticalForceLogic
    {
        public double LongForce { get; set; }
        public double StiffnessEI { get; set; }
        public double DesignLength { get; set; }

        public double GetCriticalForce()
        {
            double Ncr = - Math.Pow(Math.PI, 2) * StiffnessEI / (Math.Pow(DesignLength, 2));
            return Ncr;
        }

        public double GetEtaFactor()
        {
            if (LongForce >= 0d) return 1d;
            var Ncr = GetCriticalForce();
            if (LongForce <= Ncr)
            {
                throw new StructureHelperException(ErrorStrings.LongitudinalForceMustBeLessThanCriticalForce);
            }
            double eta = 1 / (1 - LongForce / Ncr);
            return eta;
        }
    }
}
