using StructureHelper.Infrastructure.UI.DataContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelper.Services.Primitives
{
    public class PrimitiveRepository : IPrimitiveRepository
    {
        List<PrimitiveBase> primitives;

        public IEnumerable<PrimitiveBase> Primitives { get => primitives; }

        public PrimitiveRepository()
        {
            primitives = new List<PrimitiveBase>();
        }

        public void Add(PrimitiveBase primitive)
        {
            primitives.Add(primitive);
        }

        public void Clear()
        {
            primitives = new List<PrimitiveBase>();
        }

        public void Delete(PrimitiveBase primitive)
        {
            primitives.Remove(primitive);
        }
    }
}
