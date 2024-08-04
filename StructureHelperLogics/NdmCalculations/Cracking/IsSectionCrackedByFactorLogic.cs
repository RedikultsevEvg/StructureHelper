using LoaderCalculator;
using LoaderCalculator.Data.Ndms;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Services.Forces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    public class IsSectionCrackedByFactorLogic : IIsSectionCrackedByFactorLogic
    {
        public IIsSectionCrackedByForceLogic IsSectionCrackedByForceLogic { get; set; }
        public IForceTuple StartTuple { get; set; }
        public IForceTuple EndTuple { get; set; }
        public IShiftTraceLogger? TraceLogger { get; set; }

        public IsSectionCrackedByFactorLogic(IIsSectionCrackedByForceLogic sectionLogic)
        {
            IsSectionCrackedByForceLogic = sectionLogic;
        }

        public IsSectionCrackedByFactorLogic() : this(new IsSectionCrackedByForceLogic())
        {
            
        }

        public bool IsSectionCracked(double factor)
        {
            IsSectionCrackedByForceLogic.TraceLogger ??= TraceLogger?.GetSimilarTraceLogger(50);
            var actualTuple = ForceTupleService.InterpolateTuples(EndTuple, StartTuple, factor);
            IsSectionCrackedByForceLogic.Tuple = actualTuple;
            return IsSectionCrackedByForceLogic.IsSectionCracked();
        }
    }
}
