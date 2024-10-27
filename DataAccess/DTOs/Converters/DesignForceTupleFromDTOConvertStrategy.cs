using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Forces.Logics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs
{
    public class DesignForceTupleFromDTOConvertStrategy : ConvertStrategy<DesignForceTuple, DesignForceTupleDTO>
    {
        private IUpdateStrategy<IDesignForceTuple> updateStrategy;
        private IConvertStrategy<ForceTuple, ForceTupleDTO> forceTupleConvertStrategy;

        public DesignForceTupleFromDTOConvertStrategy(
            IUpdateStrategy<IDesignForceTuple> updateStrategy,
            IConvertStrategy<ForceTuple, ForceTupleDTO> forceTupleConvertStrategy)
        {
            this.updateStrategy = updateStrategy;
            this.forceTupleConvertStrategy = forceTupleConvertStrategy;
        }

        public DesignForceTupleFromDTOConvertStrategy() : this(
            new DesignForceTupleUpdateStrategy(),
            new ForceTupleFromDTOConvertStrategy())
        {
            
        }

        public override DesignForceTuple GetNewItem(DesignForceTupleDTO source)
        {
            TraceLogger?.AddMessage("Design force tuple converting is started");
            DesignForceTuple newItem = new(source.Id);
            updateStrategy.Update(newItem, source);
            forceTupleConvertStrategy.ReferenceDictionary = ReferenceDictionary;
            forceTupleConvertStrategy.TraceLogger = TraceLogger;
            var convertLogic = new DictionaryConvertStrategy<ForceTuple, ForceTupleDTO>(this, forceTupleConvertStrategy);
            newItem.ForceTuple = convertLogic.Convert((ForceTupleDTO)source.ForceTuple);
            TraceLogger?.AddMessage("Design force tuple converting has been finished");
            return newItem;
        }
    }
}
