using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Loggers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs
{
    public class ForceTupleToDTOConvertStrategy : ConvertStrategy<ForceTupleDTO, IForceTuple>
    {
        private IUpdateStrategy<IForceTuple> updateStrategy;

        public ForceTupleToDTOConvertStrategy(IUpdateStrategy<IForceTuple> updateStrategy)
        {
            this.updateStrategy = updateStrategy;
        }

        public ForceTupleToDTOConvertStrategy() : this(new ForceTupleUpdateStrategy())
        {
            
        }

        public override ForceTupleDTO GetNewItem(IForceTuple source)
        {
            ForceTupleDTO newItem = new(source.Id);
            updateStrategy.Update(newItem, source);
            return newItem;
        }
    }
}
