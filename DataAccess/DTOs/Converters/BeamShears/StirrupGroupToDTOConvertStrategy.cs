using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperLogics.Models.BeamShears;
using StructureHelperLogics.Models.BeamShears.Logics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs
{
    internal class StirrupGroupToDTOConvertStrategy : ConvertStrategy<StirrupGroupDTO, IStirrupGroup>
    {
        private IUpdateStrategy<IStirrupGroup> updateStrategy;
        private IUpdateStrategy<IHasStirrups> stirrupUpdateStrategy;

        public StirrupGroupToDTOConvertStrategy(
            Dictionary<(Guid id, Type type), ISaveable> referenceDictionary,
            IShiftTraceLogger traceLogger)
            : base(referenceDictionary, traceLogger)
        {
        }

        public override StirrupGroupDTO GetNewItem(IStirrupGroup source)
        {
            InitializeStrategies();
            ChildClass = this;
            NewItem = new(source.Id);
            updateStrategy.Update(NewItem, source);
            stirrupUpdateStrategy.Update(NewItem, source);
            return NewItem;
        }

        private void InitializeStrategies()
        {
            updateStrategy ??= new StirrupGroupUpdateStrategy();
            stirrupUpdateStrategy ??= new HasStirrupsToDTOUpdateStrategy(ReferenceDictionary, TraceLogger);
        }
    }
}
