using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Sections.Logics
{
    public class ForceTupleMoveToPointDecorator : IProcessorDecorator<IForceTuple>
    {
        public IPoint2D Point2D { get; set; }
        public IShiftTraceLogger? TraceLogger { get; set; }
        /// <summary>
        /// Internal source of ForceTuple
        /// </summary>
        public IProcessorLogic<IForceTuple> ForceTupleLogics { get; }
        public ForceTupleMoveToPointDecorator(IProcessorLogic<IForceTuple> procLogic)
        {
            ForceTupleLogics = procLogic;
        }
        public IForceTuple GetValue()
        {
            ForceTupleLogics.TraceLogger = TraceLogger;
            var newTuple = ForceTupleLogics.GetValue();
            newTuple.Mx += newTuple.Nz * Point2D.Y;
            newTuple.My -= newTuple.Nz * Point2D.X;
            return newTuple;
        }
    }
}
