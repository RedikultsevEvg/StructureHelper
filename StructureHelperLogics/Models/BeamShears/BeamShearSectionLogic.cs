using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Calculators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.BeamShears
{
    public class BeamShearSectionLogic : IBeamShearSectionLogic
    {
        private IBeamShearSectionLogicResult result;

        public IBeamShearSectionLogicInputData InputData { get; set; }
        public IShiftTraceLogger? TraceLogger { get; set; }

        public IResult Result => result;


        public void Run()
        {
            throw new NotImplementedException();
        }
    }
}
