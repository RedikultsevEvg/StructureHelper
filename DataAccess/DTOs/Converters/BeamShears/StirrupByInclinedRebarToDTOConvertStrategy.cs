using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperLogics.Models.BeamShears;
using StructureHelperLogics.Models.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs
{
    public class StirrupByInclinedRebarToDTOConvertStrategy : ConvertStrategy<StirrupByInclinedRebarDTO, IStirrupByInclinedRebar>
    {
        private IUpdateStrategy<IStirrupByInclinedRebar> updateStrategy;
        private IConvertStrategy<RebarSectionDTO, IRebarSection> rebarConvertStrategy;

        public StirrupByInclinedRebarToDTOConvertStrategy(Dictionary<(Guid id, Type type), ISaveable> referenceDictionary, IShiftTraceLogger traceLogger) : base(referenceDictionary, traceLogger)
        {
        }

        public override StirrupByInclinedRebarDTO GetNewItem(IStirrupByInclinedRebar source)
        {
            TraceLogger?.AddMessage($"Stirrup by inclinated rebar converting Id = {source.Id} has been started", TraceLogStatuses.Debug);
            updateStrategy ??= new StirrupByInclinedRebarUpdateStrategy();
            rebarConvertStrategy ??= new RebarSectionToDTOConvertStrategy(this);
            NewItem = new(source.Id);
            updateStrategy.Update(NewItem, source);
            NewItem.RebarSection = rebarConvertStrategy.Convert(source.RebarSection);
            TraceLogger?.AddMessage($"Stirrup by inclinated rebar converting Id = {NewItem.Id} has been finished succesfully", TraceLogStatuses.Debug);
            return NewItem;
        }
    }
}
