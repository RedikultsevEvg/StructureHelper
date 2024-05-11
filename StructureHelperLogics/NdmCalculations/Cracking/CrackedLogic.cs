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
    public class CrackedLogic : ICrackedLogic
    {
        private ISectionCrackedLogic sectionCrackedLogic;
        public IForceTuple StartTuple { get; set; }
        public IForceTuple EndTuple { get; set; }
        public IEnumerable<INdm> NdmCollection { get; set; }
        public IShiftTraceLogger? TraceLogger { get; set; }

        public CrackedLogic(ISectionCrackedLogic sectionLogic)
        {
            sectionCrackedLogic = sectionLogic;
        }
        public CrackedLogic() : this (new SectionCrackedLogic())
        {
            
        }
        public bool IsSectionCracked(double factor)
        {
            sectionCrackedLogic.TraceLogger = TraceLogger?.GetSimilarTraceLogger(50);

            var actualTuple = ForceTupleService.InterpolateTuples(EndTuple, StartTuple, factor);
            sectionCrackedLogic.Tuple = actualTuple;
            sectionCrackedLogic.NdmCollection = NdmCollection;
            return sectionCrackedLogic.IsSectionCracked();
        }
    }
}
