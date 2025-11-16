using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperLogics.NdmCalculations.Analyses.ValueDiagrams.Logics;

namespace StructureHelperLogics.NdmCalculations.Analyses.ValueDiagrams
{
    public class ValueDiagramEntityCheckLogic : CheckEntityLogic<IValueDiagramEntity>
    {       
        private ICheckEntityLogic<IValueDiagram> valueDiagramCheckLogic;

        public ValueDiagramEntityCheckLogic(IShiftTraceLogger? traceLogger)
        {
            valueDiagramCheckLogic = new ValueDiagramCheckLogic();
            TraceLogger = traceLogger;
        }

        public ValueDiagramEntityCheckLogic(ICheckEntityLogic<IValueDiagram> valueDiagramCheckLogic, IShiftTraceLogger? traceLogger)
        {
            TraceLogger = traceLogger;
            this.valueDiagramCheckLogic = valueDiagramCheckLogic;
        }

        public override bool Check()
        {
            bool result = true;
            if (Entity is null)
            {
                string errorString = ErrorStrings.ParameterIsNull + ": value diagram entity";
                TraceMessage(errorString);
                throw new StructureHelperException(errorString);
            }
            valueDiagramCheckLogic.Entity = Entity.ValueDiagram;
            bool IsValid = valueDiagramCheckLogic.Check();
            if (IsValid == false)
            {
                result = false;
                string name = string.IsNullOrWhiteSpace(Entity.Name)
                    ? "<unnamed>"
                    : Entity.Name;
                TraceMessage($"Diagram: {name} has error: {valueDiagramCheckLogic.CheckResult}");
            }
            return result;
        }
    }
}
