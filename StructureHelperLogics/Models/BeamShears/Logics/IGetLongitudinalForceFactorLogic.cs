using StructureHelperCommon.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.BeamShears.Logics
{
    public interface IGetLongitudinalForceFactorLogic : IGetFactorLogic
    {
        public double LongitudinalForce { get; set; }
        public IInclinedSection InclinedSection { get; set; }
    }
}
