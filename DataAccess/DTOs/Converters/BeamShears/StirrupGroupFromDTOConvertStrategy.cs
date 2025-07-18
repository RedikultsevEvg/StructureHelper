using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperLogics.Models.BeamShears;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs
{
    public class StirrupGroupFromDTOConvertStrategy : ConvertStrategy<StirrupGroup, IStirrupGroup>
    {
        private IUpdateStrategy<IStirrupGroup> updateStrategy;
        private IUpdateStrategy<IHasStirrups> hasStirrupsUpdateStrategy;

        public StirrupGroupFromDTOConvertStrategy(Dictionary<(Guid id, Type type), ISaveable> referenceDictionary, IShiftTraceLogger traceLogger) : base(referenceDictionary, traceLogger)
        {
        }

        public override StirrupGroup GetNewItem(IStirrupGroup source)
        {
            InitializeStrategies();
            ChildClass = this;
            NewItem = new(source.Id);
            updateStrategy.Update(NewItem, source);
            hasStirrupsUpdateStrategy.Update(NewItem, source);
            return NewItem;
        }

        private void InitializeStrategies()
        {
            updateStrategy ??= new StirrupGroupUpdateStrategy() { UpdateChildren = false};
            hasStirrupsUpdateStrategy ??= new HasStirrupsFromDTOUpdateStrategy(ReferenceDictionary, TraceLogger);
        }
    }
}
