using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Calculators;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperLogics.Models.LoadHistory
{
    public class HistoryCalculator : IHistoryCalculator
    {
        public Guid Id { get; }
        public string Name { get; set; } = string.Empty;
        public bool ShowTraceData { get; set; }
        public List<IConstructionStage> ConstructionStages { get; } = [];
        public IShiftTraceLogger? TraceLogger { get; set; }

        public IResult Result => throw new NotImplementedException();


        public HistoryCalculator(Guid id, IShiftTraceLogger traceLogger = null)
        {
            Id = id;
            TraceLogger = traceLogger;
        }
        public void Run()
        {
            throw new NotImplementedException();
        }

        public object Clone()
        {
            HistoryCalculatorUpdateStrategy updateStrategy = new() { UpdateChildren = true };
            HistoryCalculator newItem = new(Guid.NewGuid());
            updateStrategy.Update(newItem, this);
            return newItem;
        }

    }
}
