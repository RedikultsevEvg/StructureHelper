using StructureHelperCommon.Models.Forces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Sections.Logics
{
    public class ForceTupleCopier : IProcessorLogic<IForceTuple>
    {
        public IForceTuple InputForceTuple { get; set; }
        public IShiftTraceLogger? TraceLogger { get; set; }
        public ForceTupleCopier(IForceTuple forceTuple)
        {
            InputForceTuple = forceTuple;
        }
        public IForceTuple GetValue()
        {
            return InputForceTuple.Clone() as IForceTuple;
        }
    }
}
