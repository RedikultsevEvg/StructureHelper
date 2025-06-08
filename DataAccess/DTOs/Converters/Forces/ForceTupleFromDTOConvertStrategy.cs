using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Forces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs
{
    public class ForceTupleFromDTOConvertStrategy : ConvertStrategy<ForceTuple, ForceTupleDTO>
    {
        private readonly IUpdateStrategy<IForceTuple> updateStrategy;

        public ForceTupleFromDTOConvertStrategy(IUpdateStrategy<IForceTuple> updateStrategy)
        {
            this.updateStrategy = updateStrategy;
        }

        public ForceTupleFromDTOConvertStrategy() : this(new ForceTupleUpdateStrategy())
        {
            
        }

        public override ForceTuple GetNewItem(ForceTupleDTO source)
        {
            ForceTuple newItem = new(source.Id);
            updateStrategy.Update(newItem, source);
            return newItem;
        }
    }
}
