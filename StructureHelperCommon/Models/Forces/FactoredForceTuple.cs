using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Forces
{
    public class FactoredForceTuple : IFactoredForceTuple
    {
        public Guid Id { get; }
        public IForceTuple ForceTuple { get; set; } = new ForceTuple();
        public IFactoredCombinationProperty CombinationProperty { get; set; } = new FactoredCombinationProperty(Guid.NewGuid());

        public FactoredForceTuple(Guid id)
        {
            Id = id;
        }

        public object Clone()
        {
            throw new NotImplementedException();
        }
    }
}
